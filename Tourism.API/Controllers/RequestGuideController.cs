using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tourism.Core.Features.RequestGuide.DTOs;
using Tourism.Core.Features.RequestGuide.Interfaces;

namespace Tourism.API.Controllers
{
    [ApiController]
    [Route("api/requests")]
    [Authorize]
    public class RequestGuideController : ControllerBase
    {
        private readonly IRequestService _service;

        public RequestGuideController(IRequestService service)
        {
            _service = service;
        }

        /// <summary>Tourist submits a new guide request.</summary>
        [HttpPost]
        [Authorize(Roles = "Tourist")]
        public async Task<IActionResult> Create([FromBody] CreateRequestDto dto)
        {
            var touristId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _service.CreateRequestAsync(touristId, dto);
            return StatusCode(201, new { message = "Request sent successfully" });
        }

        /// <summary>Tourist views his own requests.</summary>
        [HttpGet("my-requests")]
        [Authorize(Roles = "Tourist")]
        public async Task<IActionResult> GetMyRequests()
        {
            var touristId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var requests = await _service.GetTouristRequestsAsync(touristId);
            return Ok(requests);
        }

        /// <summary>Tourist cancels a pending request.</summary>
        [HttpPost("{id}/cancel")]
        [Authorize(Roles = "Tourist")]
        public async Task<IActionResult> Cancel(int id)
        {
            var touristId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _service.CancelRequestAsync(id, touristId);
            return Ok(new { message = "Request cancelled successfully" });
        }

        /// <summary>Guide views incoming requests.</summary>
        [HttpGet("incoming")]
        [Authorize(Roles = "Guide")]
        public async Task<IActionResult> GetIncomingRequests()
        {
            var guideId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var requests = await _service.GetGuideRequestsAsync(guideId);
            return Ok(requests);
        }

        /// <summary>Guide or Tourist views request details.</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var request = await _service.GetRequestDetailsAsync(id, userId);
            return Ok(request);
        }

        /// <summary>Guide accepts a pending request.</summary>
        [HttpPost("{id}/accept")]
        [Authorize(Roles = "Guide")]
        public async Task<IActionResult> Accept(int id)
        {
            var guideId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _service.AcceptRequestAsync(id, guideId);
            return Ok(new { message = "Request accepted successfully" });
        }

        /// <summary>Guide rejects a pending request.</summary>
        [HttpPost("{id}/reject")]
        [Authorize(Roles = "Guide")]
        public async Task<IActionResult> Reject(int id)
        {
            var guideId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _service.RejectRequestAsync(id, guideId);
            return Ok(new { message = "Request rejected successfully" });
        }
    }
}