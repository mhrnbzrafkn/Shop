using Shop.Infrastructures;
using Shop.Services.ShopServices.ProductPropertyServices.Contracts.Dtos;
using Shop.Services.ShopServices.ProductServices.Contracts.Dtos;

namespace Shop.Services.ShopServices.ProductPropertyServices.Contracts
{
    public interface ProductPropertyService : Service
    {
        Task<string> Add(string productId, AddProductPropertyDto dto);
        Task<IPageResult<GetAllProductPropertiesDto>> GetAll(
            string productId, 
            ISort<GetAllProductPropertiesDto>? sortExpression, 
            Pagination? pagination, 
            string? search);
    }
}
