using Microsoft.AspNetCore.Mvc;
using Shop.Services.ShopServices.ProductPropertyServices.Contracts;

namespace Shop.RestApi.Controllers.Shop.ProductProperties
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/product-properties")]
    public class ProductPropertiesController : ControllerBase
    {
        private readonly ProductPropertyService _service;

        public ProductPropertiesController(
            ProductPropertyService productPropertyService)
        {
            _service = productPropertyService;
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _service.Delete(id);
        }
    }
}
