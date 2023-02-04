using Shop.Entities.ShopEntities;
using Shop.Infrastructures;
using Shop.Services.ShopServices.ProductPropertyServices.Contracts.Dtos;
using Shop.Services.ShopServices.ProductServices.Contracts.Dtos;

namespace Shop.Services.ShopServices.ProductPropertyServices.Contracts
{
    public interface ProductPropertyRepository : Repository
    {
        void Add(ProductProperty productProperty);
        Task<bool> IsKeyDuplicated(string productId, string key);
        Task<IPageResult<GetAllProductPropertiesDto>> GetAll(
            string productId, 
            ISort<GetAllProductPropertiesDto>? sortExpression, 
            Pagination? pagination, 
            string? search);
        Task<ProductProperty> Find(string id);
        void Delete(ProductProperty productProperty);
    }
}
