namespace SubiletBackend.Domain
{
    public class EventSession
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public DateTime DateTime { get; set; }
        public int TotalCapacity { get; set; }
    }
} 