using AutoMapper;
using Tourism.Core.Entities;
using Tourism.Core.Features.Places.DTOs;

namespace Tourism.Core.Mapping
{
    public class PlaceProfile : Profile
    {
        public PlaceProfile()
        {
            CreateMap<Place, PlaceDto>()
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.CategoryNameAr,
                    opt => opt.MapFrom(src => src.Category.NameAr))
                .ForMember(dest => dest.Latitude,
                    opt => opt.MapFrom(src => src.Location.Latitude))
                .ForMember(dest => dest.Longitude,
                    opt => opt.MapFrom(src => src.Location.Longitude))
                .ForMember(dest => dest.Photos,
                    opt => opt.MapFrom(src => src.Photos.Select(p => p.ImageUrl).ToList()));

            CreateMap<CreatePlaceDto, Place>()
                .ForMember(dest => dest.Location,
                    opt => opt.MapFrom(src => new Location
                    {
                        Latitude = src.Latitude,
                        Longitude = src.Longitude
                    }))
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.Photos, opt => opt.Ignore());
        }
    }
}