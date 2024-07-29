using AMNApi.Entities.Interfaces.Entities;
using AMNApi.Filters;

namespace AMNApi.Dtos.QueryFilters;

public class AppointmentQueryFilter : PaginationControlRequestFilter, IBaseQueryFilter
{
    public int Id { get; set; }
}