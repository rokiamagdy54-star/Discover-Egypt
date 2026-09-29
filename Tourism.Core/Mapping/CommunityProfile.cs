using AutoMapper;
using Tourism.Core.Entities;
using Tourism.Core.Features.Community.DTOs;

namespace Tourism.Core.Mapping
{
    public class CommunityProfile : Profile
    {
        public CommunityProfile()
        {
            CreateMap<CommunityPost, PostDto>()
                .ForMember(dest => dest.AuthorName,
                    opt => opt.MapFrom(src =>
                        $"{src.Author.FirstName} {src.Author.LastName}"))
                .ForMember(dest => dest.AuthorImage,
                    opt => opt.MapFrom(src => src.Author.ImageUrl))
                .ForMember(dest => dest.LikesCount,
                    opt => opt.MapFrom(src => src.Likes.Count))
                .ForMember(dest => dest.CommentsCount,
                    opt => opt.MapFrom(src => src.Comments.Count))
                .ForMember(dest => dest.Images,
                    opt => opt.MapFrom(src => src.Images.Select(i => i.ImageUrl).ToList()))
                .ForMember(dest => dest.IsLikedByMe,
                    opt => opt.Ignore()); 

            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.AuthorName,
                    opt => opt.MapFrom(src =>
                        $"{src.Author.FirstName} {src.Author.LastName}"))
                .ForMember(dest => dest.AuthorImage,
                    opt => opt.MapFrom(src => src.Author.ImageUrl))
                .ForMember(dest => dest.LikesCount,
                    opt => opt.MapFrom(src => src.Likes.Count))
                .ForMember(dest => dest.Images,
                    opt => opt.MapFrom(src => src.Images.Select(i => i.ImageUrl).ToList()))
                .ForMember(dest => dest.Replies,
                    opt => opt.MapFrom(src => src.Replies))
                .ForMember(dest => dest.IsLikedByMe,
                    opt => opt.Ignore());
        }
    }
}