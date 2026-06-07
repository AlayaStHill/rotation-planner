namespace RotationPlanner.Api.Security.ApiKey;

public static class ApiKeyConfiguration
{
    public static IServiceCollection AddApiKeyConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApiKeyOptions>(configuration.GetSection(ApiKeyOptions.SectionName));

        return services;
    }
}
