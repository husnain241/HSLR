using HSLR.Models.Entities;

namespace HSLR.Models.ViewModels
{
    public class PublicationsIndexViewModel
    {
        public string? Search { get; set; }
        public int? Year { get; set; }
        public PublicationType? Type { get; set; }
        public int? DomainId { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        public List<Publication> Publications { get; set; } = new();
        public List<int> AvailableYears { get; set; } = new();
        public List<ResearchDomain> AvailableDomains { get; set; } = new();

        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;
        public int FirstItemIndex => TotalItems == 0 ? 0 : (Page - 1) * PageSize + 1;
        public int LastItemIndex => Math.Min(Page * PageSize, TotalItems);
    }
}
