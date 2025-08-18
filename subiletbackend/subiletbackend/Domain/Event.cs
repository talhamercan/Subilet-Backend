namespace SubiletBackend.Domain
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int ArtistId { get; set; }
        public string? Category { get; set; }
        public string? ImageUrl { get; set; }
    }
} 