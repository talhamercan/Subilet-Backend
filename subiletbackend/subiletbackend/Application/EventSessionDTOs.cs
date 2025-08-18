namespace SubiletBackend.Application
{
    public class EventSessionCreateRequest
    {
        public int EventId { get; set; }
        public DateTime DateTime { get; set; }
        public int TotalCapacity { get; set; }
    }

    public class EventSessionResponse
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public DateTime DateTime { get; set; }
        public int TotalCapacity { get; set; }
    }
} 