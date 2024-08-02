namespace AMNApi.Dtos.Request.Create;

public class ConsultoryCreateRequestDto
{
    public string Name { get; set; } = string.Empty;

    public string Phone { get; set; } = null!;

    // Address

    public string Address1 { get; set; } = null!;

    public string? Address2 { get; set; }

    public string Street { get; set; } = null!;

    public string ExternalNumber { get; set; } = null!;

    public string? InternalNumber { get; set; }

    public string ZipCode { get; set; } = null!;

    // MapLocation

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}