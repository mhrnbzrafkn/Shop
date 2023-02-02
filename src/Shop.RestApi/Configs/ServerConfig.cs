namespace Shop.RestApi.Configs;

internal class ServerConfig : Configuration
{
    private const string UrlConfigKey = "url";

    public override void ConfigureServer(IWebHostBuilder host)
    {
        if (AppSettings.GetSection(UrlConfigKey)
            .Exists()) host.UseUrls(AppSettings.GetValue<string>(UrlConfigKey));

        host.UseKestrel();

        host.UseIISIntegration();
    }
}