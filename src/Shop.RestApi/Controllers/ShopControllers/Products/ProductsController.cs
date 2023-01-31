using Microsoft.AspNetCore.Mvc;
using Shop.Infrastructures;
using Shop.Services.ShopServices.ProductServices.Contracts;
using Shop.Services.ShopServices.ProductServices.Contracts.Dtos;

namespace Shop.RestApi.Controllers.ShopControllers.Products
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController
    {
        private readonly ProductService _service;
        private readonly UriSortParser _sortParser;

        public ProductsController(
            ProductService service,
            UriSortParser sortParser)
        {
            _service = service;
            _sortParser = sortParser;
        }

        [HttpPost]
        public async Task<string> Add(AddProductDto dto)
        {
            return await _service.Add(dto);
        }

        [HttpGet]
        public async Task<IPageResult<GetAllProductsDto>> GetAll(
            [FromQuery] string? sort,
            [FromQuery] int? limit,
            [FromQuery] int? offset,
            string? search)
        {
            var sortExpression = !string.IsNullOrEmpty(sort) ?
                _sortParser.Parse<GetAllProductsDto>(sort) :
                null;

            var pagination = limit.HasValue && offset.HasValue ?
                Pagination.Of(offset.Value + 1, limit.Value) :
                null;

            return await _service.GetAll(sortExpression, pagination, search);
        }

        [HttpDelete]
        public async Task Delete(string Id)
        {
            await _service.Delete(Id);
        }
    }
}
