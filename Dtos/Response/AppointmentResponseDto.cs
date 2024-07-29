namespace AMNApi.Dtos.Response;

public class AppointmentResponseDto
{
    public int DoctorId { get; set; }

    public int PatientId { get; set; }

    public int ConsultoryId { get; set; }

    public DateTime AppoinmentDate { get; set; }

    public short Status { get; set; }

    public string StatusName { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }
}