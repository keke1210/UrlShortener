using System;
using System.Threading.Tasks;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Interfaces;

namespace UrlShortener.Services
{
    public class UrlShortenerService : IUrlShortenerService
    {
        private readonly IUrlShortenerRepository _urlShortenerRepository;

        public UrlShortenerService(IUrlShortenerRepository urlShortenerRepository)
        {
            _urlShortenerRepository = urlShortenerRepository;
        }

        public Task<string> CreateShortUrl(string uri)
        {
            _urlShortenerRepository.CreateUrl(new URL
            {
                Id = Guid.NewGuid(),
                LongLink = "https://video.gjirafa.com/rtv-dukagjini-live?utm_source=gjvideo&utm_medium=HomepageSection&utm_campaign=Live+TV&utm_term=position_3",
                ShortLink = "https://gj.al/1LZBa3"
            }); 

            return null;
        }
    }
}
