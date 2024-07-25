using System.ComponentModel;

namespace AMNApi.Entities.Enumerations;

public enum UserAccountType
{
    [Description("Doctor")]
    Doctor = 1,
    [Description("Paciente")]
    Patient = 2
}