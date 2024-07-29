namespace AMNApi.Dtos.Response;

public class DoctorResponseDto
{
    public int Id { get; set; }

    public Guid Code { get; set; }

    public int ConsultoryId { get; set; }

    public string ConsultoryName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public short? Gender { get; set; }

    public DateTime? BirthDate { get; set; }

    public string FullName { get; set; } = null!;
}