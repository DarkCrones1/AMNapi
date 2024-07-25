namespace AMNApi.Response;

public class MetaDataResponse
{
    public MetaDataResponse(int totalCount, int pageNumber, int pageSize)
    {
        TotalCount = totalCount;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }
    public MetaDataResponse(int totalCount = 0, int currentPage = 0, int pageSize = 0, string nextPageUrl = "", string previousPageUrl = "")
    {
        TotalCount = totalCount;
        PageSize = pageSize;
        CurrentPage = currentPage;
        TotalPages = (int)Math.Ceiling(TotalCount / (double)pageSize);
        NextPageUrl = nextPageUrl;
        PreviousPageUrl = previousPageUrl;
    }

    public int TotalCount { get; internal set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; internal set; }
    public bool HasNexPage { get => CurrentPage < TotalPages; }
    public bool HasPreviousPage { get => CurrentPage > 1; }
    public string? NextPageUrl { get; set; }
    public string? PreviousPageUrl { get; set; }
    public int? NextPageNumber { get => HasNexPage ? CurrentPage + 1 : null; }
    public int? PreviousPageNumber { get => HasPreviousPage ? CurrentPage - 1 : null; }
}