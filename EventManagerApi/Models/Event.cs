namespace EventManagerApi.Models
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime Date { get; set; }
        public string Location { get; set; } = default!;
        public string Category { get; set; } = default!;
        public int Capacity { get; set; }
        public EventStatus Status { get; set; }
        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }

    public enum EventStatus { Draft, Published, Canceled }
}
