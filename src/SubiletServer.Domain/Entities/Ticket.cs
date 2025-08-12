using SubiletServer.Domain.Abstractions;
using SubiletServer.Domain.Users;

namespace SubiletServer.Domain.Entities
{
    public class Ticket : Entity
    {
        public IdentityId EventId { get; set; } = default!;
        public IdentityId UserId { get; set; } = default!;
        public decimal Price { get; set; }
        public DateTime PurchaseDate { get; set; }
        public TicketStatus Status { get; set; } = TicketStatus.Active;
        public string TicketNumber { get; set; } = string.Empty;
        
        // Navigation properties
        public virtual Event Event { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }

    public enum TicketStatus
    {
        Active = 1,
        Cancelled = 2,
        Used = 3,
        Refunded = 4
    }
} 