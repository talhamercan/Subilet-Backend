namespace SubiletBackend.Domain
{
    public class Order
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int CartId { get; set; }
        public string Status { get; set; } = "PendingPayment"; // veya "Paid"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
} 