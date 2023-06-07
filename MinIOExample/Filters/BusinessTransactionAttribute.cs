using Microsoft.AspNetCore.Mvc.Filters;
using MinIOExample.Application.Interfaces;

namespace MinIOExample.Filters;

[AttributeUsage(AttributeTargets.Method)]
public class BusinessTransactionAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var transactionContext = context.HttpContext
            .RequestServices
            .GetRequiredService<IBusinessTransactionContext>();
        
        await transactionContext.BeginTransactionAsync();
        try
        {
            await next();
            await transactionContext.CommitTransactionAsync();
        }
        catch (Exception e)
        {
            await transactionContext.RollbackTransactionAsync();
        }
    }
}