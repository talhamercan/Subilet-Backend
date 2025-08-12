using SubiletServer.Domain.Abstractions;

namespace SubiletServer.Domain.Entities
{
    public class Event : Entity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Capacity { get; set; }
        public int SoldTickets { get; set; } = 0;
        public string ImageUrl { get; set; } = string.Empty;
        public EventCategory Category { get; set; }
        public EventStatus Status { get; set; } = EventStatus.Active;
        
        // Navigation property
        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        
        // Computed property
        public int AvailableTickets => Capacity - SoldTickets;
        public bool IsSoldOut => SoldTickets >= Capacity;
    }

    public enum EventCategory
    {
        Music = 1,
        Stage = 2,
        Sports = 3
    }

    public enum MusicGenre
    {
        Pop = 1,
        Rock = 2,
        Jazz = 3,
        Classical = 4,
        Folk = 5,
        Electronic = 6,
        Other = 7
    }

    public enum EventStatus
    {
        Active = 1,
        Inactive = 2,
        Cancelled = 3,
        SoldOut = 4
    }
} 