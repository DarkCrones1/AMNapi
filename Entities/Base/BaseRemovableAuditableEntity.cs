using AMNApi.Entities.Interfaces.Entities;

namespace AMNApi.Entities.Base;

public abstract class BaseRemovableAuditableEntity : BaseAuditableEntity, IRemovableEntity
{
    public bool? IsDeleted { get; set; }
}