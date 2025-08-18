using Microsoft.EntityFrameworkCore;
using SubiletServer.Domain.Users;
using SubiletServer.Infrastructure.Context;
using System.Linq.Expressions;

namespace SubiletServer.Infrastructure.Repositories
{
    internal sealed class SiteUserRepository : ISiteUserRepository
    {
        private readonly ApplicationDbContext _context;

        public SiteUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SiteUser?> GetByIdAsync(Guid id)
        {
            var identityId = new SubiletServer.Domain.Abstractions.IdentityId(id);
            return await _context.SiteUsers.FindAsync(identityId);
        }

        public async Task<IEnumerable<SiteUser>> GetAllAsync()
        {
            return await _context.SiteUsers.ToListAsync();
        }

        public async Task<SiteUser?> FirstOrDefaultAsync(Expression<Func<SiteUser, bool>> predicate)
        {
            return await _context.SiteUsers.FirstOrDefaultAsync(predicate);
        }

        public void Add(SiteUser entity)
        {
            _context.SiteUsers.Add(entity);
        }

        public void Delete(SiteUser entity)
        {
            _context.SiteUsers.Remove(entity);
        }

        public async Task<SiteUser?> GetByEmailAsync(string email)
        {
            Console.WriteLine($"GetByEmailAsync called with: {email}");
            var result = await _context.SiteUsers.FirstOrDefaultAsync(u => u.Email == email);
            Console.WriteLine($"GetByEmailAsync result: {(result != null ? "Found" : "Not found")}");
            return result;
        }

        public async Task<SiteUser?> GetByUsernameAsync(string username)
        {
            Console.WriteLine($"GetByUsernameAsync called with: {username}");
            var result = await _context.SiteUsers.FirstOrDefaultAsync(u => u.Username == username);
            Console.WriteLine($"GetByUsernameAsync result: {(result != null ? "Found" : "Not found")}");
            return result;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            Console.WriteLine($"EmailExistsAsync called with: {email}");
            var result = await _context.SiteUsers.AnyAsync(u => u.Email == email);
            Console.WriteLine($"EmailExistsAsync result: {result}");
            return result;
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            Console.WriteLine($"UsernameExistsAsync called with: {username}");
            var result = await _context.SiteUsers.AnyAsync(u => u.Username == username);
            Console.WriteLine($"UsernameExistsAsync result: {result}");
            return result;
        }
    }
} 