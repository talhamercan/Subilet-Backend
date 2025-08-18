namespace SubiletServer.Domain.Users
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> FirstOrDefaultAsync(Func<User, bool> predicate);
        Task<User?> GetByUsernameAsync(string username);
        void Add(User user);
        void Delete(User user);
    }
}
