using AMNApi.Entities.Interfaces.Entities;
using AMNApi.Filters;

namespace AMNApi.Dtos.QueryFilters;

public class MapLocationQueryFilter : PaginationControlRequestFilter, IBaseQueryFilter
{
    public int Id { get; set; }

    public int ConsultoryId { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }
}