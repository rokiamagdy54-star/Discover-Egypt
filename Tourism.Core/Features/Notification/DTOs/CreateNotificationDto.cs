using System.ComponentModel.DataAnnotations;

namespace Tourism.Core.Features.Notification.DTOs
{
    public class CreateNotificationDto
    {
        [Required, MaxLength(200)]
        public string Title { get; set; }

        [Required, MaxLength(1000)]
        public string Content { get; set; }
    }
}