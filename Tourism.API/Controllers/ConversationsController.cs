using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tourism.Core.Features.Conversation.DTOs;
using Tourism.Core.Features.Conversation.Interfaces;

namespace Tourism.API.Controllers
{
    [ApiController]
    [Route("api/conversations")]
    [Authorize]
    public class ConversationsController : ControllerBase
    {
        private readonly IConversationService _service;

        public ConversationsController(IConversationService service)
        {
            _service = service;
        }

        /// <summary>Creates a new conversation between a tourist and a guide.</summary>
        [HttpPost]
        [Authorize(Roles = "Tourist")]
        public async Task<IActionResult> Create([FromBody] CreateConversationDto dto)
        {
            var touristId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _service.CreateConversationAsync(touristId, dto);
            return StatusCode(201, result);
        }

        /// <summary>Retrieves all conversations for the current user.</summary>
        [HttpGet]
        public async Task<IActionResult> GetMyConversations()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _service.GetMyConversationsAsync(userId);
            return Ok(result);
        }

        /// <summary>Retrieves a specific conversation by ID.</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _service.GetConversationByIdAsync(id, userId);
            return Ok(result);
        }
    }
}