using AMNApi.Entities.Base;

namespace AMNApi.Entities;

public partial class UserAccount : BaseRemovablePaginationEntity
{
    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsAuthorized { get; set; }

    public short AccountType { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<Doctor> Doctor { get; } = new List<Doctor>();

    public virtual ICollection<Patient> Patient { get; } = new List<Patient>();
}