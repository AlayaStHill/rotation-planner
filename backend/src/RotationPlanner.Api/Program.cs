using RotationPlanner.Api.Configuration.OpenApi;
using RotationPlanner.Api.Configuration.Security;
using RotationPlanner.Api.Configuration.Security.ApiKey.Extensions;
using RotationPlanner.Api.Configuration.Security.Cors;
using RotationPlanner.Api.Logging;
using RotationPlanner.Api.Responses.ErrorHandling;
using RotationPlanner.Application.Extensions;
using RotationPlanner.Infrastructure.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApiConfiguration();
builder.Services.AddSecurityConfigurations(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);


var app = builder.Build();

app.UseCorrelationId();

app.UseGlobalExceptionHandling();

app.MapOpenApi();
app.MapScalarApiReference("/docs");

app.UseHttpsRedirection();

app.UseCors(CorsConfigurationExtensions.FrontendPolicy);

app.UseAuthorization();

app.UseApiKeyAuthentication();

app.MapControllers();

app.Run();
