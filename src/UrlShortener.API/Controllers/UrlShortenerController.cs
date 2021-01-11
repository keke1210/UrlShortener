using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using UrlShortener.Core.DTOs;
using UrlShortener.Core.Interfaces;

namespace UrlShortener.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UrlShortenerController : ControllerBase
    {
        private readonly ILogger<UrlShortenerController> _logger;
        private readonly IUrlShortenerService _urlShorteningService;

        public UrlShortenerController(ILogger<UrlShortenerController> logger, IUrlShortenerService urlShorteningService)
        {
            _logger = logger;
            _urlShorteningService = urlShorteningService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] UrlRequest urlRequest)
        {
            try
            {
                var baseUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
                var shortLink = _urlShorteningService.CreateShortUrl(urlRequest, baseUrl);
                return Ok(shortLink);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult RedirectToLong([FromBody] UrlRequest urlRequest)
        {



            return Ok();
        }
    }
}
