namespace AMNApi.Dtos.Response;

public class UserAccountPatientResponseDto
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string CellPhone { get; set; } = string.Empty;

    public short UserAccountType { get; set; }

    public string? UserAccountTypeName { get; set; }

    public bool IsDeleted { get; set; }
}