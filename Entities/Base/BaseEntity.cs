using AMNApi.Entities.Interfaces.Entities;

namespace AMNApi.Entities.Base;

public abstract class BaseEntity : IBaseQueryable
{
    public int Id { get; set; }
}