namespace AMNApi.Dtos.Response;

public class UserAccountDoctorResponseDto
{
    public int Id { get; set; }

    public int DoctorId { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public short UserAccountType { get; set; }

    public string? UserAccountTypeName { get; set; }

    public int ConsultoryId { get; set; }

    public string ConsultoryName { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }
}