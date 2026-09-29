using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tourism.Core.Features.Favorite.Interfaces;

namespace Tourism.API.Controllers
{
    [Route("api/favorites")]
    [ApiController]
    [Authorize(Roles = "Tourist")]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;

        public FavoritesController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        /// <summary>Adds a place to the current user's favorites.</summary>
        [HttpPost("{placeId}")]
        public async Task<IActionResult> AddFavorite(int placeId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _favoriteService.AddFavoriteAsync(userId, placeId);
            return StatusCode(201, new { message = "Place added to favorites" });
        }

        /// <summary>Retrieves all favorites for the current user.</summary>
        [HttpGet]
        public async Task<IActionResult> GetFavorites()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var favorites = await _favoriteService.GetFavoritesAsync(userId);
            return Ok(favorites);
        }

        /// <summary>Removes a place from the current user's favorites.</summary>
        [HttpDelete("{placeId}")]
        public async Task<IActionResult> RemoveFavorite(int placeId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _favoriteService.RemoveFavoriteAsync(userId, placeId);
            return Ok(new { message = "Place removed from favorites" });
        }
    }
}