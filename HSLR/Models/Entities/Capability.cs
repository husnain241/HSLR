using System.ComponentModel.DataAnnotations;

namespace HSLR.Models.Entities
{
    public class Capability
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Category { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string IconName { get; set; } = "bi-cpu";

        public int DisplayOrder { get; set; } = 0;

        public ICollection<CapabilityHighlight> Highlights { get; set; } = new List<CapabilityHighlight>();
    }

    public class CapabilityHighlight
    {
        public int Id { get; set; }
        public int CapabilityId { get; set; }
        public Capability? Capability { get; set; }

        [Required, MaxLength(300)]
        public string Highlight { get; set; } = string.Empty;

        public int DisplayOrder { get; set; } = 0;
    }
}
