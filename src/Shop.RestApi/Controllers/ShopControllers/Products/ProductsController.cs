using Microsoft.AspNetCore.Mvc;
using Shop.Services.ShopServices.ProductServices.Contracts;
using Shop.Services.ShopServices.ProductServices.Contracts.Dtos;

namespace Shop.RestApi.Controllers.ShopControllers.Products
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController
    {
        private readonly ProductService _service;

        public ProductsController(
            ProductService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<string> Add(AddProductDto dto)
        {
            return await _service.Add(dto);
        }
    }
}
