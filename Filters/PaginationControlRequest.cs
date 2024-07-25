using AMNApi.Entities.Interfaces.Entities;

namespace AMNApi.Filters;

public abstract class PaginationControlRequestFilter : IPaginationQueryable
{
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 1;
}