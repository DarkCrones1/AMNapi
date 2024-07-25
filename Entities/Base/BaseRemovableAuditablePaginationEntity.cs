using System.ComponentModel.DataAnnotations.Schema;

using AMNApi.Entities.Interfaces.Entities;

namespace AMNApi.Entities.Base;

public abstract class BaseRemovableAuditablePaginationEntity : BaseRemovableAuditableEntity, IPaginationQueryable
{
    [NotMapped]
    public int PageSize { get; set; }

    [NotMapped]
    public int PageNumber { get; set; }
}