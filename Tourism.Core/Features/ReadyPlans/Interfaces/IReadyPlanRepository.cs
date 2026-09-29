using Tourism.Core.Interfaces;
namespace Tourism.Core.Features.ReadyPlans.Interfaces
{
    public interface IReadyPlanRepository : IGenericRepository<Entities.ReadyPlan>
{
    Task<IReadOnlyList<Entities.ReadyPlan>> GetAllWithPlacesAsync();
    Task<Entities.ReadyPlan?> GetByIdWithPlacesAsync(int id);
}
}
