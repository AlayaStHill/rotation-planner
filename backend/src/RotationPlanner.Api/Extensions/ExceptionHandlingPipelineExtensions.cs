using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RotationPlanner.Api.Responses.Errors;
using System.Text.Json;

namespace RotationPlanner.Api.Extensions;

public static class ExceptionHandlingPipelineExtensions
{
    public static void UseGlobalExceptionHandling(this WebApplication app)
    {
        bool isDevelopment = app.Environment.IsDevelopment();

        app.UseExceptionHandler(exceptionPipeline =>
        {
            // Run registers the final middleware in the exception pipeline.
            // HttpContext contains the current request, response, and ASP.NET Core features for this error.
            exceptionPipeline.Run(async (HttpContext httpContext) =>
            {
                IExceptionHandlerFeature? exceptionFeature =
                    httpContext.Features.Get<IExceptionHandlerFeature>();

                // The property Error contains the actual exception-object, that was thrown
                Exception? caughtException = exceptionFeature?.Error;

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
