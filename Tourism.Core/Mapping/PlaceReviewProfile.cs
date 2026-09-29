using AutoMapper;
using Tourism.Core.Entities;
using Tourism.Core.Features.Review.DTOs;

namespace Tourism.Core.Mapping
{
    public class PlaceReviewProfile : Profile
    {
        public PlaceReviewProfile()
        {
            CreateMap<PlaceReview, PlaceReviewDto>()
                .ForMember(dest => dest.TouristName,
                    opt => opt.MapFrom(src =>
                        $"{src.Tourist.User.FirstName} {src.Tourist.User.LastName}"));
        }
    }
}