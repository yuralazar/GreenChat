using System;
using System.Threading.Tasks;
using CreenChat.WebAPI.Extensions;
using CreenChat.WebAPI.Services;
using CreenChat.WebAPI.WebSocketManagement;
using GreenChat.BLL;
using GreenChat.BLL.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CreenChat.WebAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                //builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddGreenChatServices(Configuration);
            services.AddAutoMapper();            

            services.AddMvc();
            services.AddWebSocketManager();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors(builder =>
                builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .AllowAnyOrigin());

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
           
            app.UseStaticFiles();
            app.UseIdentity();

            app.UseWebSockets();

            if (Convert.ToBoolean(Configuration["randomDataLoad:load"]))
            {
                var dataLoader = serviceProvider.GetService<TestDataLoader>();
                dataLoader.SetConnectionString(Configuration.GetConnectionString("Local"));
                dataLoader.SetCounts(Convert.ToInt32(Configuration["randomDataLoad:users"]),
                                     Convert.ToInt32(Configuration["randomDataLoad:friends"]),
                                     Convert.ToInt32(Configuration["randomDataLoad:messages"]));
                app.Run(context => DataLoadHandler(context, dataLoader));

            }

            app.MapWebSocketManager("/ws");

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Chat}/{action=Index}/{id?}");

            });

        }

        private async Task DataLoadHandler(HttpContext context, TestDataLoader dataLoader)
        {   
            dataLoader.LoadData();
        }
    }
}
