using Tourism.Core.Entities;
using Tourism.Core.Features.ReadyPlans.Interfaces;
using Tourism.Core.Interfaces;
using Tourism.Repository.Data.DBContext;

namespace Tourism.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();

        public IReadyPlanRepository ReadyPlans { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            ReadyPlans = new ReadyPlanRepository(_context);
        }

        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            var type = typeof(T);

            if (!_repositories.TryGetValue(type, out var repo))
            {
                repo = new GenericRepository<T>(_context);
                _repositories[type] = repo;
            }

            return (IGenericRepository<T>)repo;
        }

        public async Task<int> CompleteAsync()
            => await _context.SaveChangesAsync();

        public void Dispose()
            => _context.Dispose();
    }
}