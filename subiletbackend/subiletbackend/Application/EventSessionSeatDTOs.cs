namespace SubiletBackend.Application
{
    public class EventSessionSeatResponse
    {
        public int SessionId { get; set; }
        public int SeatId { get; set; }
        public string Row { get; set; } = null!;
        public int Number { get; set; }
        public string? Zone { get; set; }
        public bool IsReserved { get; set; }
        public bool IsSold { get; set; }
    }
} 