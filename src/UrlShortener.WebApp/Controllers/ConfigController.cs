using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace UrlShortener.WebApp.Controllers
{

    public class ConfigController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly string apiBaseUrl;

        public ConfigController(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.apiBaseUrl = this.configuration.GetValue<string>("WebAPIBaseUrl");
        }

        [HttpGet("")]
        public IActionResult GetBaseApiUrl()
        {
            return Json(new { apiBaseUrl });
        }
    }
}
