using HealthChecks.UI.Client;
using LowCodeHub.Logging.Extensions;
using LowCodeHub.Migration.SqlServer.Extensions;
using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.Permissions;
using LowCodeHub.QueryableExtensions.Extensions;
using LowCodeHub.QueryableExtensions.Transactions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TerraLink.API.Exceptions;
using TerraLink.API.Services;
using TerraLink.Domain.Interfaces;
using TerraLink.Domain.Interfaces.Services;
using TerraLink.Domain.Persistence;
using TerraLink.Domain.Services;
using TerraLink.Migration;
using TerraLink.UseCase.Asset;
using TerraLink.UseCase.Dashboard;
using TerraLink.UseCase.Document;
using TerraLink.UseCase.Finance;
using TerraLink.UseCase.IdentityShield;
using TerraLink.UseCase.Lookup;
using TerraLink.UseCase.Report;
using TerraLink.UseCase.Sales;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict;
});

string connectionString = Environment.GetEnvironmentVariable("DbConnectionString")
    ?? throw new InvalidOperationException("DbConnectionString environment variable is not set");

builder.Services.AddDbContext<TerraLinkDbContext>(options =>
{
    options.UseSqlServer(connectionString, sql =>
    {
        sql.EnableRetryOnFailure(
            maxRetryCount: 12,
            maxRetryDelay: TimeSpan.FromSeconds(15),
            errorNumbersToAdd: null);
        sql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
    }).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddHealthChecks().AddDbContextCheck<TerraLinkDbContext>();
builder.Services.AddQueryableRepositories<TerraLinkDbContext>();
builder.Services.AddTransactionManager<TerraLinkDbContext>();

builder.Services.AddAuthorization();

// Cross-cutting infrastructure (formerly TerraLink.Application).
builder.Services.AddDistributedMemoryCache();
builder.Services.AddScoped<ICodeGeneratorService, CodeGeneratorService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IPermissionService>(sp => sp.GetRequiredService<ICurrentUserService>());
builder.Services.AddPermissionAuthorization();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// UseCase composition.
builder.Services.AddAssetUseCase();
builder.Services.AddSalesUseCase();
builder.Services.AddDocumentUseCase();
builder.Services.AddLookupUseCase();
builder.Services.AddIdentityShieldUseCase(builder.Configuration);
builder.Services.AddFinanceUseCase();
builder.Services.AddReportUseCase();
builder.Services.AddDashboardUseCase();

builder.Services.AddLogging();

builder.Services.AddMigration(a =>
{
    a.ConnectionString = connectionString;
    a.Directories =
    [
        "TerraLink.Migration.Scripts.SqlServer.Tables",
        "TerraLink.Migration.Scripts.SqlServer.Data"
    ];
    Console.WriteLine($"Migration directories: {string.Join(", ", a.Directories)}");
});

if (Environment.GetEnvironmentVariable("SKIP_SERILOG") != "true")
{
    builder.AddEnhancedSerilogLogging();
}

var app = builder.Build();

app.UseCors();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

await app.RunDatabaseMigrationAsync<IMigrationScanner>();

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapModules(typeof(IAssetScanner).Assembly,
               typeof(ISalesScanner).Assembly,
               typeof(IAttachmentScanner).Assembly,
               typeof(ILookupScanner).Assembly,
               typeof(IIdentityShieldScanner).Assembly,
               typeof(IFinanceScanner).Assembly,
               typeof(IExportExcelScanner).Assembly,
               typeof(IDashboardScanner).Assembly);

await app.RunAsync();
