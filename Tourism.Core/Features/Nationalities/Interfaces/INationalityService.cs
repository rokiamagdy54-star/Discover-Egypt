using Tourism.Core.Features.Nationalities.DTOs;

namespace Tourism.Core.Features.Nationalities.Interfaces
{
    public interface INationalityService
    {
        Task<List<NationalityDto>> GetAllAsync();
        Task<NationalityDto> GetByIdAsync(int id);
        Task<NationalityDto> CreateAsync(CreateNationalityDto dto);
        Task UpdateAsync(int id, CreateNationalityDto dto);
        Task DeleteAsync(int id);
    }
}