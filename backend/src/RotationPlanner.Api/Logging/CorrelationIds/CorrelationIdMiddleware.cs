namespace RotationPlanner.Api.Logging.CorrelationIds;

public sealed class CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        string correlationId = GetOrCreateCorrelationId(httpContext);

        httpContext.Response.Headers[CorrelationIdHeaderNames.CorrelationId] = correlationId;

        Dictionary<string, object?> scopeValues = new Dictionary<string, object?>
        {
            ["CorrelationId"] = correlationId
        };

        using (logger.BeginScope(scopeValues))
        {
            // Continue the request pipeline while the correlation id scope is active.
            await next(httpContext);
        }
    }

    private static string GetOrCreateCorrelationId(HttpContext httpContext)
    {

    }

}
