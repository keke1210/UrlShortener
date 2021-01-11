using LiteDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Interfaces;
using UrlShortener.Core.Services;
using UrlShortener.Infrastructure.Repositories;

namespace UrlShortener.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UrlShortener.API", Version = "v1" });
            });

            services.AddSingleton<ILiteDatabase, LiteDatabase>((x) => new LiteDatabase(@"url-shortener.db"));
            services.AddScoped<IUrlShortenerRepository, UrlShortenerRepository>();
            services.AddScoped<IUrlShortenerService, UrlShortenerService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UrlShortener.API v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapGet("/keke", HandleShortenUrl);
                endpoints.MapFallback(HandleUrl);
            });
        }

        static Task WriteResponse(HttpContext context, int status, string response)
        {
            context.Response.StatusCode = status;
            return context.Response.WriteAsync(response);
        }

        static Task HandleShortenUrl(HttpContext context)
        {
            // Retrieve our dependencies
            var db = context.RequestServices.GetService<ILiteDatabase>();
            var service = context.RequestServices.GetService<IUrlShortenerService>();


            context.Request.Form.TryGetValue("urlRequest", out var formData);
            var requestedUrl = formData.ToString();

            // Test our URL
            if (!Uri.TryCreate(requestedUrl, UriKind.Absolute, out Uri result))
            {
                return WriteResponse(context, StatusCodes.Status400BadRequest, "Could not understand URL");
            }

            //var url = result.ToString();
            //var entry = collection.Find(p => p.OriginalUrl == url).FirstOrDefault();

            //if (entry is null)
            //{
            //    entry = new ShortLink
            //    {
            //        Url = url
            //    };
            //    collection.Insert(entry);
            //}

            //var urlChunk = entry.GetUrlChunk();
            //var responseUri = $"{context.Request.Scheme}://{context.Request.Host}/{urlChunk}";
            //context.Response.Redirect($"/#{responseUri}");
            return Task.CompletedTask;
        }

        static Task HandleUrl(HttpContext context)
        {
            // Default to home page if no matching url.
            var redirect = "/";

            var db = context.RequestServices.GetService<ILiteDatabase>();
            var collection = db.GetCollection<ShortLink>();
            var repo = context.RequestServices.GetService<IUrlShortenerRepository>();

            var path = context.Request.Path.ToUriComponent().Trim('/');
            var shortLink = repo.Get(x => x.Value.ToString() == path).FirstOrDefault();

            var entry = collection.Find(p => p.Id == shortLink.Id).SingleOrDefault();

            if (entry is not null)
            {
                redirect = entry.OriginalUrl.ToString();
            }

            context.Response.Redirect(redirect);
            return Task.CompletedTask;
        }
    }
}
