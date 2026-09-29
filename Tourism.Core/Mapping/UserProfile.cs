using AutoMapper;
using Tourism.Core.Entities;
using Tourism.Core.Features.Users.DTOs;

namespace Tourism.Core.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserProfileDto>()
                .ForMember(dest => dest.FirstName,
                    opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName,
                    opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.ImageUrl,
                    opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.Nationality,
                    opt => opt.MapFrom(src => src.Nationality.Name))
                .ForMember(dest => dest.Roles,
                    opt => opt.Ignore()); 
        }
    }
}