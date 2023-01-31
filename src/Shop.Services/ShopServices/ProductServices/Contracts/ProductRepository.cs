using Shop.Entities.ShopEntities;
using Shop.Infrastructures;
using Shop.Services.ShopServices.ProductServices.Contracts.Dtos;

namespace Shop.Services.ShopServices.ProductServices.Contracts
{
    public interface ProductRepository : Repository
    {
        void Add(Product product);
        Task<bool> IsAnyExistByTitle(string title);
        Task<IPageResult<GetAllProductsDto>> GetAll(
            ISort<GetAllProductsDto>? sortExpression,
            Pagination? pagination,
            string? search);
        Task<Product> FindById(string id);
        void Delete(Product product);
        Task<GetProductDto> Get(string id);
    }
}
