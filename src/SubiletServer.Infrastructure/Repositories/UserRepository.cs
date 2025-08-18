using SubiletServer.Domain.Users;
using SubiletServer.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using GenericRepository;

namespace SubiletServer.Infrastructure.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            var identityId = new SubiletServer.Domain.Abstractions.IdentityId(id);
            return await _context.Set<User>().FirstOrDefaultAsync(u => u.Id == identityId);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Set<User>().ToListAsync();
        }

        public async Task<User?> FirstOrDefaultAsync(Func<User, bool> predicate)
        {
            var users = await _context.Set<User>().ToListAsync();
            return users.FirstOrDefault(predicate);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(u => u.Username == username);
        }

        public void Add(User user)
        {
            _context.Set<User>().Add(user);
        }

        public void Delete(User user)
        {
            _context.Set<User>().Remove(user);
        }
    }
}
