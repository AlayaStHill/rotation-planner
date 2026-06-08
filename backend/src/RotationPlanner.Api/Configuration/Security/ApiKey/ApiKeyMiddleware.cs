using Microsoft.Extensions.Options;

namespace RotationPlanner.Api.Configuration.Security.ApiKey;

public sealed class ApiKeyMiddleware(RequestDelegate next, IOptions<ApiKeyOptions> options)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // IOptions<T> has a property named Value that contains the actual configuration object of type T (in this case, ApiKeyOptions). This allows the middleware to access the configured API key and header name.
        ApiKeyOptions apiKeyOptions = options.Value;

        // If the request is for /docs or /openapi, skip API key validation to allow access to documentation without authentication.
        if (IsDocumentationRequest(context))
        {
            await next(context);
            return;
        }

        if (string.IsNullOrWhiteSpace(apiKeyOptions.Value))
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("API key is not configured.");
            return;
        }

        if (!context.Request.Headers.TryGetValue(apiKeyOptions.HeaderName, out var providedApiKey))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("API key is missing.");
            return;
        }

        if (!string.Equals(providedApiKey, apiKeyOptions.Value, StringComparison.Ordinal))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("API key is invalid.");
            return;
        }

        // Represents the next step in the middleware pipeline. Context consists of all the information about the HTTP request and response, as well as other relevant data.
        await next(context);
    }

    private static bool IsDocumentationRequest(HttpContext context)
    {
        return context.Request.Path.StartsWithSegments("/docs") || context.Request.Path.StartsWithSegments("/openapi");
    }
}