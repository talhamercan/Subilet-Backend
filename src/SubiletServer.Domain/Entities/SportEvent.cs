using SubiletServer.Domain.Abstractions;

namespace SubiletServer.Domain.Entities
{
    public class SportEvent : Entity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Capacity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public SportGenre Genre { get; set; }
        public EventStatus Status { get; set; } = EventStatus.Active;
    }

    public enum SportGenre
    {
        Football = 1,
        Basketball = 2,
        Tennis = 3,
        Volleyball = 4,
        Swimming = 5,
        Athletics = 6,
        Other = 7
    }
} 