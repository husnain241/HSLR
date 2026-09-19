using System.ComponentModel.DataAnnotations;

namespace HSLR.Models.Entities
{
    public class SiteConfig
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string SiteName { get; set; } = "Hydroclimatic Sensing Research Lab (HSRL)";

        [Required, MaxLength(250)]
        public string Tagline { get; set; } = "Advanced Remote Sensing, Hydrological Modeling & Climate Analytics";

        [Required]
        public string Description { get; set; } = "HSRL is an interdisciplinary research laboratory studying hydrology, climate, and the environment through satellite remote sensing, GIS, hydrological modeling, AI/ML, and data-driven methods to solve real-world water challenges.";

        [Required, MaxLength(150)]
        public string DirectorName { get; set; } = "Prof. Dr. Muhammad Waseem Boota";

        [Required, EmailAddress, MaxLength(150)]
        public string ContactEmail { get; set; } = "hsrl@uet.edu.pk";

        [MaxLength(50)]
        public string Phone { get; set; } = "+92-42-99029200";

        [Required, MaxLength(200)]
        public string Location { get; set; } = "CEWRE, UET Lahore, GT Road, Lahore 54890, Pakistan";

        [Required, MaxLength(250)]
        public string Affiliation { get; set; } = "Centre of Excellence in Water Resources Engineering (CEWRE), University of Engineering and Technology (UET) Lahore";

        [MaxLength(200)]
        public string? TwitterUrl { get; set; }

        [MaxLength(200)]
        public string? LinkedInUrl { get; set; }

        [MaxLength(200)]
        public string? GoogleScholarUrl { get; set; }

        [MaxLength(200)]
        public string? GitHubUrl { get; set; }
    }
}
