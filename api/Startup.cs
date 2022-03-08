using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace api
{
    public class Startup
    {
        private const string uiUrl = "http://localhost:4200";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IPersistence, FilePersistence>();
            services.AddScoped<IPersonService, PersonService>();

            string corsUrl = uiUrl;
            if (!string.IsNullOrEmpty(corsUrl))
            {
                services.AddCors(options =>
                {
                    options.AddPolicy("apiCorsPolicy",
                           builder => builder.WithOrigins(corsUrl.Split(",")).AllowAnyHeader().AllowAnyMethod());
                });
            }

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseExceptionMiddleware();

            app.UseAuthorization();

            string corsUrl = uiUrl;
            if (!string.IsNullOrEmpty(corsUrl))
            {
                app.UseCors("apiCorsPolicy");

                // Add header:
                app.Use((context, next) =>
                {
                    context.Response.Headers["Access-Control-Allow-Origin"] = corsUrl;
                    return next.Invoke();
                });
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
