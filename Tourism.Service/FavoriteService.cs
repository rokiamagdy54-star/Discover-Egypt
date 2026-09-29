using AutoMapper;
using Tourism.Core.Entities;
using Tourism.Core.Exceptions;
using Tourism.Core.Features.Favorite.DTOs;
using Tourism.Core.Features.Favorite.Interfaces;
using Tourism.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Tourism.Service
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FavoriteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Add Favorite
        public async Task AddFavoriteAsync(string userId, int placeId)
        {
            var place = await _unitOfWork.Repository<Place>().GetByIdAsync(placeId);

            if (place == null)
                throw new NotFoundException("Place not found");

            var exists = await _unitOfWork.Repository<Favorite>().GetFirstAsync(
                predicate: f => f.UserId == userId && f.PlaceId == placeId);

            if (exists != null)
                throw new ConflictException("Place is already in your favorites");

            var favorite = new Favorite
            {
                UserId = userId,
                PlaceId = placeId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Favorite>().AddAsync(favorite);
            await _unitOfWork.CompleteAsync();
        }

        // Remove Favorite
        public async Task RemoveFavoriteAsync(string userId, int placeId)
        {
            var favorite = await _unitOfWork.Repository<Favorite>().GetFirstAsync(
                predicate: f => f.UserId == userId && f.PlaceId == placeId);

            if (favorite == null)
                throw new NotFoundException("Place is not in your favorites");

            _unitOfWork.Repository<Favorite>().Delete(favorite);
            await _unitOfWork.CompleteAsync();
        }

        // Get Favorites
        public async Task<List<FavoriteDto>> GetFavoritesAsync(string userId)
        {
            var favorites = await _unitOfWork.Repository<Favorite>().GetAllAsync(
                predicate: f => f.UserId == userId,
                include: q => q.Include(f => f.Place)
                               .ThenInclude(p => p.Category));

            return _mapper.Map<List<FavoriteDto>>(
                favorites.OrderByDescending(f => f.CreatedAt));
        }
    }
}