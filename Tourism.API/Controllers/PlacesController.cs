using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tourism.Core.Features.Places.DTOs;
using Tourism.Core.Features.Places.Interfaces;

namespace Tourism.API.Controllers
{
    [ApiController]
    [Route("api/places")]
    public class PlacesController : ControllerBase
    {
        private readonly IPlaceService _placeService;

        public PlacesController(IPlaceService placeService)
        {
            _placeService = placeService;
        }

        /// <summary>Retrieves all places, optionally filtered by city or category.</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? city,
            [FromQuery] int? categoryId)
        {
            var result = await _placeService.GetAllAsync(city, categoryId);
            return Ok(result);
        }

        /// <summary>Retrieves a specific place by ID.</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _placeService.GetByIdAsync(id);
            return Ok(result);
        }

        /// <summary>Creates a new place. Admin only.</summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] CreatePlaceDto dto) // ← FromForm
        {
            var result = await _placeService.CreateAsync(dto);
            return StatusCode(201, result);
        }

        /// <summary>Updates an existing place. Admin only.</summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdatePlaceDto dto) // ← FromForm
        {
            await _placeService.UpdateAsync(id, dto);
            return Ok(new { message = "Place updated successfully" });
        }

        /// <summary>Deletes a place. Admin only.</summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _placeService.DeleteAsync(id);
            return Ok(new { message = "Place deleted successfully" });
        }

        /// <summary>Adds photos to an existing place. Admin only.</summary>
        [HttpPost("{id}/photos")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddPhotos(int id, [FromForm] List<IFormFile> photos)
        {
            await _placeService.AddPhotosAsync(id, photos);
            return Ok(new { message = "Photos added successfully" });
        }

        /// <summary>Deletes a specific photo from a place. Admin only.</summary>
        [HttpDelete("{id}/photos/{photoId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePhoto(int id, int photoId)
        {
            await _placeService.DeletePhotoAsync(id, photoId);
            return Ok(new { message = "Photo deleted successfully" });
        }
    }
}