namespace RotationPlanner.Api.Configuration.Security.Cors;

public static class CorsConfigurationExtensions
{
    public const string FrontendPolicy = "Frontend";

    public static IServiceCollection AddFrontendCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(FrontendPolicy, policy =>
            {
                policy.WithOrigins("http://localhost:3000", "https://rotation-planner.vercel.app")
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });

        });

        return services;
    }
}
