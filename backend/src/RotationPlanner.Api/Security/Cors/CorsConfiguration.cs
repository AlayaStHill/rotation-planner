namespace RotationPlanner.Api.Security.Cors;

public static class CorsConfiguration
{
    public const string FrontendPolicy = "Frontend";

    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
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
