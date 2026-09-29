using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tourism.Core.Features.Geoapify.Interfaces;

namespace Tourism.API.Controllers
{
    [ApiController]
    [Route("api/import")]
    [Authorize(Roles = "Admin")] 
    public class ImportController : ControllerBase
    {
        private readonly IGeoapifyService _geoService;

        public ImportController(IGeoapifyService geoService)
        {
            _geoService = geoService;
        }

        /// <summary>Imports places from Geoapify API. Admin only.</summary>
        [HttpPost("places")]
        public async Task<IActionResult> ImportPlaces()
        {
            await _geoService.ImportPlacesAsync();
            return Ok(new { message = "Places imported successfully" });
        }
    }
}