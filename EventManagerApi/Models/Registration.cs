namespace EventManagerApi.Models
{
    public class Registration
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public DateTime RegisteredAt { get; set; }
    }
}
