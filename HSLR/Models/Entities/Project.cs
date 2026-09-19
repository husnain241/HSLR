using System.ComponentModel.DataAnnotations;

namespace HSLR.Models.Entities
{
    public class Project
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Slug { get; set; } = string.Empty;

        [Required, MaxLength(300)]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string ShortTitle { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? Subtitle { get; set; }

        public ProjectStatus Status { get; set; } = ProjectStatus.Active;

        public ProjectType ProjectType { get; set; } = ProjectType.Research;

        public int Year { get; set; }

        [Required, MaxLength(200)]
        public string StudyArea { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Country { get; set; } = "Pakistan";

        [Required]
        public string Challenge { get; set; } = string.Empty;

        public string? Impact { get; set; }

        public bool Featured { get; set; } = false;

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        public int DisplayOrder { get; set; } = 0;

        // Navigations
        public ICollection<ProjectObjective> Objectives { get; set; } = new List<ProjectObjective>();
        public ICollection<ProjectDataSource> DataSources { get; set; } = new List<ProjectDataSource>();
        public ICollection<ProjectMethodologyStep> MethodologySteps { get; set; } = new List<ProjectMethodologyStep>();
        public ICollection<ProjectTechnology> Technologies { get; set; } = new List<ProjectTechnology>();
        public ICollection<ProjectKeyFinding> KeyFindings { get; set; } = new List<ProjectKeyFinding>();
        public ICollection<ProjectResearchArea> ProjectResearchAreas { get; set; } = new List<ProjectResearchArea>();
    }

    public class ProjectObjective
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        [Required, MaxLength(400)]
        public string Objective { get; set; } = string.Empty;

        public int DisplayOrder { get; set; } = 0;
    }

    public class ProjectDataSource
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        [Required, MaxLength(200)]
        public string SourceName { get; set; } = string.Empty;

        public int DisplayOrder { get; set; } = 0;
    }

    public class ProjectMethodologyStep
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        [Required, MaxLength(200)]
        public string StepTitle { get; set; } = string.Empty;

        [Required, MaxLength(1000)]
        public string StepDescription { get; set; } = string.Empty;

        public int DisplayOrder { get; set; } = 0;
    }

    public class ProjectTechnology
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        [Required, MaxLength(150)]
        public string TechnologyName { get; set; } = string.Empty;

        public int DisplayOrder { get; set; } = 0;
    }

    public class ProjectKeyFinding
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        [Required, MaxLength(500)]
        public string Finding { get; set; } = string.Empty;

        public int DisplayOrder { get; set; } = 0;
    }

    public class ProjectResearchArea
    {
        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        public int ResearchDomainId { get; set; }
        public ResearchDomain? ResearchDomain { get; set; }
    }
}
