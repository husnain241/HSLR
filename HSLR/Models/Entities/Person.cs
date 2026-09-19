using System.ComponentModel.DataAnnotations;

namespace HSLR.Models.Entities
{
    public class Person
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Slug { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string DisplayName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? PhotoUrl { get; set; }

        [Required, MaxLength(150)]
        public string Role { get; set; } = string.Empty;

        public PersonCategory Category { get; set; } = PersonCategory.Researcher;

        public PersonStatus Status { get; set; } = PersonStatus.Active;

        [Required, MaxLength(500)]
        public string ShortBio { get; set; } = string.Empty;

        public string? Biography { get; set; }

        [MaxLength(250)]
        public string? CurrentAffiliation { get; set; }

        public bool Featured { get; set; } = false;

        [Required, MaxLength(10)]
        public string Initials { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? LinkedInUrl { get; set; }

        [MaxLength(300)]
        public string? GoogleScholarUrl { get; set; }

        [MaxLength(300)]
        public string? ResearchGateUrl { get; set; }

        [MaxLength(300)]
        public string? OrcidUrl { get; set; }

        [MaxLength(300)]
        public string? WebsiteUrl { get; set; }

        public int DisplayOrder { get; set; } = 0;

        // Navigations
        public ICollection<PersonResearchInterest> ResearchInterests { get; set; } = new List<PersonResearchInterest>();
        public ICollection<PersonResearchArea> PersonResearchAreas { get; set; } = new List<PersonResearchArea>();
    }

    public class PersonResearchInterest
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public Person? Person { get; set; }

        [Required, MaxLength(200)]
        public string Interest { get; set; } = string.Empty;

        public int DisplayOrder { get; set; } = 0;
    }

    public class PersonResearchArea
    {
        public int PersonId { get; set; }
        public Person? Person { get; set; }

        public int ResearchDomainId { get; set; }
        public ResearchDomain? ResearchDomain { get; set; }
    }
}
