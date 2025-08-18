using Microsoft.EntityFrameworkCore;
using SubiletServer.Domain.Entities;
using SubiletServer.Domain.Users;
using SubiletServer.Infrastructure.Context;

namespace SubiletServer.Infrastructure.Repositories
{
    public class StageEventRepository : IStageEventRepository
    {
        private readonly ApplicationDbContext _context;

        public StageEventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StageEvent>> GetAllAsync()
        {
            return await _context.StageEvents
                .Where(e => e.Status == EventStatus.Active)
                .OrderBy(e => e.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<StageEvent>> GetByGenreAsync(StageGenre genre)
        {
            return await _context.StageEvents
                .Where(e => e.Genre == genre && e.Status == EventStatus.Active)
                .OrderBy(e => e.Date)
                .ToListAsync();
        }

        public async Task<StageEvent?> GetByIdAsync(Guid id)
        {
            return await _context.StageEvents.FindAsync(id);
        }

        public async Task<StageEvent> AddAsync(StageEvent stageEvent)
        {
            _context.StageEvents.Add(stageEvent);
            await _context.SaveChangesAsync();
            return stageEvent;
        }

        public async Task<StageEvent> UpdateAsync(StageEvent stageEvent)
        {
            _context.StageEvents.Update(stageEvent);
            await _context.SaveChangesAsync();
            return stageEvent;
        }

        public async Task DeleteAsync(Guid id)
        {
            var stageEvent = await _context.StageEvents.FindAsync(id);
            if (stageEvent != null)
            {
                stageEvent.Status = EventStatus.Inactive;
                await _context.SaveChangesAsync();
            }
        }
    }
} 