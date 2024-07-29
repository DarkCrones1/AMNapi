namespace AMNApi.Dtos.Request.Create;

public class UserAccountDoctorCreateRequestDto
{
    // UserAccount
    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Email { get; set; }

    // Doctor info

    public int ConsultoryId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public short? Gender { get; set; }

    public DateTime? BirthDate { get; set; }
}