using System.ComponentModel.DataAnnotations;

namespace HSLR.Models.Entities
{
    public enum PersonCategory
    {
        [Display(Name = "Lab Director")]
        LabDirector = 1,

        [Display(Name = "Researcher")]
        Researcher = 2,

        [Display(Name = "PhD Scholar")]
        Phd = 3,

        [Display(Name = "MS / MPhil Researcher")]
        MsMphil = 4,

        [Display(Name = "Alumni")]
        Alumni = 5,

        [Display(Name = "Visiting Researcher")]
        VisitingResearcher = 6,

        [Display(Name = "Collaborator")]
        Collaborator = 7
    }

    public enum PersonStatus
    {
        Active = 1,
        Alumni = 2,
        Collaborator = 3,
        Visiting = 4,
        Inactive = 5
    }

    public enum PublicationType
    {
        [Display(Name = "Peer-Reviewed Journal")]
        Journal = 1,

        [Display(Name = "Conference Proceeding")]
        Conference = 2,

        [Display(Name = "Book Chapter")]
        BookChapter = 3,

        [Display(Name = "Technical Report")]
        Report = 4,

        [Display(Name = "Preprint / Working Paper")]
        Preprint = 5
    }

    public enum ProjectStatus
    {
        Active = 1,
        Completed = 2,
        Ongoing = 3,
        Proposed = 4
    }

    public enum ProjectType
    {
        [Display(Name = "Fundamental Research")]
        Research = 1,

        [Display(Name = "Applied / Field Study")]
        Applied = 2,

        [Display(Name = "Collaborative / Multi-institutional")]
        Collaborative = 3,

        [Display(Name = "Postgraduate Thesis")]
        Thesis = 4
    }

    public enum EventType
    {
        Seminar = 1,
        Workshop = 2,
        Conference = 3,
        Fieldwork = 4,
        Webinar = 5
    }

    public enum EventStatus
    {
        Upcoming = 1,
        Past = 2
    }

    public enum GalleryCategory
    {
        Fieldwork = 1,
        Research = 2,
        Events = 3,
        Workshops = 4,
        Outreach = 5
    }

    public enum OpportunityStatus
    {
        Open = 1,
        Closed = 2,
        Upcoming = 3
    }

    public enum OpportunityEngagementType
    {
        [Display(Name = "PhD Candidacy")]
        Phd = 1,

        [Display(Name = "MS / MPhil Research")]
        MsMphil = 2,

        [Display(Name = "Research Internship")]
        Internship = 3,

        [Display(Name = "Research Associate / Assistant")]
        Ra = 4,

        [Display(Name = "Visiting Collaboration")]
        Collaboration = 5
    }

    public enum CollaborationCategory
    {
        Academia = 1,
        Government = 2,
        Ngo = 3,
        Industry = 4,
        Students = 5
    }

    public enum AffiliationType
    {
        Academic = 1,
        Student = 2,
        Government = 3,
        Ngo = 4,
        Industry = 5,
        Independent = 6
    }

    public enum CollaborationType
    {
        [Display(Name = "Joint Research Collaboration")]
        ResearchCollaboration = 1,

        [Display(Name = "Individual Academic Support")]
        IndividualSupport = 2,

        [Display(Name = "Applied Research / Consultancy")]
        AppliedResearch = 3,

        [Display(Name = "Technical Support / Data Access")]
        TechnicalSupport = 4,

        [Display(Name = "Other Collaboration")]
        Other = 5
    }

    public enum InquiryStatus
    {
        New = 1,
        Reviewed = 2,
        Responded = 3,
        Archived = 4
    }
}
