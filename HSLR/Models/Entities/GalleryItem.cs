using System.ComponentModel.DataAnnotations;

namespace HSLR.Models.Entities
{
    public class GalleryItem
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(500)]
        public string ImageUrl { get; set; } = string.Empty;

        [Required, MaxLength(250)]
        public string AltText { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Caption { get; set; }

        public GalleryCategory Category { get; set; } = GalleryCategory.Fieldwork;

        public DateTime? EventDate { get; set; }

        [MaxLength(200)]
        public string? Location { get; set; }

        public int? EventId { get; set; }
        public Event? Event { get; set; }

        public int DisplayOrder { get; set; } = 0;

        // Navigations
        public ICollection<GalleryResearchArea> GalleryResearchAreas { get; set; } = new List<GalleryResearchArea>();
    }

    public class GalleryResearchArea
    {
        public int GalleryItemId { get; set; }
        public GalleryItem? GalleryItem { get; set; }

        public int ResearchDomainId { get; set; }
        public ResearchDomain? ResearchDomain { get; set; }
    }
}
