using HSLR.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HSLR.Data
{
    public class HsrlDbContext : IdentityDbContext<IdentityUser>
    {
        public HsrlDbContext(DbContextOptions<HsrlDbContext> options)
            : base(options)
        {
        }

        public DbSet<Person> People => Set<Person>();
        public DbSet<PersonResearchInterest> PersonResearchInterests => Set<PersonResearchInterest>();
        public DbSet<PersonResearchArea> PersonResearchAreas => Set<PersonResearchArea>();

        public DbSet<Publication> Publications => Set<Publication>();
        public DbSet<PublicationAuthor> PublicationAuthors => Set<PublicationAuthor>();
        public DbSet<PublicationResearchArea> PublicationResearchAreas => Set<PublicationResearchArea>();

        public DbSet<Project> Projects => Set<Project>();
        public DbSet<ProjectObjective> ProjectObjectives => Set<ProjectObjective>();
        public DbSet<ProjectDataSource> ProjectDataSources => Set<ProjectDataSource>();
        public DbSet<ProjectMethodologyStep> ProjectMethodologySteps => Set<ProjectMethodologyStep>();
        public DbSet<ProjectTechnology> ProjectTechnologies => Set<ProjectTechnology>();
        public DbSet<ProjectKeyFinding> ProjectKeyFindings => Set<ProjectKeyFinding>();
        public DbSet<ProjectResearchArea> ProjectResearchAreas => Set<ProjectResearchArea>();

        public DbSet<Event> Events => Set<Event>();
        public DbSet<GalleryItem> GalleryItems => Set<GalleryItem>();
        public DbSet<GalleryResearchArea> GalleryResearchAreas => Set<GalleryResearchArea>();

        public DbSet<ResearchDomain> ResearchDomains => Set<ResearchDomain>();
        public DbSet<ResearchDomainFocusTopic> ResearchDomainFocusTopics => Set<ResearchDomainFocusTopic>();

        public DbSet<Capability> Capabilities => Set<Capability>();
        public DbSet<CapabilityHighlight> CapabilityHighlights => Set<CapabilityHighlight>();

        public DbSet<OfficialOpportunity> OfficialOpportunities => Set<OfficialOpportunity>();
        public DbSet<OpportunityCategory> OpportunityCategories => Set<OpportunityCategory>();
        public DbSet<OpportunityRecurringPathway> OpportunityRecurringPathways => Set<OpportunityRecurringPathway>();

        public DbSet<CollaborationSector> CollaborationSectors => Set<CollaborationSector>();
        public DbSet<CollaborationEngagementAvenue> CollaborationEngagementAvenues => Set<CollaborationEngagementAvenue>();
        public DbSet<CollaborationInquiry> CollaborationInquiries => Set<CollaborationInquiry>();

        public DbSet<SiteConfig> SiteConfigs => Set<SiteConfig>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Unique Slugs
            builder.Entity<Person>()
                .HasIndex(p => p.Slug)
                .IsUnique();

            builder.Entity<ResearchDomain>()
                .HasIndex(r => r.Slug)
                .IsUnique();

            builder.Entity<Project>()
                .HasIndex(p => p.Slug)
                .IsUnique();

            builder.Entity<Event>()
                .HasIndex(e => e.Slug)
                .IsUnique();

            // Many-to-Many Composite Keys
            builder.Entity<PersonResearchArea>()
                .HasKey(pra => new { pra.PersonId, pra.ResearchDomainId });

            builder.Entity<PersonResearchArea>()
                .HasOne(pra => pra.Person)
                .WithMany(p => p.PersonResearchAreas)
                .HasForeignKey(pra => pra.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PersonResearchArea>()
                .HasOne(pra => pra.ResearchDomain)
                .WithMany(r => r.PersonResearchAreas)
                .HasForeignKey(pra => pra.ResearchDomainId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PublicationResearchArea>()
                .HasKey(pra => new { pra.PublicationId, pra.ResearchDomainId });

            builder.Entity<PublicationResearchArea>()
                .HasOne(pra => pra.Publication)
                .WithMany(p => p.PublicationResearchAreas)
                .HasForeignKey(pra => pra.PublicationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PublicationResearchArea>()
                .HasOne(pra => pra.ResearchDomain)
                .WithMany(r => r.PublicationResearchAreas)
                .HasForeignKey(pra => pra.ResearchDomainId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProjectResearchArea>()
                .HasKey(pra => new { pra.ProjectId, pra.ResearchDomainId });

            builder.Entity<ProjectResearchArea>()
                .HasOne(pra => pra.Project)
                .WithMany(p => p.ProjectResearchAreas)
                .HasForeignKey(pra => pra.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProjectResearchArea>()
                .HasOne(pra => pra.ResearchDomain)
                .WithMany(r => r.ProjectResearchAreas)
                .HasForeignKey(pra => pra.ResearchDomainId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<GalleryResearchArea>()
                .HasKey(gra => new { gra.GalleryItemId, gra.ResearchDomainId });

            builder.Entity<GalleryResearchArea>()
                .HasOne(gra => gra.GalleryItem)
                .WithMany(g => g.GalleryResearchAreas)
                .HasForeignKey(gra => gra.GalleryItemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<GalleryResearchArea>()
                .HasOne(gra => gra.ResearchDomain)
                .WithMany(r => r.GalleryResearchAreas)
                .HasForeignKey(gra => gra.ResearchDomainId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cascade deletes on children
            builder.Entity<PublicationAuthor>()
                .HasOne(pa => pa.Publication)
                .WithMany(p => p.Authors)
                .HasForeignKey(pa => pa.PublicationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProjectObjective>()
                .HasOne(po => po.Project)
                .WithMany(p => p.Objectives)
                .HasForeignKey(po => po.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProjectDataSource>()
                .HasOne(pd => pd.Project)
                .WithMany(p => p.DataSources)
                .HasForeignKey(pd => pd.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProjectMethodologyStep>()
                .HasOne(pm => pm.Project)
                .WithMany(p => p.MethodologySteps)
                .HasForeignKey(pm => pm.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProjectTechnology>()
                .HasOne(pt => pt.Project)
                .WithMany(p => p.Technologies)
                .HasForeignKey(pt => pt.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProjectKeyFinding>()
                .HasOne(pk => pk.Project)
                .WithMany(p => p.KeyFindings)
                .HasForeignKey(pk => pk.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
