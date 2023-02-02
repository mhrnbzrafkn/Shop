using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using System.Net.Mime;
using System.Text.Json;

namespace Shop.RestApi.Configs
{
    internal class EnvelopeConfig : Configuration
    {
        public override void ConfigureApplication(IApplicationBuilder app)
        {
            var environment = app.ApplicationServices
                .GetRequiredService<IWebHostEnvironment>();
            var jsonOptions = app.ApplicationServices
                .GetService<IOptions<JsonOptions>>()?.Value.SerializerOptions;

            app.UseExceptionHandler(_ => _.Run(async context =>
            {
                var exception = context.Features
                .Get<IExceptionHandlerPathFeature>()?.Error;
                var errorType = exception?.GetType().Name
                .Replace("Exception", string.Empty);
                var errorDescription = environment
                .IsProduction() ? null : exception?.ToString();
                var result = new
                {
                    Error = errorType,
                    Description = errorDescription
                };

                context.Response.StatusCode =
                StatusCodes.Status500InternalServerError;
                context.Response.ContentType = MediaTypeNames.Application.Json;
                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(result, jsonOptions));
            }));

            if (environment.IsProduction()) app.UseHsts();
        }
    }
}
