using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceWebApi
{
    public class Startup
    {
        // configure dependency injection (no need for unity)
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
                {
                    options.AddPolicy("AllowAllOrigins",
                        builder =>
                            {
                                builder.AllowAnyOrigin()
                                       .AllowAnyMethod()
                                       .AllowAnyMethod();
                            });
                });
            services.AddMvc();
        }

        // configure pipeline
        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseIISPlatformHandler();
            app.UseCors("AllowAllOrigins");
            app.UseMvc();
        }

        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
