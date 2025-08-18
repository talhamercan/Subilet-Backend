namespace SubiletBackend.Domain
{
    public class Cart
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
} 