using Microsoft.AspNetCore.Mvc;
using RotationPlanner.Api.Responses.Errors;
using RotationPlanner.Application.Results;

namespace RotationPlanner.Api.Responses.Mapping;

public static class ResultMappingExtensions
{
    public static IActionResult MapToActionResult(this Result result, HttpContext httpContext)
    {
        if (result.IsSuccess)
            return new NoContentResult();

        return MapErrorToProblemDetailsResult(result.Error, httpContext);
    }

    public static IActionResult MapToActionResult<T>(this Result<T> result, HttpContext httpContext)
    {
        if (result.IsSuccess)
            return new OkObjectResult(result.Value);

        return MapErrorToProblemDetailsResult(result.Error, httpContext);
    }

    private static IActionResult MapErrorToProblemDetailsResult(ResultError? error, HttpContext httpContext)
    {
        if (error is null)
            return CreateFallbackProblemDetailsResult(httpContext);

        int statusCode = GetStatusCode(error.Type);

        ProblemDetails problemDetails = ProblemDetailsBuilder.Create
        (
            type: error.Code,
            statusCode: statusCode,
            title: GetProblemTitle(error.Type),
            detail: error.Details ?? error.Message,
            instance: httpContext.Request.Path.ToString()
        );

        return new ObjectResult(problemDetails)
        {
            StatusCode = statusCode,
            ContentTypes =
            {
                ProblemContentTypes.ApplicationProblemJson
            }
        };
    }


    private static IActionResult CreateFallbackProblemDetailsResult(HttpContext httpContext)
    {
        const int statusCode = StatusCodes.Status500InternalServerError;

        ProblemDetails problemDetails = ProblemDetailsBuilder.Create
        (
            type: ProblemTypes.ApplicationResultMissingError,
            statusCode: statusCode,
            title: "Unexpected error",
            detail: "The application returned a failed result without error information.",
            instance: httpContext.Request.Path.ToString()
        );

        return new ObjectResult(problemDetails)
        {
            StatusCode = statusCode,
            ContentTypes =
            {
                ProblemContentTypes.ApplicationProblemJson
            }
        };
    }

    private static int GetStatusCode(ErrorType type)
    {
        return type switch
        {
            ErrorType.BadRequest => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unexpected => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };
    }

    private static string GetProblemTitle(ErrorType type)
    {
        return type switch
        {
            ErrorType.BadRequest => "Bad request",
            ErrorType.NotFound => "Resource not found",
            ErrorType.Conflict => "Conflict",
            ErrorType.Unexpected => "Unexpected error",
            _ => "Unexpected error"
        };
    }
}
