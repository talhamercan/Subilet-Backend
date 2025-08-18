using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubiletServer.Domain.Users;

namespace SubiletServer.Infrastructure.Configurations
{
    internal sealed class SiteUserConfiguration : IEntityTypeConfiguration<SiteUser>
    {
        public void Configure(EntityTypeBuilder<SiteUser> builder)
        {
            builder.ToTable("SiteUsers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnType("varchar(255)");

            builder.Property(x => x.Username)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            // Password owned entity configuration
            builder.OwnsOne(x => x.Password, passwordBuilder =>
            {
                passwordBuilder.Property(p => p.PasswordHash)
                    .IsRequired()
                    .HasColumnType("varbinary(max)");

                passwordBuilder.Property(p => p.PasswordSalt)
                    .IsRequired()
                    .HasColumnType("varbinary(max)");
            });

            builder.Property(x => x.IsUserActive)
                .IsRequired();

            builder.Property(x => x.IsEmailVerified)
                .IsRequired();

            builder.Property(x => x.EmailVerifiedAt);

            // Email unique index
            builder.HasIndex(x => x.Email)
                .IsUnique();

            // Username unique index
            builder.HasIndex(x => x.Username)
                .IsUnique();
        }
    }
} 