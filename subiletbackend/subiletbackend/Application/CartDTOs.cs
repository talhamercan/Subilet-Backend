namespace SubiletBackend.Application
{
    public class CartItemAddRequest
    {
        public int SessionId { get; set; }
        public List<int> SeatIds { get; set; } = new();
        public int Qty { get; set; }
    }

    public class CartItemResponse
    {
        public int CartItemId { get; set; }
        public int SessionId { get; set; }
        public int SeatId { get; set; }
        public int Qty { get; set; }
    }

    public class CartResponse
    {
        public int CartId { get; set; }
        public List<CartItemResponse> Items { get; set; } = new();
    }

    public class CheckoutRequest
    {
        public int CartId { get; set; }
    }

    public class CheckoutResponse
    {
        public int OrderId { get; set; }
        public string Status { get; set; } = null!;
    }
} 