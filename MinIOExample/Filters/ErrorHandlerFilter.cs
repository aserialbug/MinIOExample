using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MinIOExample.Application.Exceptions;

namespace MinIOExample.Filters;

public class ErrorHandlerFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var executed = await next();
        if (executed.Exception is StoredFileNotFound)
        {
            executed.Result = new ObjectResult(executed.Exception.Message)
            {
                StatusCode = StatusCodes.Status404NotFound
            };
            executed.ExceptionHandled = true;
        }
        else if(executed.Exception is ArgumentException ||
                executed.Exception is PolicyViolationException)
        {
            executed.Result = new ObjectResult(executed.Exception.Message)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
            executed.ExceptionHandled = true;
        }
    }
}