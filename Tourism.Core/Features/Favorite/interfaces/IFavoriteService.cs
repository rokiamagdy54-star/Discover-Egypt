using Tourism.Core.Features.Favorite.DTOs;

namespace Tourism.Core.Features.Favorite.Interfaces
{
    public interface IFavoriteService
    {
        Task AddFavoriteAsync(string userId, int placeId);
        Task RemoveFavoriteAsync(string userId, int placeId);
        Task<List<FavoriteDto>> GetFavoritesAsync(string userId);
    }
}