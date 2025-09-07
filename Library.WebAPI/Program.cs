using System.Text.Json;
using System.Threading.RateLimiting;
using DefaultCorsPolicyNugetPackage;
using HealthChecks.UI.Client;
using Library.Application;
using Library.Infrastructure;
using Library.Infrastructure.Context;
using Library.WebAPI.Middlewares;
using Library.WebAPI.OpenApi;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.FeatureManagement;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// 🌍 Config
builder.Configuration.AddEnvironmentVariables();

// ➕ Services
builder.Services.AddResponseCompression(o => o.EnableForHttps = true);
builder.Services.AddDefaultCors();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddFeatureManagement(); // feature service

// 📦 OData + Controllers
builder.Services.AddControllers()
    .AddOData(opt => opt.EnableQueryFeatures())
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    }); // Feature Management için

// 🛡️ Rate Limit
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", config =>
    {
        config.PermitLimit = 100;
        config.QueueLimit = 100;
        config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        config.Window = TimeSpan.FromSeconds(1);
    });
});

// 🔍 Scalar + OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<FeatureGateTransformer>();
}); // Scalar için mutlaka lazım

// 🩺 HealthChecks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>("PostgreSQL", HealthStatus.Unhealthy);

builder.Services.AddHealthChecksUI(setup => { setup.AddHealthCheckEndpoint("Library API", "/health-check"); })
    .AddInMemoryStorage();

// 📦 APM middleware'i → bunu AddControllers’dan önce değil, sonra koyabilirsin
//builder.Services.AddElasticApm();

// ✅ Build
var app = builder.Build();

// 🧱 DB Migrate
//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//    dbContext.Database.Migrate();
//}

// 🧪 DevTools
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // ➜ /openapi.json
    app.MapScalarApiReference(); // ➜ /reference
}

//// ✅ Scalar çalışabilmesi için en az bir endpoint tanımı
//app.MapGet("/", () => "Scalar çalışıyor!")
//   .WithName("Root")
//   .WithOpenApi(); // ❗ bu satır olmadan Scalar interface boş olur


// ⚙️ Middleware
app.UseHttpsRedirection();
app.UseResponseCompression();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.UseExceptionHandler();

app.MapControllers()
    .RequireAuthorization()
    .RequireRateLimiting("fixed");

// 🩺 HealthCheck endpoints
app.MapHealthChecks("/health-check", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

app.MapHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";
    options.ApiPath = "/health-ui-api";
});

// 👤 İlk kullanıcıyı oluştur
ExtensionsMiddleware.CreateFirstUser(app);

// 🚀 Başlat
await app.RunAsync();