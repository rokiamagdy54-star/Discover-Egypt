using Tourism.Core.Features.Authentication.DTOs;
using Tourism.Core.Features.Users.DTOs;
using Tourism.Core.Features.Users.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Tourism.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>Retrieves the profile of the currently authenticated user.</summary>
        [HttpGet("my-profile")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var user = await _userService.GetCurrentUserAsync(userId);
            return Ok(user);
        }

        /// <summary>Returns the points balance of the authenticated tourist.</summary>
        [HttpGet("points")]
        public async Task<IActionResult> GetPoints()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var points = await _userService.GetUserPointsAsync(userId);
            return Ok(new { points });
        }

        /// <summary>Updates the profile of the currently authenticated user.</summary>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateMyProfile([FromForm] UpdateUserDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _userService.UpdateMyProfileAsync(userId, dto);
            return Ok(new { message = "Profile updated successfully" });
        }

        /// <summary>Changes the password of the currently authenticated user.</summary>
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _userService.ChangePasswordAsync(userId, dto);
            return Ok(new { message = "Password changed successfully" });
        }

        /// <summary>Permanently deletes the account of the currently authenticated user.</summary>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteMyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _userService.DeleteMyProfileAsync(userId);
            return Ok(new { message = "Account deleted successfully" });
        }

        // ─── Admin Endpoints ───

        /// <summary>Retrieves all registered users. Admin only.</summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>Retrieves a specific user by ID. Admin only.</summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }

        /// <summary>Deletes a specific user by ID. Admin only.</summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok(new { message = "User deleted successfully" });
        }
        /// <summary>Retrieves all guides. Admin only.</summary>
        [HttpGet("guides/all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllGuidesAdmin()
        {
            var guides = await _userService.GetAllGuidesAsync(activeOnly: false);
            return Ok(guides);
        }

        /// <summary>Retrieves all pending guides. Admin only.</summary>
        [HttpGet("guides/pending")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPendingGuides()
        {
            var guides = await _userService.GetPendingGuidesAsync();
            return Ok(guides);
        }

        /// <summary>Approves a guide application. Admin only.</summary>
        [HttpPut("guides/{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveGuide(string id)
        {
            await _userService.ApproveGuideAsync(id);
            return Ok(new { message = "Guide approved successfully" });
        }

        /// <summary>Rejects a guide application. Admin only.</summary>
        [HttpPut("guides/{id}/reject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectGuide(string id, [FromBody] RejectGuideDto dto)
        {
            await _userService.RejectGuideAsync(id, dto.Reason);
            return Ok(new { message = "Guide rejected successfully" });
        }

        /// <summary>Suspends a guide. Admin only.</summary>
        [HttpPut("guides/{id}/suspend")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SuspendGuide(string id)
        {
            await _userService.SuspendGuideAsync(id);
            return Ok(new { message = "Guide suspended successfully" });
        }

        // Normal User Endpoints
        /// <summary>Retrieves all active guides. Accessible by all authenticated users.</summary>
        [HttpGet("guides")]
        public async Task<IActionResult> GetAllGuides()
        {
            var guides = await _userService.GetAllGuidesAsync(activeOnly: true);
            return Ok(guides);
        }
        /// <summary>Retrieves guide profile with languages and rating.</summary>
        [HttpGet("guides/{id}")]
        public async Task<IActionResult> GetGuideProfile(string id)
        {
            var result = await _userService.GetGuideProfileAsync(id);
            return Ok(result);
        }

        /// <summary>Guide adds a language to his profile.</summary>
        [HttpPost("guides/languages")]
        [Authorize(Roles = "Guide")]
        public async Task<IActionResult> AddLanguage([FromBody] GuideLanguageDto dto)
        {
            var guideId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _userService.AddGuideLanguageAsync(guideId, dto);
            return StatusCode(201, new { message = "Language added successfully" });
        }

        /// <summary>Guide removes a language from his profile.</summary>
        [HttpDelete("guides/languages/{languageId}")]
        [Authorize(Roles = "Guide")]
        public async Task<IActionResult> RemoveLanguage(int languageId)
        {
            var guideId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _userService.RemoveGuideLanguageAsync(guideId, languageId);
            return Ok(new { message = "Language removed successfully" });
        }


        /// <summary>Guide sets his availability status.</summary>
        [HttpPut("guides/availability")]
        [Authorize(Roles = "Guide")]
        public async Task<IActionResult> SetAvailability([FromBody] bool isOnline)
        {
            var guideId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _userService.SetGuideAvailabilityAsync(guideId, isOnline);
            return Ok(new { message = isOnline ? "You are now online" : "You are now offline" });
        }

        
    }
}