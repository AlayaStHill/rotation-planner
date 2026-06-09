using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace RotationPlanner.Api.Extensions;

public static class ExceptionHandlingExtensions
{
    public static void UseGlobalExceptionHandling(this WebApplication app)
    {
        bool isDevelopment = app.Environment.IsDevelopment();

        app.UseExceptionHandler(exceptionPipeline =>
        {
            // Run registers the final middleware in the exception pipeline.
            // The HttpContext contains the current request, response, and ASP.NET Core features for this error.
            exceptionPipeline.Run(async (HttpContext httpContext) =>
            {
                IExceptionHandlerFeature? exceptionFeature =
                    httpContext.Features.Get<IExceptionHandlerFeature>();

                // The property Error contains the actual exception-object, that was thrown
                Exception? caughtException = exceptionFeature?.Error;

                int statusCode = caughtException switch
                {
                    BadHttpRequestException => StatusCodes.Status400BadRequest,
                    JsonException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                };

                string title = statusCode switch
                {
                    StatusCodes.Status400BadRequest => "Bad request",
                    _ => "Internal server error"
                };

                string detail = caughtException switch
                {
                    BadHttpRequestException => "Invalid request format.",
                    JsonException => "Invalid JSON payload.",
                    _ when isDevelopment && caughtException is not null => caughtException.Message,
                    _ => "An unexpected error occurred."
                };

                ProblemDetails problemDetails = new()
                {
                    Status = statusCode,
                    Title = title,
                    Detail = detail,
                    Instance = httpContext.Request.Path
                };

                httpContext.Response.Clear();
                httpContext.Response.StatusCode = statusCode;
                httpContext.Response.ContentType = "application/problem+json";

                await httpContext.Response.WriteAsJsonAsync(problemDetails);
            });
        });
    }
}
