using AMNApi.Entities.Base;

namespace AMNApi.Entities;

public partial class Consultory : BaseRemovableAuditablePaginationEntity
{
    public Guid Code { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Phone { get; set; } = null!;

    public virtual ICollection<Address> Address { get; } = new List<Address>();

    public virtual ICollection<Appointment> Appointment { get; } = new List<Appointment>();

    public virtual ICollection<Doctor> Doctor { get; } = new List<Doctor>();

    public virtual ICollection<MapLocation> MapLocation { get; } = new List<MapLocation>();
}