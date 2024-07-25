namespace AMNApi.Entities.Interfaces.Entities;

public interface IAuditableEntity : IBaseQueryable
{
    public DateTime CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? LastModifiedDate { get; set; }

    public string? LastModifiedBy { get; set; }
}
