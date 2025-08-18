namespace SubiletBackend.Application
{
    public class ArtistCreateRequest
    {
        public string Name { get; set; } = null!;
        public string? Bio { get; set; }
    }

    public class ArtistUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Bio { get; set; }
    }

    public class ArtistResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Bio { get; set; }
    }
} 