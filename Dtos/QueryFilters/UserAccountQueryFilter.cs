using AMNApi.Entities.Interfaces.Entities;
using AMNApi.Filters;

namespace AMNApi.Dtos.QueryFilters;

public class UserAccountQueryFilter : PaginationControlRequestFilter, IBaseQueryFilter
{
    public int Id { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public bool? IsDeleted { get; set; }
}