
namespace AMNApi.Entities.Interfaces.Entities;

public interface ICatalogBaseEntity : IBaseQueryable
{
    public string Name { get; set; }

    public string? Description { get; set; }
}