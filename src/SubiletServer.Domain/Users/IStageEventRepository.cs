using SubiletServer.Domain.Entities;

namespace SubiletServer.Domain.Users
{
    public interface IStageEventRepository
    {
        Task<IEnumerable<StageEvent>> GetAllAsync();
        Task<IEnumerable<StageEvent>> GetByGenreAsync(StageGenre genre);
        Task<StageEvent?> GetByIdAsync(Guid id);
        Task<StageEvent> AddAsync(StageEvent stageEvent);
        Task<StageEvent> UpdateAsync(StageEvent stageEvent);
        Task DeleteAsync(Guid id);
    }
} 