namespace AMNApi.Dtos.Response;

public class AppointmentResponseDto
{
    public int Id { get; set; }

    public int DoctorId { get; set; }

    public string DoctorFullName { get; set; } = string.Empty;

    public int PatientId { get; set; }

    public string PatientFullName { get; set; } = string.Empty;

    public int ConsultoryId { get; set; }

    public string ConsultoryName { get; set; } = string.Empty;

    public DateTime AppoinmentDate { get; set; }

    public short Status { get; set; }

    public string StatusName { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }

    public string IsActive { get; set; } = null!;
}