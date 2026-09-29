using AutoMapper;
using Tourism.Core.Entities;
using Tourism.Core.Features.RequestGuide.DTOs;

namespace Tourism.Core.Mapping
{
    public class RequestGuideProfile : Profile
    {
        public RequestGuideProfile()
        {
            CreateMap<Requset, RequestDto>()
                .ForMember(dest => dest.TripId,
                    opt => opt.MapFrom(src => src.CustomPlanId))
                .ForMember(dest => dest.TouristName,
                    opt => opt.MapFrom(src => $"{src.Tourist.FirstName} {src.Tourist.LastName}"));
        }
    }
}