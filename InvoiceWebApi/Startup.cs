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
            services.AddMvc();
        }

        // configure pipeline
        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseIISPlatformHandler();
            app.UseMvc();

            // app.Use(async (context, next) =>
            // {
            //     await context.Response.WriteAsync(">>>");
            //     await next();
            //     await context.Response.WriteAsync("<<<");
            //}
            //     );

            // app.Run(async (context) =>
            // {
            //     await context.Response.WriteAsync("Hello World!");
            // });
        }

        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
