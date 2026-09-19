using System.ComponentModel.DataAnnotations;

namespace HSLR.Models.Entities
{
    public class Publication
    {
        public int Id { get; set; }

        [Required, MaxLength(400)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? Journal { get; set; }

        public int Year { get; set; }

        [MaxLength(150)]
        public string? Doi { get; set; }

        [MaxLength(500)]
        public string? Url { get; set; }

        public PublicationType PublicationType { get; set; } = PublicationType.Journal;

        public string? Abstract { get; set; }

        public bool Featured { get; set; } = false;

        [MaxLength(200)]
        public string? CitationMetrics { get; set; }

        // Navigations
        public ICollection<PublicationAuthor> Authors { get; set; } = new List<PublicationAuthor>();
        public ICollection<PublicationResearchArea> PublicationResearchAreas { get; set; } = new List<PublicationResearchArea>();
    }

    public class PublicationAuthor
    {
        public int Id { get; set; }
        public int PublicationId { get; set; }
        public Publication? Publication { get; set; }

        [Required, MaxLength(150)]
        public string AuthorName { get; set; } = string.Empty;

        public bool IsLabMember { get; set; } = false;

        public int DisplayOrder { get; set; } = 0;
    }

    public class PublicationResearchArea
    {
        public int PublicationId { get; set; }
        public Publication? Publication { get; set; }

        public int ResearchDomainId { get; set; }
        public ResearchDomain? ResearchDomain { get; set; }
    }
}
