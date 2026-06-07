using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace RotationPlanner.Api.OpenApi;
// Provides an overall description of the API.
public sealed class OpenApiDocumentTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Components ??= new OpenApiComponents();

        document.Info.Title = "RotationPlanner API";
        document.Info.Version = "v1";
        document.Info.Description = """
        ## Introduction

        The RotationPlanner API provides endpoints for viewing, comparing and matching rotation schedules.
        
        The API is responsible for:
        - reading predefined rotation groups from configuration data
        - calculating production and free days for a selected group and date range
        - comparing rotation groups across a selected period
        - matching preferred days off against all available rotation groups

        The first version uses predefined rotation patterns stored in a JSON file.
        The API does not create custom schedules from scratch. Instead, it calculates results based on existing rotation rules.
        """;

        return Task.CompletedTask;
    }
}
