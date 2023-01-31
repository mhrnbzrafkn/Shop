using Shop.Infrastructures;
using Shop.Persistence.EF;
using Shop.Persistence.EF.ShopRepositories.ProductProperties;
using Shop.Persistence.EF.ShopRepositories.Products;
using Shop.Services.ShopServices.ProductPropertyServices;
using Shop.Services.ShopServices.ProductPropertyServices.Contracts;
using Shop.Services.ShopServices.ProductServices;
using Shop.Services.ShopServices.ProductServices.Contracts;

namespace Shop.RestApi;
public static class ServiceConfig
{
    public static void RegisterServices(WebApplicationBuilder webBuilder)
    {
        webBuilder.Services.AddScoped<EFDataContext>();
        webBuilder.Services.AddTransient<UnitOfWork, EFUnitOfWork>();
        webBuilder.Services.AddTransient<UriSortParser>();

        // register product service and repository
        webBuilder.Services.AddTransient<ProductService, ProductAppService>();
        webBuilder.Services.AddTransient<ProductRepository, EFProductRepository>();

        // register product property service and repository
        webBuilder.Services.AddTransient<ProductPropertyService, ProductPropertyAppService>();
        webBuilder.Services.AddTransient<ProductPropertyRepository, EFProductPropertyRepository>();
    }
}