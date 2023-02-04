namespace Shop.RestApi.Configs;

internal class VersioningConfig : Configuration
{
    public override void ConfigureServiceContainer(IServiceCollection container)
    {
        container.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
        });

        container.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
    }
}