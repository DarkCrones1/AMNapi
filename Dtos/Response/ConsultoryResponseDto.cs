namespace AMNApi.Dtos.Response;

public class ConsultoryResponseDto
{
    public int Id { get; set; }

    public Guid Code { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Phone { get; set; } = null!;

    public string Address1 { get; set; } = null!;

    public string? Address2 { get; set; }

    public string Street { get; set; } = null!;

    public string ExternalNumber { get; set; } = null!;

    public string? InternalNumber { get; set; }

    public string ZipCode { get; set; } = null!;

    public string FullAddress { get; set; } = null!;

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}