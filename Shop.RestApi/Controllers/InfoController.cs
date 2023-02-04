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
            var informations = "This Is Some Information About This API. \n" +
                               "we have many useful apis for creating many powerfull websites";
            return informations;
        }
    }
}