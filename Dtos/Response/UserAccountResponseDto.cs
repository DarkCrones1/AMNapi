namespace AMNApi.Dtos.Response;

public class UserAccountResponseDto
{
    public int Id { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }

    public short AccountType { get; set; }

    public string AccountTypeName { get; set; } = null!;

    public DateTime CreatedDate { get; set; }
}