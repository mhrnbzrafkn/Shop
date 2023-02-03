using Microsoft.AspNetCore.Mvc;

namespace Shop.RestApi.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/informations")]
    public class InfoController : ControllerBase
    {
        [HttpGet]
        public string Informations()
        {
            return "This Is Some Information About This API.";
        }
    }
}