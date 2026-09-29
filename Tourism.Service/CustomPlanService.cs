using AutoMapper;
using Tourism.Core.Entities;
using Tourism.Core.Enums;
using Tourism.Core.Exceptions;
using Tourism.Core.Features.CustomPlans.DTOs;
using Tourism.Core.Features.CustomPlans.Interfaces;
using Tourism.Core.Features.UploadImage.Interfaces;
using Tourism.Core.Interfaces;

namespace Tourism.Service
{
    public class CustomPlanService : ICustomPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;

        public CustomPlanService(IUnitOfWork unitOfWork, IMapper mapper, IUploadService uploadService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
        }

        // Create
        public async Task<CustomPlanResponseDto> CreateCustomPlanAsync(string userId, CreateCustomPlanDto dto)
        {
            if (dto.EndDateTime <= dto.StartDateTime)
                throw new ValidationException("End date must be after start date");

            if (dto.StartDateTime < DateTime.UtcNow)
                throw new ValidationException("Start date cannot be in the past");

            var plan = _mapper.Map<CustomPlan>(dto);
            plan.TouristId = userId;
            plan.Status = PlanStatus.Open;
            plan.CreatedAt = DateTime.UtcNow;

            if (dto.Image != null)
                plan.ImageUrl = await _uploadService.UploadImageAsync(dto.Image, "plans");

            await _unitOfWork.Repository<CustomPlan>().AddAsync(plan);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<CustomPlanResponseDto>(plan);
        }

        // Get My Plans
        public async Task<List<CustomPlanResponseDto>> GetMyPlansAsync(string userId)
        {
            // Only return plans that belong to the user
            var plans = await _unitOfWork.Repository<CustomPlan>().GetAllAsync(
                predicate: p => p.TouristId == userId);

            return _mapper.Map<List<CustomPlanResponseDto>>(
                plans.OrderByDescending(p => p.CreatedAt));
        }

        // Get By Id
        public async Task<CustomPlanResponseDto> GetByIdAsync(int id, string userId)
        {
            var plan = await _unitOfWork.Repository<CustomPlan>().GetByIdAsync(id);

            if (plan == null)
                throw new NotFoundException("Plan not found");

            if (plan.TouristId != userId)
                throw new ForbiddenException("You don't have access to this plan");

            return _mapper.Map<CustomPlanResponseDto>(plan);
        }

        // Update
        public async Task UpdateCustomPlanAsync(int id, string userId, UpdateCustomPlanDto dto)
        {
            var plan = await _unitOfWork.Repository<CustomPlan>().GetByIdAsync(id);

            if (plan == null)
                throw new NotFoundException("Plan not found");

            if (plan.TouristId != userId)
                throw new ForbiddenException("You don't have access to this plan");

            if (dto.EndDateTime.HasValue && dto.StartDateTime.HasValue
                && dto.EndDateTime <= dto.StartDateTime)
                throw new ValidationException("End date must be after start date");

            if (dto.Title != null) plan.Title = dto.Title;
            if (dto.Description != null) plan.Description = dto.Description;
            if (dto.StartDateTime.HasValue) plan.StartDateTime = dto.StartDateTime.Value;
            if (dto.EndDateTime.HasValue) plan.EndDateTime = dto.EndDateTime.Value;
            if (dto.Notes != null) plan.Notes = dto.Notes;
            if (dto.Destination != null) plan.Destination = dto.Destination;

            if (dto.Image != null)
                plan.ImageUrl = await _uploadService.UploadImageAsync(dto.Image, "plans");

            _unitOfWork.Repository<CustomPlan>().Update(plan);
            await _unitOfWork.CompleteAsync();
        }

        // Delete
        public async Task DeleteCustomPlanAsync(int id, string userId)
        {
            var plan = await _unitOfWork.Repository<CustomPlan>().GetByIdAsync(id);

            if (plan == null)
                throw new NotFoundException("Plan not found");

            if (plan.TouristId != userId)
                throw new ForbiddenException("You don't have access to this plan");

            _unitOfWork.Repository<CustomPlan>().Delete(plan);
            await _unitOfWork.CompleteAsync();
        }

        // Get All (Admin)
        public async Task<List<CustomPlanResponseDto>> GetAllAsync()
        {
            var plans = await _unitOfWork.Repository<CustomPlan>().GetAllAsync();
            return _mapper.Map<List<CustomPlanResponseDto>>(
                plans.OrderByDescending(p => p.CreatedAt));
        }
    }
}