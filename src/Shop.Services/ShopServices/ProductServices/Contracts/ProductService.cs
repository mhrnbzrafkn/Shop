using Shop.Infrastructures;
using Shop.Services.ShopServices.ProductServices.Contracts.Dtos;

namespace Shop.Services.ShopServices.ProductServices.Contracts
{
    public interface ProductService : Service
    {
        Task<string> Add(AddProductDto dto);
        Task<IPageResult<GetAllProductsDto>> GetAll(
            ISort<GetAllProductsDto>? sortExpression,
            Pagination? pagination, 
            string? search);
        Task Delete(string id);
        Task<GetProductDto?> Get(string id);
    }
}
