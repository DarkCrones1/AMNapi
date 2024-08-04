namespace AMNApi.Dtos.Response;

public class PatientResponseDto
{
    public int Id { get; set; }

    public Guid Code { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public string CellPhone { get; set; } = null!;

    public short? Gender { get; set; }

    public string GenderName { get; set; } = null!;

    public DateTime? BirthDate { get; set; }

    public string FullName { get => $"{FirstName} {MiddleName} {LastName}".Trim(); }

    public bool IsDeleted { get; set; }

    public string Address1 { get; set; } = null!;

    public string? Address2 { get; set; }

    public string Street { get; set; } = null!;

    public string ExternalNumber { get; set; } = null!;

    public string? InternalNumber { get; set; }

    public string ZipCode { get; set; } = null!;

    public string FullAddress { get; set; } = null!;
}