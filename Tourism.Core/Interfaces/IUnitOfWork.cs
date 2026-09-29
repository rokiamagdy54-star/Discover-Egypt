using Tourism.Core.Entities;
using Tourism.Core.Features.ReadyPlans.Interfaces;

namespace Tourism.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : BaseEntity;
        IReadyPlanRepository ReadyPlans { get; }
        Task<int> CompleteAsync();
    }
}