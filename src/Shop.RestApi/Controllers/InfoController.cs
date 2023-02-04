using Microsoft.AspNetCore.Mvc;

namespace Shop.RestApi.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/infos")]
    public class InfoController : ControllerBase
    {
        [HttpGet("introduction")]
        public GetIntroductionDto Informations()
        {
            var introduction = new GetIntroductionDto(
                "Shop Api For Your Shop Projects",
                "This Is Some Information About This API.",
                "we have many useful apis for creating many powerfull websites.");

            return introduction;
        }

        [HttpGet("contact-us")]
        public GetContactUsDto ContactUs()
        {
            var ContactUs = new GetContactUsDto(
                "MHRN",
                "+9308894481",
                "Shiraz - Nasr blvd - Feyz st - Hamed Building",
                "https://www.mhrn.com",
                "mhrn.bzrafkn.dev@gmail.com");

            return ContactUs;
        }
    }

    public class GetIntroductionDto
    {
        public GetIntroductionDto(string title, string description, string content)
        {
            Title = title;
            Description = description;
            Content = content;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
    }

    public class GetContactUsDto
    {
        public GetContactUsDto(
            string companyName,
            string phoneNumber,
            string address,
            string webSite,
            string email)
        {
            CompanyName = companyName;
            PhoneNumber = phoneNumber;
            Address = address;
            WebSite = webSite;
            Email = email;
        }

        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string WebSite { get; set; }
        public string Email { get; set; }
    }
}