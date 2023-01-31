using Shop.Entities.ShopEntities;
using Shop.Infrastructures;

namespace Shop.Services.ShopServices.ProductServices.Contracts
{
    public interface ProductRepository : Repository
    {
        void Add(Product product);
        Task<bool> IsAnyExistByTitle(string title);
    }
}
