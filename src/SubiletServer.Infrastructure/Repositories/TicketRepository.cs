using Microsoft.EntityFrameworkCore;
using SubiletServer.Domain.Entities;
using SubiletServer.Domain.Users;
using SubiletServer.Domain.Abstractions;
using SubiletServer.Infrastructure.Context;

namespace SubiletServer.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _context;

        public TicketRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Ticket?> GetByIdAsync(IdentityId id)
        {
            return await _context.Tickets
                .Include(t => t.Event)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Ticket>> GetByUserIdAsync(IdentityId userId)
        {
            return await _context.Tickets
                .Include(t => t.Event)
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetUserTicketsAsync(IdentityId userId)
        {
            return await _context.Tickets
                .Include(t => t.Event)
                .Include(t => t.User)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.PurchaseDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetByEventIdAsync(IdentityId eventId)
        {
            return await _context.Tickets
                .Include(t => t.User)
                .Where(t => t.EventId == eventId)
                .ToListAsync();
        }

        public async Task<Ticket> AddAsync(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task<Ticket> UpdateAsync(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task<bool> DeleteAsync(IdentityId id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return false;

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetSoldTicketsCountAsync(IdentityId eventId)
        {
            return await _context.Tickets
                .Where(t => t.EventId == eventId && t.Status == TicketStatus.Active)
                .CountAsync();
        }

        public async Task<bool> IsEventSoldOutAsync(IdentityId eventId)
        {
            var @event = await _context.Events.FindAsync(eventId);
            if (@event == null) return true;

            var soldTickets = await GetSoldTicketsCountAsync(eventId);
            return soldTickets >= @event.Capacity;
        }
    }
} 