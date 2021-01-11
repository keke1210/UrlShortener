using System;
using System.Linq;
using UrlShortener.Core.DTOs;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Helpers;
using UrlShortener.Core.Interfaces;

namespace UrlShortener.Core.Services
{
    public class UrlShortenerService : IUrlShortenerService
    {
        private readonly IUrlShortenerRepository _repository;

        public UrlShortenerService(IUrlShortenerRepository repository)
        {
            _repository = repository;
        }

        public UrlResponse CreateShortUrl(UrlRequest urlRequest, string baseUrl)
        {
            var id = Guid.NewGuid();

            Uri uriResult;
            bool isValidUrl = Uri.TryCreate(urlRequest.Url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (!isValidUrl)
                throw new Exception("Uri is not valid!");

            var getUrl = _repository.Get(x => x.OriginalUrl == uriResult && x.Prefix == urlRequest.Prefix).FirstOrDefault();
            if (getUrl != null)
                return new UrlResponse
                {
                    OriginalUrl = getUrl.OriginalUrl,
                    Token = getUrl.Token,
                    Value = getUrl.Value
                }; 

            var shortLink = new ShortLink()
            {
                Id = id,
                Prefix = urlRequest.Prefix,
                Token = UrlHelper.GenerateToken(),
                OriginalUrl = uriResult,
                DateCreated = DateTime.UtcNow,
                Duration = urlRequest.Duration
            };

            if (string.IsNullOrEmpty(shortLink.Prefix))
                shortLink.Prefix = "tmp.al";

            //shortLink.Value = new Uri($"http://{shortLink.Prefix}/{shortLink.Token}");
            shortLink.Value = new Uri($"{baseUrl}keke/{shortLink.Token}");

            _repository.CreateUrl(shortLink);

            return new UrlResponse
            {
                OriginalUrl = shortLink.OriginalUrl,
                Token = shortLink.Token,
                Value = shortLink.Value
            };
        }

        public bool RedirectToLongUrl(UrlRequest uri)
        {
            throw new NotImplementedException();
        }
    }
}
