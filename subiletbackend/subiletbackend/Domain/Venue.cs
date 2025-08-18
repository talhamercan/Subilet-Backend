namespace SubiletBackend.Domain
{
    public class Venue
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string LayoutJson { get; set; } = null!;
    }
} 