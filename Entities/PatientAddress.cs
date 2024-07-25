using AMNApi.Entities.Base;

namespace AMNApi.Entities;

public partial class PatientAddress : BaseEntity
{
    public int AddressId { get; set; }

    public int PatientId { get; set; }

    public DateTime RegisterDate { get; set; }

    public bool IsDefault { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;
}