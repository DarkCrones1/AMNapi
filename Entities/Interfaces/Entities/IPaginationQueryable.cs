namespace AMNApi.Entities.Interfaces.Entities;

public interface IPaginationQueryable : IBaseQueryFilter
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}