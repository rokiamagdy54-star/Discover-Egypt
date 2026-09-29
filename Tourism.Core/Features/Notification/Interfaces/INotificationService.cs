using Tourism.Core.Features.Notification.DTOs;

namespace Tourism.Core.Features.Notification.Interfaces
{
    public interface INotificationService
    {
        Task<List<NotificationDto>> GetAllAsync(string userId);
        Task<NotificationDto> GetByIdAsync(int id, string userId);
        Task<NotificationDto> CreateAsync(string userId, CreateNotificationDto dto);
        Task MarkAsReadAsync(int id, string userId);
        Task MarkAllAsReadAsync(string userId);
        Task DeleteAsync(int id, string userId);
    }
}