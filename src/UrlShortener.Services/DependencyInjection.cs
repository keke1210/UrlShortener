using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Core.Interfaces;

namespace UrlShortener.Services
{
    public static class DependencyInjection
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUrlShortenerService, UrlShortenerService>();
        }
    }
}
