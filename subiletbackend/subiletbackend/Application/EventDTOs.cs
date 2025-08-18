namespace SubiletBackend.Application
{
    public class EventCreateRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int ArtistId { get; set; }
        public string? Category { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class EventUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int ArtistId { get; set; }
        public string? Category { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class EventResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int ArtistId { get; set; }
        public string? Category { get; set; }
        public string? ImageUrl { get; set; }
    }
} 