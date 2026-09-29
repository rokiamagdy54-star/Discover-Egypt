using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tourism.Core.Features.Community.DTOs;
using Tourism.Core.Features.Community.Interfaces;

namespace DiscoverEgypt.API.Controllers
{
    [ApiController]
    [Route("api/community")]
    [Authorize]
    public class CommunityController : ControllerBase
    {
        private readonly ICommunityService _service;

        public CommunityController(ICommunityService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get the feed of posts for the authenticated user, including posts from followed users and popular posts.
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetFeed(
            [FromQuery] int page = 1,
            [FromQuery] int size = 20)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _service.GetFeedAsync(userId, page, size);
            return Ok(result);
        }

        /// <summary>
        /// Get a specific post by its ID, including details like content, images, author info, comments, and likes.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _service.GetPostByIdAsync(id, userId);
            return Ok(result);
        }

        /// <summary>
        /// Create a new post with content, optional title, and images. The post will be associated with the authenticated user.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _service.CreatePostAsync(userId, dto);
            return StatusCode(201, result);
        }

        /// <summary>
        /// Update an existing post by its ID. The user can update the content, title, add new images, or delete existing images. Only the author of the post can perform this action.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromForm] UpdatePostDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _service.UpdatePostAsync(id, userId, dto);
            return Ok(new { message = "Post updated successfully" });
        }

        /// <summary>
        /// Delete a post by its ID. Only the author of the post can perform this action. This will also delete all associated comments and likes.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _service.DeletePostAsync(id, userId);
            return Ok(new { message = "Post deleted successfully" });
        }

        /// <summary>
        /// Add images to an existing post. The user can upload multiple images at once. Only the author of the post can perform this action.
        /// </summary>
        [HttpPost("{id}/images")]
        public async Task<IActionResult> AddPostImages(int id, [FromForm] List<IFormFile> images)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _service.AddPostImagesAsync(id, userId, images);
            return Ok(new { message = "Images added successfully" });
        }

        /// <summary>
        /// Delete an image from a post by its image ID. Only the author of the post can perform this action. This will remove the image from the post but keep the post and other content intact.
        /// </summary>
        [HttpDelete("{id}/images/{imageId}")]
        public async Task<IActionResult> DeletePostImage(int id, int imageId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _service.DeletePostImageAsync(id, userId, imageId);
            return Ok(new { message = "Image deleted successfully" });
        }

        /// <summary>
        /// Like a post by its ID. The authenticated user can like any post, but can only like a post once. If the user has already liked the post, this action will have no effect.
        /// </summary>
        [HttpPost("{id}/like")]
        public async Task<IActionResult> LikePost(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _service.LikePostAsync(id, userId);
            return Ok(new { message = "Post liked" });
        }

        /// <summary>
        /// Unlike a post by its ID. The authenticated user can unlike a post that they have previously liked. If the user has not liked the post before, this action will have no effect.
        /// </summary>
        [HttpDelete("{id}/like")]
        public async Task<IActionResult> UnlikePost(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _service.UnlikePostAsync(id, userId);
            return Ok(new { message = "Post unliked" });
        }

        /// <summary>
        /// Get all comments for a specific post by its ID. This will return a list of comments, including the comment content, author information, and any likes on the comments. The authenticated user's like status on each comment will also be included.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("{id}/comments")]
        public async Task<IActionResult> GetComments(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _service.GetCommentsAsync(id, userId);
            return Ok(result);
        }

        /// <summary>
        /// Create a new comment on a specific post. The comment can be a top-level comment or a reply to another comment (if ParentCommentId is provided). The authenticated user will be the author of the comment. The comment can also include multiple images.
        /// </summary>
        [HttpPost("comments")]
        public async Task<IActionResult> CreateComment([FromForm] CreateCommentDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _service.CreateCommentAsync(userId, dto);
            return StatusCode(201, result);
        }

        /// <summary>
        /// Update an existing comment by its ID. The user can update the content, add new images, or delete existing images. Only the author of the comment can perform this action.
        /// </summary>
        [HttpPut("comments/{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromForm] UpdateCommentDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _service.UpdateCommentAsync(id, userId, dto);
            return Ok(new { message = "Comment updated successfully" });
        }

        /// <summary>
        /// Delete a comment by its ID. Only the author of the comment can perform this action. This will also delete all associated images and likes on the comment, but will not affect the parent post or other comments.
        /// </summary>
        [HttpDelete("comments/{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _service.DeleteCommentAsync(id, userId);
            return Ok(new { message = "Comment deleted successfully" });
        }

        /// <summary>
        /// Like a comment by its ID. The authenticated user can like any comment, but can only like a comment once. If the user has already liked the comment, this action will have no effect.
        /// </summary>
        [HttpPost("comments/{id}/like")]
        public async Task<IActionResult> LikeComment(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _service.LikeCommentAsync(id, userId);
            return Ok(new { message = "Comment liked" });
        }

        /// <summary>
        /// Unlike a comment by its ID. The authenticated user can unlike a comment that they have previously liked. If the user has not liked the comment before, this action will have no effect.
        /// </summary>
        [HttpDelete("comments/{id}/like")]
        public async Task<IActionResult> UnlikeComment(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _service.UnlikeCommentAsync(id, userId);
            return Ok(new { message = "Comment unliked" });
        }
    }
}