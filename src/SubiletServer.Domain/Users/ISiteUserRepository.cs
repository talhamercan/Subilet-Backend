using SubiletServer.Domain.Abstractions;
using System.Linq.Expressions;

namespace SubiletServer.Domain.Users
{
    public interface ISiteUserRepository
    {
        Task<SiteUser?> GetByIdAsync(Guid id);
        Task<IEnumerable<SiteUser>> GetAllAsync();
        Task<SiteUser?> FirstOrDefaultAsync(Expression<Func<SiteUser, bool>> predicate);
        void Add(SiteUser entity);
        void Delete(SiteUser entity);
        Task<SiteUser?> GetByEmailAsync(string email);
        Task<SiteUser?> GetByUsernameAsync(string username);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> UsernameExistsAsync(string username);
    }
} 