using SubiletServer.Domain.Abstractions;

namespace SubiletServer.Domain.Entities
{
    public class MusicEvent : Entity
    {
        public string ArtistName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Capacity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public MusicGenre Genre { get; set; }
        public EventStatus Status { get; set; } = EventStatus.Active;
    }
} 