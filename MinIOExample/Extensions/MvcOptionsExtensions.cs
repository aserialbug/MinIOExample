using Microsoft.AspNetCore.Mvc;
using MinIOExample.Filters;

namespace MinIOExample.Extensions;

public static class MvcOptionsExtensions
{
    public static MvcOptions AddFilters(this MvcOptions options)
    {
        return options
            .AddErrorHandler();
    }
    
    public static MvcOptions AddErrorHandler(this MvcOptions options)
    {
        options.Filters.Add<ErrorHandlerFilter>();
        return options;
    }
}