using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Ecommerce.WebAPI.OpenApi;

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

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }
}