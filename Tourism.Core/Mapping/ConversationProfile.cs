using AutoMapper;
using Tourism.Core.Entities;
using Tourism.Core.Features.Conversation.DTOs;
using Tourism.Core.Features.Message.DTOs;

namespace Tourism.Core.Mapping
{
    public class ConversationProfile : Profile
    {
        public ConversationProfile()
        {
            CreateMap<Conversation, ConversationDto>()
                .ForMember(dest => dest.GuideName,
                    opt => opt.MapFrom(src =>
                        src.Guide != null && src.Guide.User != null
                            ? $"{src.Guide.User.FirstName} {src.Guide.User.LastName}"
                            : null))
                .ForMember(dest => dest.TouristName,
                    opt => opt.MapFrom(src =>
                        src.Tourist != null && src.Tourist.User != null
                            ? $"{src.Tourist.User.FirstName} {src.Tourist.User.LastName}"
                            : null))
                .ForMember(dest => dest.UnreadCount,
                    opt => opt.Ignore())
                .ForMember(dest => dest.LastMessage,
                    opt => opt.Ignore());

            CreateMap<Message, MessageDto>();
        }
    }
}