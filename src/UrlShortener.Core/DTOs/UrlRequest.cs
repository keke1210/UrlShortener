namespace UrlShortener.Core.DTOs
{
    public class UrlRequest
    {
        public string Url { get; set; }
        public string Prefix { get; set; }
        public ulong Duration { get; set; }
    }
}
