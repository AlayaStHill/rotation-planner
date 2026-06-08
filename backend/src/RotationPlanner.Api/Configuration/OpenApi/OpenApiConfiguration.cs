namespace RotationPlanner.Api.Configuration.OpenApi;

// OpenAPI setup for the API. This is separated from Program.cs to keep startup configuration cleaner.
public static class OpenApiConfiguration
{
    public static IServiceCollection AddOpenApiConfiguration(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<OpenApiDocumentTransformer>();
        });

        return services;
    }
}
