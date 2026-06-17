namespace RotationPlanner.Api.Configuration.Security.ApiKey.Extensions;

public static class ApiKeyConfigurationExtensions
{
    public static IServiceCollection AddApiKeyConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApiKeyOptions>(configuration.GetSection(ApiKeyOptions.SectionName));

        return services;
    }
}
