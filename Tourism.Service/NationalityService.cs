using AutoMapper;
using Tourism.Core.Entities;
using Tourism.Core.Exceptions;
using Tourism.Core.Features.Nationalities.DTOs;
using Tourism.Core.Features.Nationalities.Interfaces;
using Tourism.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Tourism.Service
{
    public class NationalityService : INationalityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public NationalityService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<List<NationalityDto>> GetAllAsync()
        {
            var nationalities = await _unitOfWork.Repository<Nationality>().GetAllAsync();
            return _mapper.Map<List<NationalityDto>>(nationalities);
        }

        public async Task<NationalityDto> GetByIdAsync(int id)
        {
            var nationality = await _unitOfWork.Repository<Nationality>().GetByIdAsync(id);

            if (nationality == null)
                throw new NotFoundException("Nationality not found");

            return _mapper.Map<NationalityDto>(nationality);
        }

        public async Task<NationalityDto> CreateAsync(CreateNationalityDto dto)
        {
            var exists = await _unitOfWork.Repository<Nationality>().GetFirstAsync(
                predicate: n => n.Name.ToLower() == dto.Name.ToLower());

            if (exists != null)
                throw new ConflictException("Nationality already exists");

            var nationality = _mapper.Map<Nationality>(dto);

            await _unitOfWork.Repository<Nationality>().AddAsync(nationality);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<NationalityDto>(nationality);
        }

        public async Task UpdateAsync(int id, CreateNationalityDto dto)
        {
            var nationality = await _unitOfWork.Repository<Nationality>().GetByIdAsync(id);

            if (nationality == null)
                throw new NotFoundException("Nationality not found");

            nationality.Name = dto.Name;

            _unitOfWork.Repository<Nationality>().Update(nationality);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var nationality = await _unitOfWork.Repository<Nationality>().GetByIdAsync(id);

            if (nationality == null)
                throw new NotFoundException("Nationality not found");

            var isUsed = _userManager.Users.Any(u => u.NationalityId == id);

            if (isUsed)
                throw new ValidationException("Cannot delete nationality that is assigned to users");

            _unitOfWork.Repository<Nationality>().Delete(nationality);
            await _unitOfWork.CompleteAsync();
        }
    }
}