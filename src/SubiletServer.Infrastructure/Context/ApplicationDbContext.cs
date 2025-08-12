using GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SubiletServer.Domain.Abstractions;
using SubiletServer.Domain.Entities;
using SubiletServer.Domain.Users;
using System.Security.Claims;
using System.Security.Cryptography;

namespace SubiletServer.Infrastructure.Context;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }


    public DbSet<User> Users { get; set; }
    public DbSet<SiteUser> SiteUsers { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<SiteUserLoginHistory> SiteUserLoginHistories { get; set; }
    public DbSet<MusicEvent> MusicEvents { get; set; }
    public DbSet<StageEvent> StageEvents { get; set; }
    public DbSet<SportEvent> SportEvents { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Ticket> Tickets { get; set; }




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<IdentityId>().HaveConversion<IdentityIdValueConverter>();
        configurationBuilder.Properties<decimal>().HaveColumnType("decimal(18,2)");
        configurationBuilder.Properties<string>().HaveColumnType("varchar(MAX)");
        base.ConfigureConventions(configurationBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<Entity>();

        // Admin kullanıcısı oluşturma sırasında CreatedBy alanını otomatik doldur
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(p => p.CreatedAt)
                    .CurrentValue = DateTimeOffset.Now;
                
                // Eğer CreatedBy null ise, entity'nin kendi ID'sini kullan
                if (entry.Property(p => p.CreatedBy).CurrentValue == null)
                {
                    entry.Property(p => p.CreatedBy)
                        .CurrentValue = new IdentityId(entry.Entity.Id);
                }
            }

            if (entry.State == EntityState.Modified)
            {
                if (entry.Property(p => p.IsDeleted).CurrentValue == true)
                {
                    entry.Property(p => p.DeletedAt)
                    .CurrentValue = DateTimeOffset.Now;
                }
                else
                {
                    entry.Property(p => p.UpdatedAt)
                        .CurrentValue = DateTimeOffset.Now;
                }
            }

            if (entry.State == EntityState.Deleted)
            {
                throw new ArgumentException("Db'den direkt silme işlemi yapamazsınız");
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}

internal sealed class IdentityIdValueConverter : ValueConverter<IdentityId, Guid>
{
    public IdentityIdValueConverter() : base(m => m.Value, m => new IdentityId(m)) { }
}