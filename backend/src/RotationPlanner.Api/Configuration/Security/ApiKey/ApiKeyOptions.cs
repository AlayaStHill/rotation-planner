namespace RotationPlanner.Api.Configuration.Security.ApiKey;

public sealed class ApiKeyOptions
{
    public const string SectionName = "ApiKey";
    public string HeaderName { get; init; } = "x-api-key";
    public string Value { get; init; } = null!;
}
