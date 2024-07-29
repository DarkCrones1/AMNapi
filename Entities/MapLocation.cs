using AMNApi.Entities.Base;

namespace AMNApi.Entities;

public partial class MapLocation : BaseRemovableAuditablePaginationEntity
{
    public int ConsultoryId { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public virtual Consultory Consultory { get; set; } = null!;
}