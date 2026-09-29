using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tourism.Core.Entities;
using Tourism.Core.Exceptions;
using Tourism.Core.Features.Community.DTOs;
using Tourism.Core.Features.Community.Interfaces;
using Tourism.Core.Features.UploadImage.Interfaces;
using Tourism.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Tourism.Service
{
    public class CommunityService : ICommunityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;

        public CommunityService(IUnitOfWork unitOfWork, IMapper mapper, IUploadService uploadService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
        }

        // ─── Feed ───
        public async Task<List<PostDto>> GetFeedAsync(string userId, int page = 1, int size = 20)
        {
            var posts = await _unitOfWork.Repository<CommunityPost>().GetAllAsync(
                include: q => q
                    .Include(p => p.Author)
                    .Include(p => p.Images)
                    .Include(p => p.Likes)
                    .Include(p => p.Comments));

            var paged = posts
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();

            var dtos = _mapper.Map<List<PostDto>>(paged);

            // IsLikedByMe
            for (int i = 0; i < paged.Count; i++)
                dtos[i].IsLikedByMe = paged[i].Likes.Any(l => l.UserId == userId);

            return dtos;
        }

        // ─── Get Post ───
        public async Task<PostDto> GetPostByIdAsync(int postId, string userId)
        {
            var post = await _unitOfWork.Repository<CommunityPost>().GetFirstAsync(
                predicate: p => p.Id == postId,
                include: q => q
                    .Include(p => p.Author)
                    .Include(p => p.Images)
                    .Include(p => p.Likes)
                    .Include(p => p.Comments));

            if (post == null)
                throw new NotFoundException("Post not found");

            var dto = _mapper.Map<PostDto>(post);
            dto.IsLikedByMe = post.Likes.Any(l => l.UserId == userId);

            return dto;
        }

        // ─── Create Post ───
        public async Task<PostDto> CreatePostAsync(string userId, CreatePostDto dto)
        {
            var post = new CommunityPost
            {
                Content = dto.Content,
                Title = dto.Title,
                AuthorId = userId
            };

            await _unitOfWork.Repository<CommunityPost>().AddAsync(post);

            if (dto.Images != null && dto.Images.Any())
            {
                foreach (var image in dto.Images)
                {
                    var url = await _uploadService.UploadImageAsync(image, "posts");
                    await _unitOfWork.Repository<PostImage>().AddAsync(new PostImage
                    {
                        Post = post,
                        ImageUrl = url
                    });
                }
            }

            await _unitOfWork.CompleteAsync();

            return await GetPostByIdAsync(post.Id, userId);
        }

        // ─── Update Post ───
        public async Task UpdatePostAsync(int postId, string userId, UpdatePostDto dto)
        {
            var post = await _unitOfWork.Repository<CommunityPost>().GetFirstAsync(
                predicate: p => p.Id == postId,
                include: q => q.Include(p => p.Images));

            if (post == null)
                throw new NotFoundException("Post not found");

            if (post.AuthorId != userId)
                throw new ForbiddenException("You don't have access to this post");

            if (dto.Content != null) post.Content = dto.Content;
            if (dto.Title != null) post.Title = dto.Title;
            post.UpdatedAt = DateTime.UtcNow;
            post.IsEdited = true;

            // Delete specified images
            if (dto.DeleteImageIds != null && dto.DeleteImageIds.Any())
            {
                var toDelete = post.Images
                    .Where(i => dto.DeleteImageIds.Contains(i.Id))
                    .ToList();

                foreach (var img in toDelete)
                    _unitOfWork.Repository<PostImage>().Delete(img);
            }

            // Add new images
            if (dto.NewImages != null && dto.NewImages.Any())
            {
                foreach (var image in dto.NewImages)
                {
                    var url = await _uploadService.UploadImageAsync(image, "posts");
                    await _unitOfWork.Repository<PostImage>().AddAsync(new PostImage
                    {
                        PostId = postId,
                        ImageUrl = url
                    });
                }
            }

            _unitOfWork.Repository<CommunityPost>().Update(post);
            await _unitOfWork.CompleteAsync();
        }

        // ─── Delete Post ───
        public async Task DeletePostAsync(int postId, string userId)
        {
            var post = await _unitOfWork.Repository<CommunityPost>().GetByIdAsync(postId);

            if (post == null)
                throw new NotFoundException("Post not found");

            if (post.AuthorId != userId)
                throw new ForbiddenException("You don't have access to this post");

            _unitOfWork.Repository<CommunityPost>().Delete(post);
            await _unitOfWork.CompleteAsync();
        }

        // ─── Post Images ───
        public async Task AddPostImagesAsync(int postId, string userId, List<IFormFile> images)
        {
            var post = await _unitOfWork.Repository<CommunityPost>().GetByIdAsync(postId);

            if (post == null)
                throw new NotFoundException("Post not found");

            if (post.AuthorId != userId)
                throw new ForbiddenException("You don't have access to this post");

            foreach (var image in images)
            {
                var url = await _uploadService.UploadImageAsync(image, "posts");
                await _unitOfWork.Repository<PostImage>().AddAsync(new PostImage
                {
                    PostId = postId,
                    ImageUrl = url
                });
            }

            await _unitOfWork.CompleteAsync();
        }

        public async Task DeletePostImageAsync(int postId, string userId, int imageId)
        {
            var post = await _unitOfWork.Repository<CommunityPost>().GetByIdAsync(postId);

            if (post == null)
                throw new NotFoundException("Post not found");

            if (post.AuthorId != userId)
                throw new ForbiddenException("You don't have access to this post");

            var image = await _unitOfWork.Repository<PostImage>().GetFirstAsync(
                predicate: i => i.Id == imageId && i.PostId == postId);

            if (image == null)
                throw new NotFoundException("Image not found");

            _unitOfWork.Repository<PostImage>().Delete(image);
            await _unitOfWork.CompleteAsync();
        }

        // ─── Post Likes ───
        public async Task LikePostAsync(int postId, string userId)
        {
            var post = await _unitOfWork.Repository<CommunityPost>().GetByIdAsync(postId);

            if (post == null)
                throw new NotFoundException("Post not found");

            var exists = await _unitOfWork.Repository<PostLike>().GetFirstAsync(
                predicate: l => l.PostId == postId && l.UserId == userId);

            if (exists != null)
                throw new ConflictException("You already liked this post");

            await _unitOfWork.Repository<PostLike>().AddAsync(new PostLike
            {
                PostId = postId,
                UserId = userId
            });

            await _unitOfWork.CompleteAsync();
        }

        public async Task UnlikePostAsync(int postId, string userId)
        {
            var like = await _unitOfWork.Repository<PostLike>().GetFirstAsync(
                predicate: l => l.PostId == postId && l.UserId == userId);

            if (like == null)
                throw new NotFoundException("You haven't liked this post");

            _unitOfWork.Repository<PostLike>().Delete(like);
            await _unitOfWork.CompleteAsync();
        }

        // ─── Comment Likes ───
        public async Task LikeCommentAsync(int commentId, string userId)
        {
            var comment = await _unitOfWork.Repository<Comment>().GetByIdAsync(commentId);

            if (comment == null)
                throw new NotFoundException("Comment not found");

            var exists = await _unitOfWork.Repository<CommentLike>().GetFirstAsync(
                predicate: l => l.CommentId == commentId && l.UserId == userId);

            if (exists != null)
                throw new ConflictException("You already liked this comment");

            await _unitOfWork.Repository<CommentLike>().AddAsync(new CommentLike
            {
                CommentId = commentId,
                UserId = userId
            });

            await _unitOfWork.CompleteAsync();
        }

        public async Task UnlikeCommentAsync(int commentId, string userId)
        {
            var like = await _unitOfWork.Repository<CommentLike>().GetFirstAsync(
                predicate: l => l.CommentId == commentId && l.UserId == userId);

            if (like == null)
                throw new NotFoundException("You haven't liked this comment");

            _unitOfWork.Repository<CommentLike>().Delete(like);
            await _unitOfWork.CompleteAsync();
        }

        // ─── Comments ───
        public async Task<List<CommentDto>> GetCommentsAsync(int postId, string userId)
        {
            // Return only top-level comments, and include their replies in the same query to avoid N+1 problem
            var comments = await _unitOfWork.Repository<Comment>().GetAllAsync(
                predicate: c => c.PostId == postId && c.ParentCommentId == null,
                include: q => q
                    .Include(c => c.Author)
                    .Include(c => c.Images)
                    .Include(c => c.Likes)
                    .Include(c => c.Replies).ThenInclude(r => r.Author)
                    .Include(c => c.Replies).ThenInclude(r => r.Images)
                    .Include(c => c.Replies).ThenInclude(r => r.Likes));

            var dtos = _mapper.Map<List<CommentDto>>(
                comments.OrderBy(c => c.CreatedAt));

            // IsLikedByMe for comments and their replies
            for (int i = 0; i < comments.Count; i++)
            {
                var comment = comments.ElementAt(i);
                dtos[i].IsLikedByMe = comment.Likes.Any(l => l.UserId == userId);

                var replies = comment.Replies.OrderBy(r => r.CreatedAt).ToList();
                for (int j = 0; j < replies.Count; j++)
                    dtos[i].Replies[j].IsLikedByMe = replies[j].Likes.Any(l => l.UserId == userId);
            }

            return dtos;
        }

        // ─── Create Comment / Reply ───
        public async Task<CommentDto> CreateCommentAsync(string userId, CreateCommentDto dto)
        {
            var post = await _unitOfWork.Repository<CommunityPost>().GetByIdAsync(dto.PostId);

            if (post == null)
                throw new NotFoundException("Post not found");

            // check if it's a reply and validate parent comment
            if (dto.ParentCommentId.HasValue)
            {
                var parent = await _unitOfWork.Repository<Comment>().GetByIdAsync(dto.ParentCommentId.Value);

                if (parent == null)
                    throw new NotFoundException("Comment not found");

                if (parent.ParentCommentId != null)
                    throw new ValidationException("Cannot reply to a reply");
            }

            var comment = new Comment
            {
                Content = dto.Content,
                AuthorId = userId,
                PostId = dto.PostId,
                ParentCommentId = dto.ParentCommentId
            };

            await _unitOfWork.Repository<Comment>().AddAsync(comment);

            if (dto.Images != null && dto.Images.Any())
            {
                foreach (var image in dto.Images)
                {
                    var url = await _uploadService.UploadImageAsync(image, "comments");
                    await _unitOfWork.Repository<CommentImage>().AddAsync(new CommentImage
                    {
                        Comment = comment,
                        ImageUrl = url
                    });
                }
            }

            await _unitOfWork.CompleteAsync();

            // return the created comment with all related data (author, images, likes, replies)
            var created = await _unitOfWork.Repository<Comment>().GetFirstAsync(
                predicate: c => c.Id == comment.Id,
                include: q => q
                    .Include(c => c.Author)
                    .Include(c => c.Images)
                    .Include(c => c.Likes)
                    .Include(c => c.Replies));

            var commentDto = _mapper.Map<CommentDto>(created!);
            commentDto.IsLikedByMe = false;

            return commentDto;
        }

        // ─── Update Comment ───
        public async Task UpdateCommentAsync(int commentId, string userId, UpdateCommentDto dto)
        {
            var comment = await _unitOfWork.Repository<Comment>().GetFirstAsync(
                predicate: c => c.Id == commentId,
                include: q => q.Include(c => c.Images));

            if (comment == null)
                throw new NotFoundException("Comment not found");

            if (comment.AuthorId != userId)
                throw new ForbiddenException("You don't have access to this comment");

            comment.Content = dto.Content;
            comment.UpdatedAt = DateTime.UtcNow;
            comment.IsEdited = true;

            // Delete specified images
            if (dto.DeleteImageIds != null && dto.DeleteImageIds.Any())
            {
                var toDelete = comment.Images
                    .Where(i => dto.DeleteImageIds.Contains(i.Id))
                    .ToList();

                foreach (var img in toDelete)
                    _unitOfWork.Repository<CommentImage>().Delete(img);
            }

            // add new images
            if (dto.NewImages != null && dto.NewImages.Any())
            {
                foreach (var image in dto.NewImages)
                {
                    var url = await _uploadService.UploadImageAsync(image, "comments");
                    await _unitOfWork.Repository<CommentImage>().AddAsync(new CommentImage
                    {
                        CommentId = commentId,
                        ImageUrl = url
                    });
                }
            }

            _unitOfWork.Repository<Comment>().Update(comment);
            await _unitOfWork.CompleteAsync();
        }

        // ─── Delete Comment ───
        public async Task DeleteCommentAsync(int commentId, string userId)
        {
            var comment = await _unitOfWork.Repository<Comment>().GetByIdAsync(commentId);

            if (comment == null)
                throw new NotFoundException("Comment not found");

            if (comment.AuthorId != userId)
                throw new ForbiddenException("You don't have access to this comment");

            _unitOfWork.Repository<Comment>().Delete(comment);
            await _unitOfWork.CompleteAsync();
        }
    }
}