using System.ComponentModel.DataAnnotations;

namespace EventManagerApi.Models
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        [StringLength(250, ErrorMessage = "The description must be between 0 and 250 characters.")]
        public string Description { get; set; } = default!;
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "The event date must be in the future.")]
        public DateTime Date { get; set; }
        public string Location { get; set; } = default!;
        public string Category { get; set; } = default!;
        [Range(1, 1000)]
        public int Capacity { get; set; }
        public EventStatus Status { get; set; }
        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }

    public enum EventStatus { Draft, Published, Canceled }

    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime dateValue)
            {
                if (dateValue > DateTime.Now)
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult(ErrorMessage ?? "Date must be in the future.");
            }
            return new ValidationResult("Invalid date value.");
        }
    }
}
