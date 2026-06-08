using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RotationPlanner.Infrastructure.Extensions;

public static class InfrastructureRegistrationExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        return services;
    }
}
