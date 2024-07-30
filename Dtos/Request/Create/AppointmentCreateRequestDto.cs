namespace AMNApi.Dtos.Request.Create;

public class AppointmentCreateRequestDto
{
    public int DoctorId { get; set; }

    public int PatientId { get; set; }

    public int ConsultoryId { get; set; }

    public DateTime AppointmentDate { get; set; }
}