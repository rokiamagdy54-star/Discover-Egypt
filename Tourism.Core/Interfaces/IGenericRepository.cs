using System.Linq.Expressions;
using Tourism.Core.Entities;

namespace Tourism.Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);

        Task<IReadOnlyList<T>> GetAllAsync(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null);

        Task<T?> GetFirstAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IQueryable<T>>? include = null);

        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}