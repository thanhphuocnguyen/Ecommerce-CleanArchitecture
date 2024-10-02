using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Carter;
using Ecommerce.Domain.DependencyInjection;
using Ecommerce.Infrastructure.BackgroundJobs;
using Ecommerce.Infrastructure.DependencyInjection;
using Ecommerce.Infrastructure.Persistence.Initialization;
using Ecommerce.WebAPI.Exceptions;
using Ecommerce.WebAPI.Middleware;
using Ecommerce.WebAPI.OpenApi;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();
builder.Services.AddProblemDetails();
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1);
    opt.ApiVersionReader = new UrlSegmentApiVersionReader();

    /* FIXME:
    ApiVersionReader.Combine(
    new UrlSegmentApiVersionReader(),
    new HeaderApiVersionReader("X-ApiVersion"));
    */
})
.AddApiExplorer(opt =>
{
    opt.GroupNameFormat = "'v'V";
    opt.SubstituteApiVersionInUrl = true;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddExceptionHandler<ExceptionHandler>();

builder.Host.UseSerilog((ctx, config) =>
{
    config.ReadFrom.Configuration(ctx.Configuration);
});

builder.Services.AddCarter();

var app = builder.Build();

using var scope = app.Services.CreateScope();
await JobSchedulersSetup.StartOutboxScheduler(scope.ServiceProvider);

app
    .UseExceptionHandler()
    .UseSerilogRequestLogging()
    .UseInfrastructure();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        IReadOnlyList<ApiVersionDescription> description = app.DescribeApiVersions();
        foreach (ApiVersionDescription apiVersionDescription in description)
        {
            opt.SwaggerEndpoint(
                $"/swagger/{apiVersionDescription.GroupName}/swagger.json",
                apiVersionDescription.GroupName.ToUpperInvariant());
        }
    });
    await scope.ServiceProvider.GetRequiredService<AppDbInitializer>().InitializeAsync();
}

var apiVersion = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1))
    .HasApiVersion(new ApiVersion(2))
    .ReportApiVersions()
    .Build();

var versionedGroup = app
    .MapGroup("/api/v{apiVersion:apiVersion}")
    .WithApiVersionSet(apiVersion);

versionedGroup.MapCarter();

app.UseHttpsRedirection();
app.UseMiddleware<RequestLogContextMiddleware>();

await app.RunAsync();