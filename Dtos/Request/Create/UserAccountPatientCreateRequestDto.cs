namespace AMNApi.Dtos.Request.Create;

public class UserAccountPatientCreateRequestDto
{
    // UserAccount
    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Email { get; set; }
}