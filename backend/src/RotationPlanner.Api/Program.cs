using RotationPlanner.Api.OpenApi;
using RotationPlanner.Api.Security;
using RotationPlanner.Api.Security.ApiKey;
using RotationPlanner.Api.Security.Cors;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApiConfiguration();
builder.Services.AddSecurityConfigurations(builder.Configuration);


var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference("/docs");

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(CorsConfiguration.FrontendPolicy);

app.UseMiddleware<ApiKeyMiddleware>();

app.MapControllers();

app.Run();
