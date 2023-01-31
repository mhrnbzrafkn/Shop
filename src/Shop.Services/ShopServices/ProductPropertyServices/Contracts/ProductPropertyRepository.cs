using Shop.Entities.ShopEntities;
using Shop.Infrastructures;

namespace Shop.Services.ShopServices.ProductPropertyServices.Contracts
{
    public interface ProductPropertyRepository : Repository
    {
        void Add(ProductProperty productProperty);
        Task<bool> IsKeyDuplicated(string productId, string key);
    }
}
