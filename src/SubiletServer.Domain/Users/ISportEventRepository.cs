using SubiletServer.Domain.Entities;

namespace SubiletServer.Domain.Users
{
    public interface ISportEventRepository
    {
        Task<IEnumerable<SportEvent>> GetAllAsync();
        Task<IEnumerable<SportEvent>> GetByGenreAsync(SportGenre genre);
        Task<SportEvent?> GetByIdAsync(Guid id);
        Task<SportEvent> AddAsync(SportEvent sportEvent);
        Task<SportEvent> UpdateAsync(SportEvent sportEvent);
        Task DeleteAsync(Guid id);
    }
} 