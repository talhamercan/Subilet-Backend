using Microsoft.EntityFrameworkCore;
using SubiletBackend.Domain;

namespace SubiletBackend.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<Artist> Artists => Set<Artist>();
        public DbSet<Venue> Venues => Set<Venue>();
        public DbSet<Seat> Seats => Set<Seat>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<EventSession> EventSessions => Set<EventSession>();
        public DbSet<EventSessionSeat> EventSessionSeats => Set<EventSessionSeat>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<Order> Orders => Set<Order>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasMany(u => u.RefreshTokens)
                      .WithOne(rt => rt.User)
                      .HasForeignKey(rt => rt.UserId);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(rt => rt.Id);
            });
        }
    }
} 