using SubiletServer.Domain.Entities;
using SubiletServer.Domain.Abstractions;

namespace SubiletServer.Domain.Users
{
    public interface IEventRepository
    {
        Task<Event?> GetByIdAsync(IdentityId id);
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event> AddAsync(Event @event);
        Task<Event> UpdateAsync(Event @event);
        Task<bool> DeleteAsync(IdentityId id);
        Task<IEnumerable<Event>> GetByCategoryAsync(EventCategory category);
        Task<IEnumerable<Event>> GetActiveEventsAsync();
    }
} 