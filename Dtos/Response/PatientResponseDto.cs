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
}