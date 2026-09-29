using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tourism.Core.Features.Nationalities.DTOs;
using Tourism.Core.Features.Nationalities.Interfaces;

namespace Tourism.API.Controllers
{
    [Route("api/nationalities")]
    [ApiController]
    public class NationalitiesController : ControllerBase
    {
        private readonly INationalityService _service;

        public NationalitiesController(INationalityService service)
        {
            _service = service;
        }

        /// <summary>Retrieves all nationalities.</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        /// <summary>Retrieves a specific nationality by ID.</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        /// <summary>Creates a new nationality. Admin only.</summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateNationalityDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return StatusCode(201, result);
        }

        /// <summary>Updates an existing nationality. Admin only.</summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateNationalityDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok(new { message = "Nationality updated successfully" });
        }

        /// <summary>Deletes a nationality. Admin only.</summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok(new { message = "Nationality deleted successfully" });
        }
    }
}