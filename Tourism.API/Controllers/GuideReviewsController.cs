using Tourism.Core.Features.GuideReviews.DTOs;
using Tourism.Core.Features.GuideReviews.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Tourism.API.Controllers
{
    [ApiController]
    [Route("api/guide-reviews")]
    public class GuideReviewsController : ControllerBase
    {
        private readonly IGuideReviewService _service;

        public GuideReviewsController(IGuideReviewService service)
        {
            _service = service;
        }
        /// <summary>Adds a new guide review.</summary>
        [HttpPost]
        [Authorize(Roles = "Tourist")]
        public async Task<IActionResult> Add([FromBody] CreateGuideReviewDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _service.AddAsync(userId, dto);
            return StatusCode(201, new { message = "Review added successfully" });
        }
        /// <summary>Gets all reviews for a specific guide.</summary>
        [HttpGet("guide/{guideId}")]
        public async Task<IActionResult> GetByGuide(string guideId)
        {
            var reviews = await _service.GetByGuideAsync(guideId);
            return Ok(reviews);
        }
        
        /// <summary>Deletes a guide review.</summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Tourist")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _service.DeleteAsync(id, userId);
            return Ok(new { message = "Review deleted successfully" });
        }
    }
}