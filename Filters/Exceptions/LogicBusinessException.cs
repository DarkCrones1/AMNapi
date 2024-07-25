namespace AMNApi.Filters.Exceptions;

public class LogicBusinessException : Exception
{
    public LogicBusinessException()
    {
    }

    public LogicBusinessException(string message) : base(message)
    {
    }

    public LogicBusinessException(Exception innerException, string? message = "Ocurrió un fallo al intentar procesar tu solicitud, intentalo de nuevo....") : base(message, innerException)
    {
    }
}