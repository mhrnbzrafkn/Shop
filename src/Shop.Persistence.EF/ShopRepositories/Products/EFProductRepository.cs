using Microsoft.EntityFrameworkCore;
using Shop.Entities.ShopEntities;
using Shop.Services.ShopServices.ProductServices.Contracts;

namespace Shop.Persistence.EF.ShopRepositories.Products
{
    public class EFProductRepository : ProductRepository
    {
        private readonly DbSet<Product> _products;

        public EFProductRepository(EFDataContext context)
        {
            _products = context.Set<Product>();
        }

        public void Add(Product product)
        {
            _products.Add(product);
        }

        public async Task<bool> IsAnyExistByTitle(string title)
        {
            return await _products.AnyAsync(_ => _.Title == title);
        }
    }
}
