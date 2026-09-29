using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tourism.Core.Features.Review.DTOs;
using Tourism.Core.Features.Review.Interfaces;

namespace Tourism.API.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    public class PlaceReviewsController : ControllerBase
    {
        private readonly IPlaceReviewService _service;

        public PlaceReviewsController(IPlaceReviewService service)
        {
            _service = service;
        }

        /// <summary>Submits a new review for a specific place.</summary>
        [HttpPost]
        [Authorize(Roles = "Tourist")]
        public async Task<IActionResult> AddReview([FromBody] CreatePlaceReviewDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _service.AddReviewAsync(userId, dto);
            return StatusCode(201, new { message = "Review added successfully" });
        }

        /// <summary>Retrieves all reviews for a specific place.</summary>
        [HttpGet("place/{placeId}")]
        public async Task<IActionResult> GetReviews(int placeId)
        {
            var reviews = await _service.GetReviewsByPlaceAsync(placeId);
            return Ok(reviews);
        }

        /// <summary>Updates an existing review.</summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Tourist")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] UpdatePlaceReviewDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _service.UpdateReviewAsync(id, userId, dto);
            return Ok(new { message = "Review updated successfully" });
        }

        /// <summary>Deletes a review.</summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Tourist")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _service.DeleteReviewAsync(id, userId);
            return Ok(new { message = "Review deleted successfully" });
        }
    }
}