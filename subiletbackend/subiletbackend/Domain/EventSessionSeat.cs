namespace SubiletBackend.Domain
{
    public class EventSessionSeat
    {
        public int SessionId { get; set; }
        public int SeatId { get; set; }
        public bool IsReserved { get; set; }
        public bool IsSold { get; set; }
        public byte[]? RowVersion { get; set; }
    }
} 