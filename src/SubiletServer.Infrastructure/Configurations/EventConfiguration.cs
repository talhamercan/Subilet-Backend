using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubiletServer.Domain.Entities;

namespace SubiletServer.Infrastructure.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Name)
                .HasMaxLength(200)
                .IsRequired();
                
            builder.Property(e => e.Description)
                .HasMaxLength(2000);
                
            builder.Property(e => e.Date)
                .IsRequired();
                
            builder.Property(e => e.Location)
                .HasMaxLength(500)
                .IsRequired();
                
            builder.Property(e => e.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
                
            builder.Property(e => e.Capacity)
                .IsRequired();
                
            builder.Property(e => e.SoldTickets)
                .IsRequired()
                .HasDefaultValue(0);
                
            builder.Property(e => e.ImageUrl)
                .HasMaxLength(500);
                
            builder.Property(e => e.Category)
                .IsRequired();
                
            builder.Property(e => e.Status)
                .IsRequired()
                .HasDefaultValue(EventStatus.Active);
                
            // Indexes
            builder.HasIndex(e => e.Category);
            builder.HasIndex(e => e.Status);
            builder.HasIndex(e => e.Date);
        }
    }
} 