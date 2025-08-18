namespace SubiletBackend.Domain
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int SessionId { get; set; }
        public int SeatId { get; set; }
        public int Qty { get; set; }
    }
} 