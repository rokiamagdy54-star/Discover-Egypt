using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tourism.Core.Features.CustomPlans.DTOs;
using Tourism.Core.Features.CustomPlans.Interfaces;
using Tourism.Core.Features.ReadyPlans.DTOs;
using Tourism.Core.Features.ReadyPlans.Interfaces;

namespace Tourism.API.Controllers
{
    [ApiController]
    [Route("api/trips")]
    public class TripsController : ControllerBase
    {
        private readonly IPlanService _planService;
        private readonly ICustomPlanService _customService;

        public TripsController(IPlanService planService, ICustomPlanService customService)
        {
            _planService = planService;
            _customService = customService;
        }

        // ─── Ready Plans ───

        /// <summary>Retrieves all ready plans.</summary>
        [HttpGet]
        public async Task<IActionResult> GetReadyTrips()
        {
            var result = await _planService.GetAllReadyPlansAsync();
            return Ok(result);
        }

        /// <summary>Retrieves a specific ready plan by ID.</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReadyTripById(int id)
        {
            var result = await _planService.GetReadyPlanByIdAsync(id);
            return Ok(result);
        }

        /// <summary>Creates a new ready plan. Admin only.</summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateReadyTrip([FromForm] CreateReadyPlanDto dto)
        {
            var result = await _planService.CreateReadyPlanAsync(dto);
            return StatusCode(201, result);
        }

        /// <summary>Deletes a ready plan. Admin only.</summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteReadyTrip(int id)
        {
            await _planService.DeleteReadyPlanAsync(id);
            return Ok(new { message = "Trip deleted successfully" });
        }

        // ─── Custom Plans ───

        /// <summary>Creates a custom plan for the current tourist.</summary>
        [HttpPost("custom")]
        [Authorize(Roles = "Tourist")]
        public async Task<IActionResult> CreateCustomTrip([FromForm] CreateCustomPlanDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _customService.CreateCustomPlanAsync(userId, dto);
            return StatusCode(201, result);
        }

        /// <summary>Retrieves all custom plans for the current tourist.</summary>
        [HttpGet("custom/my")]
        [Authorize(Roles = "Tourist")]
        public async Task<IActionResult> GetMyCustomTrips()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _customService.GetMyPlansAsync(userId);
            return Ok(result);
        }

        /// <summary>Retrieves a specific custom plan by ID.</summary>
        [HttpGet("custom/{id}")]
        [Authorize(Roles = "Tourist")]
        public async Task<IActionResult> GetCustomTripById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _customService.GetByIdAsync(id, userId);
            return Ok(result);
        }

        /// <summary>Updates a custom plan.</summary>
        [HttpPut("custom/{id}")]
        [Authorize(Roles = "Tourist")]
        public async Task<IActionResult> UpdateCustomTrip(int id, [FromForm] UpdateCustomPlanDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _customService.UpdateCustomPlanAsync(id, userId, dto);
            return Ok(new { message = "Plan updated successfully" });
        }

        /// <summary>Deletes a custom plan.</summary>
        [HttpDelete("custom/{id}")]
        [Authorize(Roles = "Tourist")]
        public async Task<IActionResult> DeleteCustomTrip(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _customService.DeleteCustomPlanAsync(id, userId);
            return Ok(new { message = "Plan deleted successfully" });
        }

        /// <summary>Retrieves all custom plans. Admin only.</summary>
        [HttpGet("custom/all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllCustomTrips()
        {
            var result = await _customService.GetAllAsync();
            return Ok(result);
        }
    }
}