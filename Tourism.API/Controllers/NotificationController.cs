using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tourism.Core.Features.Notification.DTOs;
using Tourism.Core.Features.Notification.Interfaces;

namespace Tourism.API.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationsController(INotificationService service)
        {
            _service = service;
        }

        /// <summary>Retrieves all notifications for the current user.</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _service.GetAllAsync(userId);
            return Ok(result);
        }

        /// <summary>Retrieves a specific notification by ID.</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _service.GetByIdAsync(id, userId);
            return Ok(result);
        }

        /// <summary>Creates a new notification for the current user.</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNotificationDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _service.CreateAsync(userId, dto);
            return StatusCode(201, result);
        }

        /// <summary>Marks a specific notification as read.</summary>
        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _service.MarkAsReadAsync(id, userId);
            return Ok(new { message = "Notification marked as read" });
        }

        /// <summary>Marks all notifications as read.</summary>
        [HttpPut("read-all")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _service.MarkAllAsReadAsync(userId);
            return Ok(new { message = "All notifications marked as read" });
        }

        /// <summary>Deletes a specific notification.</summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _service.DeleteAsync(id, userId);
            return Ok(new { message = "Notification deleted successfully" });
        }
    }
}