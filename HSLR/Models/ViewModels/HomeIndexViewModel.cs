using HSLR.Models.Entities;

namespace HSLR.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public SiteConfig Config { get; set; } = new();
        public List<ResearchDomain> ResearchDomains { get; set; } = new();
        public List<Capability> Capabilities { get; set; } = new();
        public List<Project> FeaturedProjects { get; set; } = new();
        public List<Publication> FeaturedPublications { get; set; } = new();
        public List<Person> FeaturedPeople { get; set; } = new();
        public Person? Director { get; set; }
        public List<Event> UpcomingEvents { get; set; } = new();

        public int TotalPublications { get; set; }
        public int TotalProjects { get; set; }
        public int TotalScholars { get; set; }
        public int TotalDomains { get; set; }
    }
}
