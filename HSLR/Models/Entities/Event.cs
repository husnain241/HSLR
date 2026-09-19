using System.ComponentModel.DataAnnotations;

namespace HSLR.Models.Entities
{
    public class Event
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Slug { get; set; } = string.Empty;

        [Required, MaxLength(250)]
        public string Title { get; set; } = string.Empty;

        public EventType EventType { get; set; } = EventType.Seminar;

        public DateTime EventDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required, MaxLength(200)]
        public string Location { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? Speaker { get; set; }

        [MaxLength(200)]
        public string? Affiliation { get; set; }

        public EventStatus Status { get; set; } = EventStatus.Upcoming;

        public bool Featured { get; set; } = false;

        [MaxLength(500)]
        public string? LinkUrl { get; set; }
    }
}
