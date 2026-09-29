using AutoMapper;
using Tourism.Core.Entities;
using Tourism.Core.Features.Favorite.DTOs;

namespace Tourism.Core.Mapping
{
    public class FavoriteProfile : Profile
    {
        public FavoriteProfile()
        {
            CreateMap<Favorite, FavoriteDto>()
                .ForMember(dest => dest.PlaceName,
                    opt => opt.MapFrom(src => src.Place.Name))
                .ForMember(dest => dest.City,
                    opt => opt.MapFrom(src => src.Place.City))
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Place.Category.Name))
                .ForMember(dest => dest.AddedAt,
                    opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}