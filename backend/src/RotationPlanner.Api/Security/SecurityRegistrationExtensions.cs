using RotationPlanner.Api.Security.ApiKey;
using RotationPlanner.Api.Security.Cors;

namespace RotationPlanner.Api.Security;

public static class SecurityRegistrationExtensions
{
    public static IServiceCollection AddSecurityConfigurations(this IServiceCollection services, IConfiguration configurations)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddCorsConfiguration();
        services.AddApiKeyConfiguration(configurations);

        return services.AddSecurityConfigurations(configurations);
    }
}
