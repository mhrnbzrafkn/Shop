namespace Shop.RestApi.Configs;

internal class OptionsConfig : Configuration
{
    public override void ConfigureSettings(IConfigurationBuilder config)
    {
        config.Sources.Clear();
        config.SetBasePath(BaseDirectory)
            .AddConfiguration(AppSettings);
    }
}