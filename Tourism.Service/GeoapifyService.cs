using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Tourism.Core.Entities;
using Tourism.Core.Exceptions;
using Tourism.Core.Features.Geoapify.Interfaces;
using Tourism.Core.Features.Geoapify.Models;
using Tourism.Core.Interfaces;
using Tourism.Repository.Data.DBContext;
using Microsoft.Extensions.Configuration;

namespace Tourism.Service
{
    public class GeoapifyService : IGeoapifyService
    {
        private readonly HttpClient _httpClient;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;
        private readonly string _apiKey;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private static readonly List<(string Name, double Lat, double Lon)> Cities = new()
        {
            ("Giza",       29.9870, 31.2118),
            ("Cairo",      30.0444, 31.2357),
            ("Luxor",      25.6872, 32.6396),
            ("Aswan",      24.0889, 32.8998),
            ("Alexandria", 31.2001, 29.9187)
        };

        public GeoapifyService(HttpClient httpClient, IUnitOfWork unitOfWork,
            ApplicationDbContext context, IConfiguration config)
        {
            _httpClient = httpClient;
            _unitOfWork = unitOfWork;
            _context = context;
            _apiKey = config["Geoapify:ApiKey"]
                ?? throw new InvalidOperationException("Geoapify API key is not configured");
        }

        public async Task ImportPlacesAsync()
        {
            var existingNamesList = await _context.Places
                                    .Select(p => p.Name.ToLower())
                                    .ToListAsync();

            var existingNames = existingNamesList.ToHashSet();

            var categoryMap = await _context.Categories
                .ToDictionaryAsync(c => c.Name.ToLower(), c => c.Id);

            var placesToAdd = new List<Place>();

            foreach (var city in Cities)
            {
                var url = $"https://api.geoapify.com/v2/places" +
                          $"?categories=tourism.sights,tourism.attraction" +
                          $"&filter=circle:{city.Lon},{city.Lat},3000" +
                          $"&limit=50" +
                          $"&apiKey={_apiKey}";

                var response = await _httpClient.GetStringAsync(url);
                var data = JsonSerializer.Deserialize<GeoResponse>(response, _jsonOptions);

                if (data?.features == null) continue;

                foreach (var item in data.features)
                {
                    var props = item.properties;

                    if (props == null || string.IsNullOrWhiteSpace(props.name))
                        continue;

                    if (existingNames.Contains(props.name.ToLower()))
                        continue;

                    var place = new Place
                    {
                        Name = props.name,
                        Description = props.formatted ?? $"Famous place in {city.Name}",
                        City = city.Name,
                        Location = new Location
                        {
                            Latitude = (decimal)props.lat,
                            Longitude = (decimal)props.lon
                        },
                        AverageVisitDuration = GetVisitDuration(props.categories),
                        TicketPrice = GetTicketPrice(props.categories),
                        OpeningTime = TimeSpan.FromHours(9),
                        ClosingTime = TimeSpan.FromHours(17),

                        CategoryId = MapCategory(props.categories, categoryMap)
                    };

                    placesToAdd.Add(place);
                    existingNames.Add(props.name.ToLower()); // Add to HashSet to prevent duplicates in the same run
                }
            }

            if (placesToAdd.Any())
            {
                await _context.Places.AddRangeAsync(placesToAdd);
                await _unitOfWork.CompleteAsync();
            }
        }

        // Private Helpers

        private static int MapCategory(List<string> categories, Dictionary<string, int> categoryMap)
        {
            if (categories == null || !categories.Any())
                return categoryMap.GetValueOrDefault("other", 1);

            if (categories.Any(c => c.Contains("museum")))
                return categoryMap.GetValueOrDefault("museum", 1);

            if (categories.Any(c => c.Contains("religion") || c.Contains("church") || c.Contains("mosque")))
                return categoryMap.GetValueOrDefault("religious", 1);

            if (categories.Any(c => c.Contains("beach")))
                return categoryMap.GetValueOrDefault("beach", 1);

            if (categories.Any(c => c.Contains("heritage") || c.Contains("historic")))
                return categoryMap.GetValueOrDefault("historical", 1);

            if (categories.Any(c => c.Contains("desert")))
                return categoryMap.GetValueOrDefault("desert", 1);

            if (categories.Any(c => c.Contains("river") || c.Contains("nile")))
                return categoryMap.GetValueOrDefault("nature", 1);

            if (categories.Any(c => c.Contains("mall") || c.Contains("commercial")))
                return categoryMap.GetValueOrDefault("shopping", 1);

            return categoryMap.GetValueOrDefault("other", 1);
        }

        private static TimeSpan GetVisitDuration(List<string> categories)
        {
            if (categories == null) return TimeSpan.FromHours(2);

            if (categories.Any(c => c.Contains("beach"))) return TimeSpan.FromHours(5);
            if (categories.Any(c => c.Contains("museum"))) return TimeSpan.FromHours(2);
            if (categories.Any(c => c.Contains("park"))) return TimeSpan.FromHours(3);

            return TimeSpan.FromHours(2);
        }

        private static decimal GetTicketPrice(List<string> categories)
        {
            if (categories == null) return 0;

            if (categories.Any(c => c.Contains("historic"))) return 150;
            if (categories.Any(c => c.Contains("museum"))) return 100;
            if (categories.Any(c => c.Contains("beach"))) return 50;

            return 0;
        }
    }
}