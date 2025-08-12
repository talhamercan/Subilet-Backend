using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubiletServer.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubiletServer.Infrastructure.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(i => i.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(i => i.Email)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnType("varchar(255)");

            builder.Property(i => i.Username)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.Property(i => i.Role)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("varchar(50)");

            builder.Property(i => i.IsActive)
                .IsRequired();

            // Password owned entity configuration
            builder.OwnsOne(i => i.Password, passwordBuilder =>
            {
                passwordBuilder.Property(p => p.PasswordHash)
                    .IsRequired()
                    .HasColumnType("varbinary(max)");

                passwordBuilder.Property(p => p.PasswordSalt)
                    .IsRequired()
                    .HasColumnType("varbinary(max)");
            });

            // Email unique index
            builder.HasIndex(x => x.Email)
                .IsUnique();

            // Username unique index
            builder.HasIndex(x => x.Username)
                .IsUnique();
        }
    }
}
