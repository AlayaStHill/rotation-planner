using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace RotationPlanner.Api.Logging.CorrelationIds;

public sealed class CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
{
    private const int MaxCorrelationIdLength = 128;

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

            if (IsValidCorrelationId(correlationId))
            {
                return correlationId;
            }
        }

        string? traceId = Activity.Current?.TraceId.ToString();

        if (IsValidCorrelationId(traceId))
        {
            return traceId;
        }

        return CreateCorrelationId();
    }

    private static string CreateCorrelationId()
    {
        return Guid.NewGuid().ToString("N");
    }

    private static bool IsValidCorrelationId([NotNullWhen(true)] string? correlationId)
    {
        if (string.IsNullOrWhiteSpace(correlationId))
        {
            return false;
        }

        if (correlationId.Length > MaxCorrelationIdLength)
        {
            return false;
        }

        // A string can be treated as a sequence of chars.
        // All is a LINQ method that checks all elements in a sequence and returns true only if all satisfy a condition. The condition is defiend by IsAllowedCorrelationIdCharacter, which is called for each of the characters. 
        return correlationId.All(IsAllowedCorrelationIdCharacter);
    }
    
    private static bool IsAllowedCorrelationIdCharacter(char character)
    {
        return character is >= 'a' and <= 'z'
            || character is >= 'A' and <= 'Z'
            || character is >= '0' and <= '9'
            || character == '-'
            || character == '_'
            || character == '.';
    }
}


