using AMNApi.Entities.Base;

namespace AMNApi.Entities;

public partial class Address : BaseEntity
{
    public string FullAddress
    {
        get
        {
            var address = string.Empty;
            var internalNumber = string.IsNullOrEmpty(InternalNumber) ? string.Empty : $"Num.Int:{InternalNumber}";

            address = $"Calle: {Street} Num.Ext:{ExternalNumber} {internalNumber} C.P:{ZipCode}, {Address1} {Address2}";

            return address.Trim();
        }
    }

    public string Address1 { get; set; } = null!;

    public string? Address2 { get; set; }

    public string Street { get; set; } = null!;

    public string ExternalNumber { get; set; } = null!;

    public string? InternalNumber { get; set; }

    public string ZipCode { get; set; } = null!;

    public virtual ICollection<Consultory> Consultory { get; } = new List<Consultory>();

    public virtual ICollection<PatientAddress> PatientAddress { get; } = new List<PatientAddress>();
}