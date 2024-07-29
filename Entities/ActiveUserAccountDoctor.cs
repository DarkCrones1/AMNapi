using AMNApi.Entities.Base;

namespace AMNApi.Entities;

public partial class ActiveUserAccountDoctor : BaseQueryable
{
    public string UserName { get; set; } = null!;

    public int DoctorId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public short AccountType { get; set; }

    public string Name => $"{this.FirstName} {this.MiddleName} {this.LastName}".Trim();
}