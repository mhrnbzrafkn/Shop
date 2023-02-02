using Autofac;
using Shop.Infrastructures;
using Shop.Persistence.EF;
using Shop.Persistence.EF.StorageRepositories;
using Shop.Services.ShopServices.ProductServices;

namespace Shop.RestApi.Configs;

internal class ServicesConfig : Configuration
{
    private string _dbConnectionString;

    public override void Initialized()
    {
        _dbConnectionString = AppSettings.GetValue<string>("ConnectionString");
    }

    public override void ConfigureServiceContainer(ContainerBuilder container)
    {
        container.RegisterAssemblyTypes(typeof(ProductAppService).Assembly)
            .AssignableTo<Service>()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        container.RegisterType<UtcDateTimeService>()
            .As<DateTimeService>()
            .SingleInstance();

        container.RegisterType<MagickImageService>()
            .As<ImageService>()
            .SingleInstance();

        container.RegisterType<UriSortParser>()
            .AsSelf()
            .InstancePerLifetimeScope();

        container.RegisterType<EFUnitOfWork>()
            .As<UnitOfWork>()
            .InstancePerLifetimeScope();

        container.RegisterType<EFDataContext>()
            .WithParameter("connectionString", _dbConnectionString)
            .AsSelf()
            .InstancePerLifetimeScope();

        container.RegisterAssemblyTypes(typeof(EFStorageRepository).Assembly)
            .AssignableTo<Repository>()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}