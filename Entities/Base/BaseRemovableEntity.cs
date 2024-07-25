using AMNApi.Entities.Interfaces.Entities;

namespace AMNApi.Entities.Base;
public abstract class BaseRemovableEntity : BaseEntity, IRemovableEntity
{
    public bool? IsDeleted { get; set; }
}