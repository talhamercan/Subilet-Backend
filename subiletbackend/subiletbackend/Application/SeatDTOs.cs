namespace SubiletBackend.Application
{
    public class SeatCreateRequest
    {
        public int VenueId { get; set; }
        public string Row { get; set; } = null!;
        public int Number { get; set; }
        public string? Zone { get; set; }
    }

    public class SeatResponse
    {
        public int Id { get; set; }
        public int VenueId { get; set; }
        public string Row { get; set; } = null!;
        public int Number { get; set; }
        public string? Zone { get; set; }
    }
} 