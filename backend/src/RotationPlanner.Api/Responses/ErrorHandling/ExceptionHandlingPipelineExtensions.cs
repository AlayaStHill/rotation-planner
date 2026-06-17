using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace RotationPlanner.Api.Responses.ErrorHandling;

public static class ExceptionHandlingPipelineExtensions
{
    public static WebApplication UseGlobalExceptionHandling(this WebApplication app)
    {
        bool isDevelopment = app.Environment.IsDevelopment();

        app.UseExceptionHandler(exceptionPipeline =>
        {
            // Run registers the final middleware in the exception pipeline.
            // HttpContext contains the current request, response, and ASP.NET Core features for this error.
            exceptionPipeline.Run(async (HttpContext httpContext) =>
            {
                // This code runs inside a static extension method, so ILogger cannot be injected through a constructor.
                // RequestServices gives access to the DI container for the current request scope.
                // ILoggerFactory is used to create a logger with a category name for exception handling.
                ILogger logger = httpContext.RequestServices
                    .GetRequiredService<ILoggerFactory>()
                    .CreateLogger("RotationPlanner.Api.ExceptionHandling");

                // When UseExceptionHandler catches an exception, ASP.NET Core stores error information
                // in HttpContext.Features and exposes it through IExceptionHandlerFeature.
                IExceptionHandlerFeature? exceptionFeature = httpContext.Features.Get<IExceptionHandlerFeature>();

                // Error contains the actual exception-object, that was thrown
                Exception? caughtException = exceptionFeature?.Error;

                if (caughtException is not null)
                {
                    // The message uses structured logging. {Method} and {Path} are named placeholders,
                    // and the values are passed separately. This allows logging providers to store Method
                    // and Path as searchable log fields.
                    logger.LogError(
                        caughtException,
                        "An exception occurred while processing {Method} {Path}", 
                        httpContext.Request.Method,
                        httpContext.Request.Path);
                }

                int statusCode = GetStatusCode(caughtException);

                ProblemDetails problemDetails = ProblemDetailsBuilder.Create
                (
                    type: GetProblemType(caughtException),
                    statusCode: statusCode,
                    title: GetProblemTitle(statusCode),
                    detail: GetProblemDetail(caughtException, isDevelopment),
                    instance: httpContext.Request.Path.ToString()
                ); 

                httpContext.Response.Clear();
                httpContext.Response.StatusCode = statusCode;
                httpContext.Response.ContentType = "application/problem+json";

                await httpContext.Response.WriteAsJsonAsync(problemDetails);
            });
        });

        return app;
    }


    private static int GetStatusCode(Exception? exception)
    {
        return exception switch
        {
            BadHttpRequestException => StatusCodes.Status400BadRequest,
            JsonException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
    }

    private static string GetProblemType(Exception? exception)
    {
        return exception switch
        {
            BadHttpRequestException => ProblemTypes.HttpInvalidRequest,
            JsonException => ProblemTypes.JsonInvalidPayload,
            _ => ProblemTypes.UnexpectedException
        };
    }

    private static string GetProblemTitle(int statusCode)
    {
        return statusCode switch
        {
            StatusCodes.Status400BadRequest => "Bad request",
            _ => "Internal server error"
        };
    }

    private static string GetProblemDetail(Exception? exception, bool isDevelopment)
    {
        return exception switch
        {
            BadHttpRequestException => "Invalid request format.",
            JsonException => "Invalid JSON payload.",
            _ when isDevelopment && exception is not null => exception.Message,
            _ => "An unexpected error occurred."
        };
    }
}
