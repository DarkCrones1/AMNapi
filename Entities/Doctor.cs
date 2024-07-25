using AMNApi.Entities.Base;

namespace AMNApi.Entities;

public partial class Doctor : BaseRemovableAuditablePaginationEntity
{
    public Guid Code { get; set; }

    public int ConsultoryId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public short? Gender { get; set; }

    public DateTime? BirthDate { get; set; }

    public string FullName { get => $"{FirstName} {MiddleName} {LastName}".Trim(); }

    public virtual ICollection<Appointment> Appointment { get; } = new List<Appointment>();

    public virtual Consultory Consultory { get; set; } = null!;

    public virtual ICollection<UserAccount> UserAccount { get; } = new List<UserAccount>();
}