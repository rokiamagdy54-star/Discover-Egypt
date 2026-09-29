using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourism.Core.Entities;
using Tourism.Core.Features.ReadyPlans.Interfaces;
using Tourism.Repository.Data.DBContext;

namespace Tourism.Repository
{
    public class ReadyPlanRepository : GenericRepository<ReadyPlan>, IReadyPlanRepository
    {
        private readonly ApplicationDbContext _context;

        public ReadyPlanRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ReadyPlan>> GetAllWithPlacesAsync()
        {
            return await _context.ReadyPlans
                .Include(r => r.PlanPlaces)
                .Include(r => r.Guide)
                .ThenInclude(g => g.User).ToListAsync();
        }

        public async Task<ReadyPlan?> GetByIdWithPlacesAsync(int id)
        {
            return await _context.ReadyPlans
                .Include(p => p.PlanPlaces)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
