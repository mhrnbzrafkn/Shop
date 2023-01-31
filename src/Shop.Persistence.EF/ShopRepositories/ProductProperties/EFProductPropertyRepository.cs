using Microsoft.EntityFrameworkCore;
using Shop.Entities.ShopEntities;
using Shop.Services.ShopServices.ProductPropertyServices.Contracts;

namespace Shop.Persistence.EF.ShopRepositories.ProductProperties
{
    public class EFProductPropertyRepository : ProductPropertyRepository
    {
        private readonly DbSet<ProductProperty> _productProperties;

        public EFProductPropertyRepository(EFDataContext context)
        {
            _productProperties = context.Set<ProductProperty>();
        }

        public void Add(ProductProperty productProperty)
        {
            _productProperties.Add(productProperty);
        }

        public async Task<bool> IsKeyDuplicated(
            string productId,
            string key)
        {
            return await _productProperties.AnyAsync(_ =>
            _.ProductId == productId && _.Key.Trim().ToLower() == key.Trim().ToLower());
        }
    }
}
