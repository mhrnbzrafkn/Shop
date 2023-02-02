using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Shop.Infrastructures;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Shop.RestApi.Configs;

internal class SwaggerDocConfig : Configuration
{
    private const string ModuleTitle = "Shop";
    private const string CompanyName = "Mhrn";
    private const string CompanySiteUrl = "http://Mhrn.com";
    private const string CompanyEmail = "admin@Mhrn.com";
    private const string CompanyTermsUrl = "http://Mhrn.com/terms";
    private const string CompanyLicenseUrl = "http://Mhrn.com/license";

    public override void ConfigureServiceContainer(IServiceCollection container)
    {
        container.AddRouting();

        container.AddSwaggerGen(options =>
        {
            var versionProvider = container.BuildServiceProvider()
            .GetRequiredService<IApiVersionDescriptionProvider>();
            versionProvider.ApiVersionDescriptions.ForEach(description =>
            {
                options.SwaggerDoc(
                    description.GroupName,
                    CreateApiInfo(description));
            });

            // AddSecurity(options);

            var xmlDocFilepath =
            GetXmlDocumentationFilePath(typeof(Application).Assembly);
            if (File.Exists(xmlDocFilepath))
                options.IncludeXmlComments(xmlDocFilepath);
        });
    }

    public override void ConfigureApplication(IApplicationBuilder app)
    {
        var provider = app.ApplicationServices
            .GetRequiredService<IApiVersionDescriptionProvider>();
        var environment = app.ApplicationServices
            .GetRequiredService<IHostEnvironment>();

        if (environment.IsDevelopment())
        {
            app.UseSwagger(options =>
            {
                options.RouteTemplate = "docs/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "docs";

                provider.ApiVersionDescriptions.ForEach(_ =>
                    options.SwaggerEndpoint(
                        $"{_.GroupName}/swagger.json",
                        _.GroupName.ToUpperInvariant()));

                options.DocumentTitle = $"{ModuleTitle} API Documentation";
            });
        }
    }

    private OpenApiInfo CreateApiInfo(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = $"{ModuleTitle} API {description.ApiVersion}",
            Version = description.ApiVersion.ToString(),
            Description = $"{ModuleTitle} API Documentation.",
            Contact = new OpenApiContact
            {
                Name = CompanyName,
                Url = new Uri(CompanySiteUrl),
                Email = CompanyEmail
            },
            TermsOfService = new Uri(CompanyTermsUrl),
            License = new OpenApiLicense
            {
                Name = CompanyName,
                Url = new Uri(CompanyLicenseUrl)
            }
        };

        if (description.IsDeprecated)
            info.Description += " This API version has been deprecated.";

        return info;
    }

    private string GetXmlDocumentationFilePath(Assembly assembly)
    {
        var basePath = Path.GetDirectoryName(assembly.Location);
        var fileName = $"{assembly.GetName().Name}.xml";
        return Path.Combine(basePath, fileName);
    }

    private static void AddSecurity(SwaggerGenOptions options)
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Example: \"Bearer token\""
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
}

internal class SwaggerDefaultValues : IOperationFilter
{
    public void Apply(
        OpenApiOperation operation,
        OperationFilterContext context)
    {
        if (operation.Parameters == null) return;

        foreach (var parameter in operation.Parameters)
        {
            var description = context.ApiDescription.ParameterDescriptions
                .First(p => p.Name == parameter.Name);
            var routeInfo = description.RouteInfo;

            if (parameter.Description == null)
                parameter.Description = description.ModelMetadata?.Description;

            if (routeInfo == null) continue;

            parameter.Required |= !routeInfo.IsOptional;
        }
    }
}