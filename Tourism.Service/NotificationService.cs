using AutoMapper;
using Tourism.Core.Entities;
using Tourism.Core.Exceptions;
using Tourism.Core.Features.Notification.DTOs;
using Tourism.Core.Features.Notification.Interfaces;
using Tourism.Core.Interfaces;

namespace Tourism.Service
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Get All
        public async Task<List<NotificationDto>> GetAllAsync(string userId)
        {
            // Notfications should be ordered by CreatedAt, not Id, to ensure correct order
            var notifications = await _unitOfWork.Repository<Notification>().GetAllAsync(
                predicate: n => n.TouristId == userId);

            return _mapper.Map<List<NotificationDto>>(
                notifications.OrderByDescending(n => n.Id));
        }

        // Get By Id 
        public async Task<NotificationDto> GetByIdAsync(int id, string userId)
        {
            var notification = await _unitOfWork.Repository<Notification>().GetByIdAsync(id);

            if (notification == null)
                throw new NotFoundException("Notification not found");

            // Check ownership to prevent unauthorized access to other users' notifications
            if (notification.TouristId != userId)
                throw new ForbiddenException("You don't have access to this notification");

            return _mapper.Map<NotificationDto>(notification);
        }

        // Create
        public async Task<NotificationDto> CreateAsync(string userId, CreateNotificationDto dto)
        {
            var notification = _mapper.Map<Notification>(dto);
            notification.TouristId = userId;

            await _unitOfWork.Repository<Notification>().AddAsync(notification);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<NotificationDto>(notification);
        }

        // Mark As Read
        public async Task MarkAsReadAsync(int id, string userId)
        {
            var notification = await _unitOfWork.Repository<Notification>().GetByIdAsync(id);

            if (notification == null)
                throw new NotFoundException("Notification not found");

            // Check ownership to prevent unauthorized access to other users' notifications
            if (notification.TouristId != userId)
                throw new ForbiddenException("You don't have access to this notification");

            if (notification.IsRead)
                return; // No need to update if it's already read

            notification.IsRead = true;
            _unitOfWork.Repository<Notification>().Update(notification);
            await _unitOfWork.CompleteAsync();
        }

        // Mark All As Read
        public async Task MarkAllAsReadAsync(string userId)
        {
            var unread = await _unitOfWork.Repository<Notification>().GetAllAsync(
                predicate: n => n.TouristId == userId && !n.IsRead);

            foreach (var notification in unread)
            {
                notification.IsRead = true;
                _unitOfWork.Repository<Notification>().Update(notification);
            }

            await _unitOfWork.CompleteAsync();
        }

        // Delete
        public async Task DeleteAsync(int id, string userId)
        {
            var notification = await _unitOfWork.Repository<Notification>().GetByIdAsync(id);

            if (notification == null)
                throw new NotFoundException("Notification not found");

            // Check ownership to prevent unauthorized access to other users' notifications
            if (notification.TouristId != userId)
                throw new ForbiddenException("You don't have access to this notification");

            _unitOfWork.Repository<Notification>().Delete(notification);
            await _unitOfWork.CompleteAsync();
        }
    }
}