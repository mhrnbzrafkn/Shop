using Shop.Infrastructures;
using Shop.Services.ShopServices.ProductPropertyServices.Contracts.Dtos;

namespace Shop.Services.ShopServices.ProductPropertyServices.Contracts
{
    public interface ProductPropertyService : Service
    {
        Task<string> Add(string productId, AddProductPropertyDto dto);
    }
}
