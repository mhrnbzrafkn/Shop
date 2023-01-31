using Shop.Infrastructures;
using Shop.Services.ShopServices.ProductServices.Contracts.Dtos;

namespace Shop.Services.ShopServices.ProductServices.Contracts
{
    public interface ProductService : Service
    {
        Task<string> Add(AddProductDto dto);
    }
}
