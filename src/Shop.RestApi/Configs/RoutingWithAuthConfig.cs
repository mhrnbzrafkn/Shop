namespace Shop.RestApi.Configs;

internal class RoutingWithAuthConfig : Configuration
{
    public override void ConfigureServiceContainer(IServiceCollection container)
    {
        container.AddRouting();
        container.AddControllers();
        container.AddHttpContextAccessor();
    }

    public override void ConfigureApplication(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}