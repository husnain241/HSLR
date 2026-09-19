using System.ComponentModel.DataAnnotations;

namespace HSLR.Models.Entities
{
    public class ResearchDomain
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Slug { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(500)]
        public string ShortDescription { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string IconName { get; set; } = "bi-water";

        public bool Featured { get; set; } = true;

        public int DisplayOrder { get; set; } = 0;

        // Navigation
        public ICollection<ResearchDomainFocusTopic> FocusTopics { get; set; } = new List<ResearchDomainFocusTopic>();
        public ICollection<PersonResearchArea> PersonResearchAreas { get; set; } = new List<PersonResearchArea>();
        public ICollection<PublicationResearchArea> PublicationResearchAreas { get; set; } = new List<PublicationResearchArea>();
        public ICollection<ProjectResearchArea> ProjectResearchAreas { get; set; } = new List<ProjectResearchArea>();
        public ICollection<GalleryResearchArea> GalleryResearchAreas { get; set; } = new List<GalleryResearchArea>();
    }

    public class ResearchDomainFocusTopic
    {
        public int Id { get; set; }
        public int ResearchDomainId { get; set; }
        public ResearchDomain? ResearchDomain { get; set; }

        [Required, MaxLength(250)]
        public string Topic { get; set; } = string.Empty;

        public int DisplayOrder { get; set; } = 0;
    }
}
