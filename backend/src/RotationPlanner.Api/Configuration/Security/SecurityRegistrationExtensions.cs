using RotationPlanner.Api.Configuration.Security.ApiKey;
using RotationPlanner.Api.Configuration.Security.Cors;

namespace RotationPlanner.Api.Configuration.Security;

public static class SecurityRegistrationExtensions
{
    public static IServiceCollection AddSecurityConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddCorsConfiguration();
        services.AddApiKeyConfiguration(configuration);

        return services.AddSecurityConfigurations(configuration);
    }
}
