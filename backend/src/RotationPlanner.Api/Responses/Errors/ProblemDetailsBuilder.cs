using Microsoft.AspNetCore.Mvc;

namespace RotationPlanner.Api.Responses.Errors;

public static class ProblemDetailsBuilder
{
    public static ProblemDetails Create(string type, int statusCode, string title, string detail, string? instance = null)
    {
        return new ProblemDetails
        {
            Type = type,
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = instance
        };
    }
}
