﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StockTracker.Services;

namespace StockTracker
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            
            //MVC services
            services.AddMvc();

            //TODO add services configuration for yahoo stock data service
            services.AddSingleton < IStockData, StockData > ();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            //dev http pipeline injections
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //standard wwwroot file serving
            app.UseFileServer();

            //call to my appbuilder extension to server node_modules
            app.UseNodeModules(env.ContentRootPath);

            //finally MVC middleware/routing
            app.UseMvcWithDefaultRoute();            
        }
    }
}
