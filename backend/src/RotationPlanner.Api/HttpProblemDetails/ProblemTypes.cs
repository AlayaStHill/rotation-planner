namespace RotationPlanner.Api.HttpProblemDetails;

public static class ProblemTypes
{
    public const string HttpInvalidRequest = "Http.InvalidRequest";
    public const string JsonInvalidPayload = "Json.InvalidPayload";
    public const string UnexpectedException = "Unexpected.Exception";
    public const string ApplicationResultMissingError = "Application.Result.MissingError";
}
