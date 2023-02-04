namespace Shop.RestApi.Configs
{
    internal class CorsConfig : Configuration
    {
        public override void ConfigureServiceContainer(IServiceCollection container)
        {
            container.AddCors();
        }

        public override void ConfigureApplication(IApplicationBuilder app)
        {
            app.UseCors(cors =>
            {
                cors.AllowAnyHeader()
                    //.WithExposedHeaders()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
            });
        }
    }
}
