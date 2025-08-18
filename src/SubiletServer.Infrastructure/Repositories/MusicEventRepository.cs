using Microsoft.EntityFrameworkCore;
using SubiletServer.Domain.Entities;
using SubiletServer.Domain.Users;
using SubiletServer.Infrastructure.Context;

namespace SubiletServer.Infrastructure.Repositories
{
    public class MusicEventRepository : IMusicEventRepository
    {
        private readonly ApplicationDbContext _context;

        public MusicEventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MusicEvent>> GetAllAsync()
        {
            return await _context.MusicEvents
                .Where(e => e.Status == EventStatus.Active)
                .OrderBy(e => e.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<MusicEvent>> GetByGenreAsync(MusicGenre genre)
        {
            return await _context.MusicEvents
                .Where(e => e.Genre == genre && e.Status == EventStatus.Active)
                .OrderBy(e => e.Date)
                .ToListAsync();
        }

        public async Task<MusicEvent?> GetByIdAsync(Guid id)
        {
            return await _context.MusicEvents.FindAsync(id);
        }

        public async Task<MusicEvent> AddAsync(MusicEvent musicEvent)
        {
            _context.MusicEvents.Add(musicEvent);
            await _context.SaveChangesAsync();
            return musicEvent;
        }

        public async Task<MusicEvent> UpdateAsync(MusicEvent musicEvent)
        {
            _context.MusicEvents.Update(musicEvent);
            await _context.SaveChangesAsync();
            return musicEvent;
        }

        public async Task DeleteAsync(Guid id)
        {
            var musicEvent = await _context.MusicEvents.FindAsync(id);
            if (musicEvent != null)
            {
                musicEvent.Status = EventStatus.Inactive;
                await _context.SaveChangesAsync();
            }
        }
    }
} 