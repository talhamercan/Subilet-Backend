using SubiletServer.Domain.Entities;
using SubiletServer.Domain.Abstractions;

namespace SubiletServer.Domain.Users
{
    public interface ITicketRepository
    {
        Task<Ticket?> GetByIdAsync(IdentityId id);
        Task<IEnumerable<Ticket>> GetByUserIdAsync(IdentityId userId);
        Task<IEnumerable<Ticket>> GetUserTicketsAsync(IdentityId userId);
        Task<IEnumerable<Ticket>> GetByEventIdAsync(IdentityId eventId);
        Task<Ticket> AddAsync(Ticket ticket);
        Task<Ticket> UpdateAsync(Ticket ticket);
        Task<bool> DeleteAsync(IdentityId id);
        Task<int> GetSoldTicketsCountAsync(IdentityId eventId);
        Task<bool> IsEventSoldOutAsync(IdentityId eventId);
    }
} 