using System.ComponentModel;

namespace AMNApi.Entities.Enumerations;

public enum AppoinmentStatus
{
    [Description("Agendada")]
    Scheduled = 1,
    [Description("Concluida")]
    Concluded = 2,
    [Description("Perdida")]
    Loss = 3,
    [Description("Re-Agendada")]
    ReScheduled = 4,
    [Description("Cancelada")]
    Canceled = 10,

}