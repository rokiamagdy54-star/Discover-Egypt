using AutoMapper;
using Tourism.Core.Entities;
using Tourism.Core.Exceptions;
using Tourism.Core.Features.Places.DTOs;
using Tourism.Core.Features.Places.Interfaces;
using Tourism.Core.Features.UploadImage.Interfaces;
using Tourism.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Tourism.Service
{
    public class PlaceService : IPlaceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;

        public PlaceService(IUnitOfWork unitOfWork, IMapper mapper, IUploadService uploadService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
        }

        // ─── Get All ───
        public async Task<List<PlaceDto>> GetAllAsync(string? city = null, int? categoryId = null)
        {
            var places = await _unitOfWork.Repository<Place>().GetAllAsync(
                predicate: p =>
                    (city == null || p.City.ToLower() == city.ToLower()) &&
                    (categoryId == null || p.CategoryId == categoryId),
                include: q => q.Include(p => p.Category)
                               .Include(p => p.Photos));

            return _mapper.Map<List<PlaceDto>>(places);
        }

        // ─── Get By Id ───
        public async Task<PlaceDto> GetByIdAsync(int id)
        {
            var place = await _unitOfWork.Repository<Place>().GetFirstAsync(
                predicate: p => p.Id == id,
                include: q => q.Include(p => p.Category)
                               .Include(p => p.Photos));

            if (place == null)
                throw new NotFoundException("Place not found");

            return _mapper.Map<PlaceDto>(place);
        }

        // ─── Create ───
        public async Task<PlaceDto> CreateAsync(CreatePlaceDto dto)
        {
            var place = _mapper.Map<Place>(dto);

            // Main Image
            if (dto.MainImage != null)
                place.ImageUrl = await _uploadService.UploadImageAsync(dto.MainImage, "places");

            await _unitOfWork.Repository<Place>().AddAsync(place);

            // Photos Collection
            if (dto.Photos != null && dto.Photos.Any())
            {
                foreach (var photo in dto.Photos)
                {
                    var url = await _uploadService.UploadImageAsync(photo, "places");
                    await _unitOfWork.Repository<PlacePhoto>().AddAsync(new PlacePhoto
                    {
                        Place = place,
                        ImageUrl = url
                    });
                }
            }

            await _unitOfWork.CompleteAsync();

            var created = await _unitOfWork.Repository<Place>().GetFirstAsync(
                predicate: p => p.Id == place.Id,
                include: q => q.Include(p => p.Category)
                               .Include(p => p.Photos));

            return _mapper.Map<PlaceDto>(created!);
        }

        // ─── Update ───
        public async Task UpdateAsync(int id, UpdatePlaceDto dto)
        {
            var place = await _unitOfWork.Repository<Place>().GetByIdAsync(id);

            if (place == null)
                throw new NotFoundException("Place not found");

            if (dto.Name != null) place.Name = dto.Name;
            if (dto.Description != null) place.Description = dto.Description;
            if (dto.City != null) place.City = dto.City;
            if (dto.TicketPrice.HasValue) place.TicketPrice = dto.TicketPrice.Value;
            if (dto.CategoryId.HasValue) place.CategoryId = dto.CategoryId.Value;
            if (dto.OpeningTime.HasValue) place.OpeningTime = dto.OpeningTime.Value;
            if (dto.ClosingTime.HasValue) place.ClosingTime = dto.ClosingTime.Value;
            if (dto.AverageVisitDuration.HasValue) place.AverageVisitDuration = dto.AverageVisitDuration.Value;

            if (dto.Latitude.HasValue || dto.Longitude.HasValue)
            {
                place.Location = new Location
                {
                    Latitude = dto.Latitude ?? place.Location.Latitude,
                    Longitude = dto.Longitude ?? place.Location.Longitude
                };
            }

            // Update Main Image
            if (dto.MainImage != null)
                place.ImageUrl = await _uploadService.UploadImageAsync(dto.MainImage, "places");

            _unitOfWork.Repository<Place>().Update(place);
            await _unitOfWork.CompleteAsync();
        }

        // ─── Delete ───
        public async Task DeleteAsync(int id)
        {
            var place = await _unitOfWork.Repository<Place>().GetByIdAsync(id);

            if (place == null)
                throw new NotFoundException("Place not found");

            _unitOfWork.Repository<Place>().Delete(place);
            await _unitOfWork.CompleteAsync();
        }

        // ─── Add Photos ───
        public async Task AddPhotosAsync(int placeId, List<IFormFile> photos)
        {
            var place = await _unitOfWork.Repository<Place>().GetByIdAsync(placeId);

            if (place == null)
                throw new NotFoundException("Place not found");

            foreach (var photo in photos)
            {
                var url = await _uploadService.UploadImageAsync(photo, "places");
                await _unitOfWork.Repository<PlacePhoto>().AddAsync(new PlacePhoto
                {
                    PlaceId = placeId,
                    ImageUrl = url
                });
            }

            await _unitOfWork.CompleteAsync();
        }

        // ─── Delete Photo ───
        public async Task DeletePhotoAsync(int placeId, int photoId)
        {
            var photo = await _unitOfWork.Repository<PlacePhoto>().GetFirstAsync(
                predicate: p => p.Id == photoId && p.PlaceId == placeId);

            if (photo == null)
                throw new NotFoundException("Photo not found");

            _unitOfWork.Repository<PlacePhoto>().Delete(photo);
            await _unitOfWork.CompleteAsync();
        }
    }
}