using AutoMapper;
using Tourism.Core.Entities;
using Tourism.Core.Features.CustomPlans.DTOs;
using Tourism.Core.Features.ReadyPlans.DTOs;

namespace Tourism.Core.Mapping
{
    public class PlanProfile : Profile
    {
        public PlanProfile()
        {
            // CustomPlan
            CreateMap<CustomPlan, CustomPlanResponseDto>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<CreateCustomPlanDto, CustomPlan>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore()) 
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.Destination, opt => opt.MapFrom(src => src.Destination));

            // ReadyPlan
            CreateMap<ReadyPlan, ReadyPlanResponseDto>()
                .ForMember(dest => dest.PlaceIds,
                    opt => opt.MapFrom(src => src.PlanPlaces.Select(pp => pp.PlaceId)))
                .ForMember(dest => dest.GuideName,
                    opt => opt.MapFrom(src => src.Guide != null && src.Guide.User != null
                        ? $"{src.Guide.User.FirstName} {src.Guide.User.LastName}"
                        : null))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}