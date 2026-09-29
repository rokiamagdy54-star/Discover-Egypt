using AutoMapper;
using Tourism.Core.Entities;
using Tourism.Core.Enums;
using Tourism.Core.Exceptions;
using Tourism.Core.Features.ReadyPlans.DTOs;
using Tourism.Core.Features.ReadyPlans.Interfaces;
using Tourism.Core.Features.UploadImage.Interfaces;
using Tourism.Core.Interfaces;

namespace Tourism.Service
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;

        public PlanService(IUnitOfWork unitOfWork, IMapper mapper, IUploadService uploadService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
        }

        // ─── Create ────
        public async Task<ReadyPlanResponseDto> CreateReadyPlanAsync(CreateReadyPlanDto dto)
        {
            if (dto.EndDateTime <= dto.StartDateTime)
                throw new ValidationException("End date must be after start date");

            if (dto.StartDateTime < DateTime.UtcNow)
                throw new ValidationException("Start date cannot be in the past");

            var plan = new ReadyPlan
            {
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                StartDateTime = dto.StartDateTime,
                EndDateTime = dto.EndDateTime,
                GuideId = dto.GuideId,
                CompanyId = dto.CompanyId,
                Status = PlanStatus.Open
            };

            if (dto.Image != null)
                plan.ImageUrl = await _uploadService.UploadImageAsync(dto.Image, "plans");

            await _unitOfWork.Repository<ReadyPlan>().AddAsync(plan);

            if (dto.PlaceIds.Any())
            {
                foreach (var placeId in dto.PlaceIds)
                {
                    await _unitOfWork.Repository<PlanPlace>().AddAsync(new PlanPlace
                    {
                        ReadyPlan = plan,
                        PlaceId = placeId
                    });
                }
            }

            await _unitOfWork.CompleteAsync(); 

            return _mapper.Map<ReadyPlanResponseDto>(plan);
        }

        // ─── Get All ───
        public async Task<List<ReadyPlanResponseDto>> GetAllReadyPlansAsync()
        {
            var plans = await _unitOfWork.ReadyPlans.GetAllWithPlacesAsync();
            return _mapper.Map<List<ReadyPlanResponseDto>>(plans);
        }

        // ─── Get By Id ───
        public async Task<ReadyPlanResponseDto> GetReadyPlanByIdAsync(int id)
        {
            var plan = await _unitOfWork.ReadyPlans.GetByIdWithPlacesAsync(id);

            if (plan == null)
                throw new NotFoundException("Plan not found");

            return _mapper.Map<ReadyPlanResponseDto>(plan);
        }

        // ─── Delete ────
        public async Task DeleteReadyPlanAsync(int id)
        {
            var plan = await _unitOfWork.Repository<ReadyPlan>().GetByIdAsync(id);

            if (plan == null)
                throw new NotFoundException("Plan not found");

            _unitOfWork.Repository<ReadyPlan>().Delete(plan);
            await _unitOfWork.CompleteAsync();
        }
    }
}