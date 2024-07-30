using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Ecommerce.WebAPI;

public class ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider apiVersionProvider) : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _apiVersionProvider = apiVersionProvider;

    public void Configure(SwaggerGenOptions options)
    {
        foreach (ApiVersionDescription description in _apiVersionProvider.ApiVersionDescriptions)
        {
            var openApi = new OpenApiInfo
            {
                Title = $"Ecommerce API v{description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = "A simple example ASP.NET Core Web API",
                Contact = new OpenApiContact
                {
                    Name = "Ecommerce",
                    Email = "thanhphuocb7@gmail.com",
                },
            };

            options.SwaggerDoc(description.GroupName, openApi);
        }
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }
}