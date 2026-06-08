using Microsoft.Extensions.DependencyInjection;

namespace RotationPlanner.Application.Extensions;

public static class ApplicationRegistrationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        return services;
    }
}
