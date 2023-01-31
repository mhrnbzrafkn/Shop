using Microsoft.AspNetCore.Mvc;
using Shop.Infrastructures;
using Shop.Services.ShopServices.ProductPropertyServices.Contracts;
using Shop.Services.ShopServices.ProductPropertyServices.Contracts.Dtos;
using Shop.Services.ShopServices.ProductServices.Contracts;
using Shop.Services.ShopServices.ProductServices.Contracts.Dtos;

namespace Shop.RestApi.Controllers.ShopControllers.Products
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _service;
        private readonly UriSortParser _sortParser;
        private readonly ProductPropertyService _productPropertyService;

        public ProductsController(
            ProductService service,
            UriSortParser sortParser,
            ProductPropertyService productPropertyService)
        {
            _service = service;
            _sortParser = sortParser;
            _productPropertyService = productPropertyService;
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

        [HttpGet("{id}")]
        public async Task<GetProductDto?> Get(string id)
        {
            return await _service.Get(id);
        }

        [HttpPut("{id}")]
        public async Task Edit(string id, EditProductDto dto)
        {
            await _service.Edit(id, dto);
        }

        [HttpPost("{id}/properties")]
        public async Task<string> AddProperty(string id, AddProductPropertyDto dto)
        {
            return await _productPropertyService.Add(id, dto);
        }

        [HttpGet("{id}/properties")]
        public async Task<IPageResult<GetAllProductPropertiesDto>> GetAllProperties(
            string id,
            [FromQuery] string? sort,
            [FromQuery] int? limit,
            [FromQuery] int? offset,
            string? search)
        {
            var sortExpression = !string.IsNullOrEmpty(sort) ?
                _sortParser.Parse<GetAllProductPropertiesDto>(sort) :
                null;

            var pagination = limit.HasValue && offset.HasValue ?
                Pagination.Of(offset.Value + 1, limit.Value) :
                null;

            return await _productPropertyService.GetAll(id, sortExpression, pagination, search);
        }
    }
}
