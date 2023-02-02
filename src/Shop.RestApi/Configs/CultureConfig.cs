using System.Globalization;

namespace Shop.RestApi.Configs
{
    internal class CultureConfig : Configuration
    {
        public override void ConfigureApplication(IApplicationBuilder app)
        {
            var culture = CultureInfo.GetCultureInfo("en-US");
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }
    }
}
