using AMNApi.Entities.Interfaces.Entities;
using AMNApi.Filters;

namespace AMNApi.Dtos.QueryFilters;

public class ConsultoryQueryFilter : PaginationControlRequestFilter, IBaseQueryFilter
{
    public int Id { get; set; }
}