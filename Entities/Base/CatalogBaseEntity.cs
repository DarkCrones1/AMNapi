using AMNApi.Entities.Interfaces.Entities;

namespace AMNApi.Entities.Base;
public abstract class CatalogBaseEntity : BaseEntity, ICatalogBaseEntity, IRemovableEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool? IsDeleted { get; set; }
}