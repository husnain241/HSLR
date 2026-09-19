using System.ComponentModel.DataAnnotations;

namespace HSLR.Models.Entities
{
    public class CollaborationSector
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public CollaborationCategory Category { get; set; } = CollaborationCategory.Academia;

        [Required, MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string IconName { get; set; } = "bi-building";

        public int DisplayOrder { get; set; } = 0;

        public ICollection<CollaborationEngagementAvenue> EngagementAvenues { get; set; } = new List<CollaborationEngagementAvenue>();
    }

    public class CollaborationEngagementAvenue
    {
        public int Id { get; set; }
        public int CollaborationSectorId { get; set; }
        public CollaborationSector? CollaborationSector { get; set; }

        [Required, MaxLength(300)]
        public string Avenue { get; set; } = string.Empty;

        public int DisplayOrder { get; set; } = 0;
    }

    public class CollaborationInquiry
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string Organization { get; set; } = string.Empty;

        public AffiliationType AffiliationType { get; set; } = AffiliationType.Academic;

        public CollaborationType CollaborationType { get; set; } = CollaborationType.ResearchCollaboration;

        public int? ResearchDomainId { get; set; }
        public ResearchDomain? ResearchDomain { get; set; }

        [MaxLength(250)]
        public string? Title { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        public DateTime SubmittedAtUtc { get; set; } = DateTime.UtcNow;

        public InquiryStatus Status { get; set; } = InquiryStatus.New;

        public bool IsSpam { get; set; } = false;

        [MaxLength(1000)]
        public string? AdminNotes { get; set; }
    }
}
