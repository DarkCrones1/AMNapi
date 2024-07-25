using AMNApi.Entities.Interfaces.Entities;

namespace AMNApi.Entities.Base;

public class PagedList<T> : List<T>, IPagedList<T>
{
    public PagedList(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
    {
        TotalCount = totalCount;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

        AddRange(items);
    }

    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public bool HasNexPage { get => CurrentPage > 1; }
    public bool HasPreviousPage { get => CurrentPage < TotalPages; }
    public string? NextPageUrl { get; set; }
    public string? PreviousPageUrl { get; set; }
    public int? NextPageNumber { get => HasNexPage ? CurrentPage + 1 : null; }
    public int? PreviousPageNumber { get => HasPreviousPage ? CurrentPage - 1 : null; }

    public static PagedList<T> Create(IEnumerable<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count();
        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}
