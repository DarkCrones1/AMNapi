namespace AMNApi.Dtos.Request.Create;

public class DoctorCreateRequestDto
{
    public int ConsultoryId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public short? Gender { get; set; }

    public DateTime? BirthDate { get; set; }
}