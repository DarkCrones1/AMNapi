namespace AMNApi.Entities.Interfaces.Entities;

public interface IPagedList<T> : IEnumerable<T>
{
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public bool HasNexPage { get; }
    public bool HasPreviousPage { get; }
    public string? NextPageUrl { get; set; }
    public string? PreviousPageUrl { get; set; }
    public int? NextPageNumber { get; }
    public int? PreviousPageNumber { get; }
}
