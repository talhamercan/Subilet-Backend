using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubiletServer.Domain.Entities;

namespace SubiletServer.Infrastructure.Configurations
{
    public class MusicEventConfiguration : IEntityTypeConfiguration<MusicEvent>
    {
        public void Configure(EntityTypeBuilder<MusicEvent> builder)
        {
            builder.ToTable("MusicEvents");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.ArtistName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(e => e.Location)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(e => e.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(e => e.Capacity)
                .IsRequired();

            builder.Property(e => e.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(e => e.Genre)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion<int>();

            // Indexes
            builder.HasIndex(e => e.Genre);
            builder.HasIndex(e => e.Status);
            builder.HasIndex(e => e.Date);
        }
    }
} 