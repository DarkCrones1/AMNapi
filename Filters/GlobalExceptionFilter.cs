using AMNApi.Filters.Exceptions;
using AMNApi.Filters.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace AMNApi.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception.GetType() == typeof(BusinessException))
            DisplayBusinessException(context);
        else if (context.Exception.GetType() == typeof(LogicBusinessException))
            DisplayOtherException(context);
    }

    private static void DisplayBusinessException(ExceptionContext context)
    {
        var exception = (BusinessException)context.Exception;
        var validation = new
        {
            Status = 400,
            Title = "Bad Request",
            Detail = exception.Message
        };

        var json = new
        {
            errors = new[] { validation }
        };

        context.Result = new BadRequestObjectResult(json);
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.ExceptionHandled = true;
    }

    private static void DisplayOtherException(ExceptionContext context)
    {
        var exception = (LogicBusinessException)context.Exception;
        var validation = new
        {
            Status = 500,
            Title = "Internal Server Error",
            Detail = exception.Message
            //TODO: Enviar la excepción real al log
        };

        var json = new
        {
            errors = new[] { validation }
        };

        context.Result = new InternalServerErrorObjectResult(json);
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.ExceptionHandled = true;
    }
}