using Microsoft.AspNetCore.Mvc;

namespace Shop.RestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfoController : ControllerBase
    {
        [HttpGet]
        public string Informations()
        {
            return "This Is Some Information About This API.";
        }
    }
}