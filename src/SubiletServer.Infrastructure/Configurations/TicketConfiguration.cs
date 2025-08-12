using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubiletServer.Domain.Entities;

namespace SubiletServer.Infrastructure.Configurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(t => t.Id);
            
            builder.Property(t => t.EventId)
                .IsRequired();
                
            builder.Property(t => t.UserId)
                .IsRequired();
                
            builder.Property(t => t.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
                
            builder.Property(t => t.PurchaseDate)
                .IsRequired();
                
            builder.Property(t => t.Status)
                .IsRequired();
                
            builder.Property(t => t.TicketNumber)
                .HasMaxLength(50)
                .IsRequired();
                
            // Foreign key relationships
            builder.HasOne(t => t.Event)
                .WithMany(e => e.Tickets)
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);
                
            // Indexes
            builder.HasIndex(t => new { t.EventId, t.UserId })
                .HasDatabaseName("IX_Tickets_EventId_UserId");
        }
    }
} 