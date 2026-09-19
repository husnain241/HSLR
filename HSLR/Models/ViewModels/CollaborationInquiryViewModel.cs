using HSLR.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace HSLR.Models.ViewModels
{
    public class CollaborationInquiryViewModel
    {
        [Required(ErrorMessage = "Please enter your full name.")]
        [Display(Name = "Full Name")]
        [StringLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your official or institutional email address.")]
        [EmailAddress(ErrorMessage = "Please provide a valid email address.")]
        [Display(Name = "Email Address")]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please specify your organization or university.")]
        [Display(Name = "Organization / University")]
        [StringLength(200)]
        public string Organization { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select your affiliation type.")]
        [Display(Name = "Affiliation Type")]
        public AffiliationType AffiliationType { get; set; } = AffiliationType.Academic;

        [Required(ErrorMessage = "Please select the type of collaboration you wish to propose.")]
        [Display(Name = "Collaboration Type")]
        public CollaborationType CollaborationType { get; set; } = CollaborationType.ResearchCollaboration;

        [Display(Name = "Research Domain")]
        public int? ResearchDomainId { get; set; }

        [Display(Name = "Proposal / Inquiry Title (Optional)")]
        [StringLength(250)]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Please provide details regarding your collaboration or inquiry.")]
        [Display(Name = "Detailed Proposal / Message")]
        [MinLength(20, ErrorMessage = "Please provide at least 20 characters describing your inquiry.")]
        public string Description { get; set; } = string.Empty;

        // Anti-Spam Honeypot Field (Must remain empty for real humans)
        public string? WebsiteTrap { get; set; }
    }
}
