using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Assecor.Api.Person.Filter;

[ExcludeFromCodeCoverage(Justification = "Bootstrapping")]
public class ProblemDetailsLoggingFilter(ILogger<ProblemDetailsLoggingFilter> logger) : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is ObjectResult { Value: ProblemDetails problemDetails })
        {
            logger.LogError(
                "Request failed with ProblemDetails (code: {Code}, message: {Message})",
                problemDetails.Type,
                problemDetails.Detail
            );
        }
    }
}
