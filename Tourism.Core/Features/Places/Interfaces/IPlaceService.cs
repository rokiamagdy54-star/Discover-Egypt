using Tourism.Core.Features.Places.DTOs;
using Microsoft.AspNetCore.Http;

namespace Tourism.Core.Features.Places.Interfaces
{
    public interface IPlaceService
    {
        Task<List<PlaceDto>> GetAllAsync(string? city = null, int? categoryId = null);
        Task<PlaceDto> GetByIdAsync(int id);
        Task<PlaceDto> CreateAsync(CreatePlaceDto dto);
        Task UpdateAsync(int id, UpdatePlaceDto dto);
        Task DeleteAsync(int id);
        Task AddPhotosAsync(int placeId, List<IFormFile> photos);
        Task DeletePhotoAsync(int placeId, int photoId);
    }
}