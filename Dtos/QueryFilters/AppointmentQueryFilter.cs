using AMNApi.Entities.Interfaces.Entities;
using AMNApi.Filters;

namespace AMNApi.Dtos.QueryFilters;

public class AppointmentQueryFilter : PaginationControlRequestFilter, IBaseQueryFilter
{
    public int Id { get; set; }

    public short? Status { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? AppointmenDate { get; set; }

    public DateTime? MinAppointmentDate { get; set; }

    public DateTime? MaxAppointmentDate { get; set; }
}