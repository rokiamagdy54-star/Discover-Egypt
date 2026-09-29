using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tourism.Core.Entities;
using Tourism.Core.Enums;
using Tourism.Core.Exceptions;
using Tourism.Core.Features.RequestGuide.DTOs;
using Tourism.Core.Features.RequestGuide.Interfaces;
using Tourism.Core.Interfaces;

namespace Tourism.Service
{
    public class RequestGuideService : IRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RequestGuideService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateRequestAsync(string touristId, CreateRequestDto dto)
        {
            var plan = await _unitOfWork.Repository<CustomPlan>().GetByIdAsync(dto.TripId);

            if (plan == null)
                throw new NotFoundException("Plan not found");

            if (plan.TouristId != touristId)
                throw new ForbiddenException("You don't have access to this plan");

            var existing = await _unitOfWork.Repository<Requset>().GetFirstAsync(
                predicate: r => r.TouristId == touristId &&
                                r.CustomPlanId == dto.TripId &&
                                r.GuideId == dto.GuideId &&
                                r.Status == RequestStatus.Pending);

            if (existing != null)
                throw new ConflictException("A pending request already exists for this guide and plan");

            var request = new Requset
            {
                TouristId = touristId,
                GuideId = dto.GuideId,
                CustomPlanId = dto.TripId,
                Title = plan.Title,
                Status = RequestStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Requset>().AddAsync(request);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<List<RequestDto>> GetGuideRequestsAsync(string guideId)
        {
            var requests = await _unitOfWork.Repository<Requset>().GetAllAsync(
                predicate: r => r.GuideId == guideId,
                include: q => q.Include(r => r.Tourist));

            return _mapper.Map<List<RequestDto>>(
                requests.OrderByDescending(r => r.CreatedAt));
        }

        public async Task<List<RequestDto>> GetTouristRequestsAsync(string touristId)
        {
            var requests = await _unitOfWork.Repository<Requset>().GetAllAsync(
                predicate: r => r.TouristId == touristId,
                include: q => q.Include(r => r.Tourist));

            return _mapper.Map<List<RequestDto>>(
                requests.OrderByDescending(r => r.CreatedAt));
        }

        public async Task<RequestDto> GetRequestDetailsAsync(int requestId, string userId)
        {
            var request = await _unitOfWork.Repository<Requset>().GetFirstAsync(
                predicate: r => r.Id == requestId,
                include: q => q.Include(r => r.Tourist));

            if (request == null)
                throw new NotFoundException("Request not found");

            if (request.GuideId != userId && request.TouristId != userId)
                throw new ForbiddenException("You don't have access to this request");

            return _mapper.Map<RequestDto>(request);
        }

        public async Task AcceptRequestAsync(int requestId, string guideId)
        {
            var request = await _unitOfWork.Repository<Requset>().GetByIdAsync(requestId);

            if (request == null)
                throw new NotFoundException("Request not found");

            if (request.GuideId != guideId)
                throw new ForbiddenException("You don't have access to this request");

            if (request.Status != RequestStatus.Pending)
                throw new ValidationException("Request has already been handled");

            request.Status = RequestStatus.Accepted;

            _unitOfWork.Repository<Requset>().Update(request);
            await _unitOfWork.CompleteAsync();
        }

        public async Task RejectRequestAsync(int requestId, string guideId)
        {
            var request = await _unitOfWork.Repository<Requset>().GetByIdAsync(requestId);

            if (request == null)
                throw new NotFoundException("Request not found");

            if (request.GuideId != guideId)
                throw new ForbiddenException("You don't have access to this request");

            if (request.Status != RequestStatus.Pending)
                throw new ValidationException("Request has already been handled");

            request.Status = RequestStatus.Rejected;

            _unitOfWork.Repository<Requset>().Update(request);
            await _unitOfWork.CompleteAsync();
        }

        public async Task CancelRequestAsync(int requestId, string touristId)
        {
            var request = await _unitOfWork.Repository<Requset>().GetByIdAsync(requestId);

            if (request == null)
                throw new NotFoundException("Request not found");

            if (request.TouristId != touristId)
                throw new ForbiddenException("You don't have access to this request");

            if (request.Status != RequestStatus.Pending)
                throw new ValidationException("Only pending requests can be cancelled");

            request.Status = RequestStatus.Cancelled;

            _unitOfWork.Repository<Requset>().Update(request);
            await _unitOfWork.CompleteAsync();
        }
    }
}