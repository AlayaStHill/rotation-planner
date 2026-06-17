using System.Diagnostics;

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

        using (IDisposable? loggingScope = logger.BeginScope(scopeValues))
        {
            // Continue the request pipeline while the correlation id scope is active.
            await next(httpContext);
        }
    }

    private static string GetOrCreateCorrelationId(HttpContext httpContext)
    {
        if (httpContext.Request.Headers.TryGetValue(CorrelationIdHeaderNames.CorrelationId,
        out Microsoft.Extensions.Primitives.StringValues correlationIdValues))
        {
            string? correlationId = correlationIdValues.FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(correlationId))
            {
                return correlationId;
            }
        }

        string? traceId = Activity.Current?.TraceId.ToString();

        if (!string.IsNullOrWhiteSpace(traceId))
        {
            return traceId;
        }

        return Guid.NewGuid().ToString("N");
    }
}


