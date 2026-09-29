using AutoMapper;
using Tourism.Core.Entities;
using Tourism.Core.Features.Notification.DTOs;

namespace Tourism.Core.Mapping
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationDto>();

            CreateMap<CreateNotificationDto, Notification>()
                .ForMember(dest => dest.IsRead,
                    opt => opt.MapFrom(_ => false));
        }
    }
}