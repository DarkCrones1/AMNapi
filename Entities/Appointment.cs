using AMNApi.Entities.Base;

namespace AMNApi.Entities;

public partial class Appointment : BaseRemovableAuditablePaginationEntity
{
    public int DoctorId { get; set; }

    public int PatientId { get; set; }

    public int ConsultoryId { get; set; }

    public DateTime AppoinmentDate { get; set; }

    public short Status { get; set; }

    public virtual Consultory Consultory { get; set; } = null!;

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;

}