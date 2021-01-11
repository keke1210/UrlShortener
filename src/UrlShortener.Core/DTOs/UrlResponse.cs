using System;

namespace UrlShortener.Core.DTOs
{
    public class UrlResponse
    {
        public string Token { get; set; }
        public Uri OriginalUrl { get; set; }
        public Uri Value { get; set; }
    }
}
