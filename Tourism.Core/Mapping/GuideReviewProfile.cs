using AutoMapper;
using Tourism.Core.Entities;
using Tourism.Core.Features.GuideReviews.DTOs;

namespace Tourism.Core.Mapping
{
    public class GuideReviewProfile : Profile
    {
        public GuideReviewProfile()
        {
            CreateMap<GuideReview, GuideReviewDto>()
                .ForMember(dest => dest.TouristName,
                    opt => opt.MapFrom(src =>
                        $"{src.Tourist.User.FirstName} {src.Tourist.User.LastName}"));
        }
    }
}
