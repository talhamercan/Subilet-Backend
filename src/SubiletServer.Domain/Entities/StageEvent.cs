using SubiletServer.Domain.Abstractions;

namespace SubiletServer.Domain.Entities
{
    public class StageEvent : Entity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Capacity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public StageGenre Genre { get; set; }
        public EventStatus Status { get; set; } = EventStatus.Active;
    }

    public enum StageGenre
    {
        Theatre = 1,
        Dance = 2,
        Opera = 3,
        Ballet = 4,
        Musical = 5,
        Comedy = 6,
        Other = 7
    }
} 