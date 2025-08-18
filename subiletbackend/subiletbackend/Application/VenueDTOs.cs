namespace SubiletBackend.Application
{
    public class VenueCreateRequest
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string LayoutJson { get; set; } = null!;
    }

    public class VenueResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string LayoutJson { get; set; } = null!;
    }
} 