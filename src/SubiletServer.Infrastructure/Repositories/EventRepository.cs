using Microsoft.EntityFrameworkCore;
using SubiletServer.Domain.Entities;
using SubiletServer.Domain.Users;
using SubiletServer.Domain.Abstractions;
using SubiletServer.Infrastructure.Context;

namespace SubiletServer.Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Event?> GetByIdAsync(IdentityId id)
        {
            return await _context.Events
                .Include(e => e.Tickets)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _context.Events
                .Include(e => e.Tickets)
                .ToListAsync();
        }

        public async Task<Event> AddAsync(Event @event)
        {
            _context.Events.Add(@event);
            await _context.SaveChangesAsync();
            return @event;
        }

        public async Task<Event> UpdateAsync(Event @event)
        {
            _context.Events.Update(@event);
            await _context.SaveChangesAsync();
            return @event;
        }

        public async Task<bool> DeleteAsync(IdentityId id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null) return false;

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Event>> GetByCategoryAsync(EventCategory category)
        {
            return await _context.Events
                .Include(e => e.Tickets)
                .Where(e => e.Category == category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetActiveEventsAsync()
        {
            return await _context.Events
                .Include(e => e.Tickets)
                .Where(e => e.Status == EventStatus.Active)
                .ToListAsync();
        }
    }
} 