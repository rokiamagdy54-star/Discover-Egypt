using Tourism.Core.Features.Community.DTOs;
using Microsoft.AspNetCore.Http;

namespace Tourism.Core.Features.Community.Interfaces
{
    public interface ICommunityService
    {
        // Posts
        Task<List<PostDto>> GetFeedAsync(string userId, int page = 1, int size = 20);
        Task<PostDto> GetPostByIdAsync(int postId, string userId);
        Task<PostDto> CreatePostAsync(string userId, CreatePostDto dto);
        Task UpdatePostAsync(int postId, string userId, UpdatePostDto dto);
        Task DeletePostAsync(int postId, string userId);
        Task AddPostImagesAsync(int postId, string userId, List<IFormFile> images);
        Task DeletePostImageAsync(int postId, string userId, int imageId);

        // Likes
        Task LikePostAsync(int postId, string userId);
        Task UnlikePostAsync(int postId, string userId);
        Task LikeCommentAsync(int commentId, string userId);
        Task UnlikeCommentAsync(int commentId, string userId);

        // Comments
        Task<List<CommentDto>> GetCommentsAsync(int postId, string userId);
        Task<CommentDto> CreateCommentAsync(string userId, CreateCommentDto dto);
        Task UpdateCommentAsync(int commentId, string userId, UpdateCommentDto dto);
        Task DeleteCommentAsync(int commentId, string userId);
    }
}