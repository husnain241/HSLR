using System.ComponentModel.DataAnnotations;

namespace HSLR.Models.Entities
{
    public class OfficialOpportunity
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Category { get; set; } = string.Empty;

        public DateTime? Deadline { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        public OpportunityStatus Status { get; set; } = OpportunityStatus.Open;

        [MaxLength(500)]
        public string? Link { get; set; }

        public int DisplayOrder { get; set; } = 0;
    }

    public class OpportunityCategory
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(500)]
        public string ShortDescription { get; set; } = string.Empty;

        public OpportunityEngagementType EngagementType { get; set; } = OpportunityEngagementType.Phd;

        public int DisplayOrder { get; set; } = 0;

        public ICollection<OpportunityRecurringPathway> RecurringPathways { get; set; } = new List<OpportunityRecurringPathway>();
    }

    public class OpportunityRecurringPathway
    {
        public int Id { get; set; }
        public int OpportunityCategoryId { get; set; }
        public OpportunityCategory? OpportunityCategory { get; set; }

        [Required, MaxLength(300)]
        public string Pathway { get; set; } = string.Empty;

        public int DisplayOrder { get; set; } = 0;
    }
}
