using AutoMapper;
using Tourism.Core.Entities;
using Tourism.Core.Features.Booking.DTOs;

namespace Tourism.Core.Mapping
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<Booking, BookingDto>()
                .ForMember(dest => dest.PlanName,
                    opt => opt.MapFrom(src => src.Plan.Title))
                .ForMember(dest => dest.PlanType,
                    opt => opt.MapFrom(src => src.Plan.GetType().Name))
                .ForMember(dest => dest.GuideName,
                    opt => opt.MapFrom(src => src.Guide != null
                        ? $"{src.Guide.User.FirstName} {src.Guide.User.LastName}"
                        : null))
                .ForMember(dest => dest.StartDate,
                    opt => opt.MapFrom(src => src.BookingStart))
                .ForMember(dest => dest.EndDate,
                    opt => opt.MapFrom(src => src.BookingEnd))
                .ForMember(dest => dest.PaymentMethod,
                    opt => opt.MapFrom(src => src.PaymentMethod.ToString()))
                .ForMember(dest => dest.PaymentStatus,
                    opt => opt.MapFrom(src => src.PaymentStatus.ToString()))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}