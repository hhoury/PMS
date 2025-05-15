namespace PMS.Application.Models
{
    public record QueryParameters
    {
        public int PageNumber { get; init; }
        public int PageSize { get; init; } = 15;
        public string? SortBy { get; init; }
        public string? SortOrder { get; init; } = "asc";
    }
}
