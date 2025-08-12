using Microsoft.EntityFrameworkCore;
using SubiletServer.Domain.Entities;
using SubiletServer.Domain.Users;
using SubiletServer.Infrastructure.Context;

namespace SubiletServer.Infrastructure.Repositories
{
    public class SportEventRepository : ISportEventRepository
    {
        private readonly ApplicationDbContext _context;

        public SportEventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SportEvent>> GetAllAsync()
        {
            return await _context.SportEvents
                .Where(e => e.Status == EventStatus.Active)
                .OrderBy(e => e.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<SportEvent>> GetByGenreAsync(SportGenre genre)
        {
            return await _context.SportEvents
                .Where(e => e.Genre == genre && e.Status == EventStatus.Active)
                .OrderBy(e => e.Date)
                .ToListAsync();
        }

        public async Task<SportEvent?> GetByIdAsync(Guid id)
        {
            return await _context.SportEvents.FindAsync(id);
        }

        public async Task<SportEvent> AddAsync(SportEvent sportEvent)
        {
            _context.SportEvents.Add(sportEvent);
            await _context.SaveChangesAsync();
            return sportEvent;
        }

        public async Task<SportEvent> UpdateAsync(SportEvent sportEvent)
        {
            _context.SportEvents.Update(sportEvent);
            await _context.SaveChangesAsync();
            return sportEvent;
        }

        public async Task DeleteAsync(Guid id)
        {
            var sportEvent = await _context.SportEvents.FindAsync(id);
            if (sportEvent != null)
            {
                sportEvent.Status = EventStatus.Inactive;
                await _context.SaveChangesAsync();
            }
        }
    }
} 