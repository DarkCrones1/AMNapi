namespace AMNApi.Dtos.Response;

public class MapLocationResponseDto
{
    public int ConsultoryId { get; set; }

    public string ConsultoryNAme { get; set; } = string.Empty;

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}