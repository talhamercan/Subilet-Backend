using SubiletServer.Domain.Entities;

namespace SubiletServer.Domain.Users
{
    public interface IMusicEventRepository
    {
        Task<IEnumerable<MusicEvent>> GetAllAsync();
        Task<IEnumerable<MusicEvent>> GetByGenreAsync(MusicGenre genre);
        Task<MusicEvent?> GetByIdAsync(Guid id);
        Task<MusicEvent> AddAsync(MusicEvent musicEvent);
        Task<MusicEvent> UpdateAsync(MusicEvent musicEvent);
        Task DeleteAsync(Guid id);
    }
} 