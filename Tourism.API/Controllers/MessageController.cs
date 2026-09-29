using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tourism.Core.Features.Message.DTOs;
using Tourism.Core.Features.Message.Interfaces;

namespace Tourism.API.Controllers
{
    [ApiController]
    [Route("api/messages")]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _service;

        public MessagesController(IMessageService service)
        {
            _service = service;
        }

        /// <summary>Sends a new message in a conversation.</summary>
        [HttpPost]
        public async Task<IActionResult> Send([FromBody] CreateMessageDto dto)
        {
            var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _service.SendMessageAsync(senderId, dto);
            return StatusCode(201, result);
        }

        /// <summary>Retrieves all messages in a specific conversation.</summary>
        [HttpGet("{conversationId}")]
        public async Task<IActionResult> GetMessages(int conversationId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var messages = await _service.GetMessagesAsync(userId, conversationId);
            return Ok(messages);
        }

        /// <summary>Marks all messages in a conversation as read.</summary>
        [HttpPut("{conversationId}/read")]
        public async Task<IActionResult> MarkAsRead(int conversationId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _service.MarkAsReadAsync(userId, conversationId);
            return Ok(new { message = "Messages marked as read" });
        }
    }
}