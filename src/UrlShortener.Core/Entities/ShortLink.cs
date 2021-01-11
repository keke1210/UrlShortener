using System;

namespace UrlShortener.Core.Entities
{
    public class ShortLink
    {
        public Guid Id { get; set; }
        public string Prefix { get; set; }
        public string Token { get; set; }
        public Uri OriginalUrl { get; set; }
        public Uri Value { get; set; }
        public int Hits { get; set; }
        public DateTime DateCreated { get; set; }
        /// <summary>
        /// Ticks
        /// </summary>
        public ulong Duration { get; set; }
    }
}
