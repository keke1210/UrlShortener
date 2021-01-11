using UrlShortener.Core.DTOs;

namespace UrlShortener.Core.Interfaces
{
    public interface IUrlShortenerService
    {
        UrlResponse CreateShortUrl(UrlRequest uri, string baseUrl);
        bool RedirectToLongUrl(UrlRequest uri);
    }
}
