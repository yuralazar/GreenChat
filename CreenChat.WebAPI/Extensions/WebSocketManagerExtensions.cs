using CreenChat.WebAPI.WebSocketManagement;
using GreenChat.BLL.WebSockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CreenChat.WebAPI.Extensions
{    
    public static class WebSocketManagerExtensions
    {
        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            services.AddSingleton<WebSocketConnectionManager>();
            services.AddTransient<WebSocketHandler>();            
            return services;
        }

        public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder app, 
                                                              PathString path)
        {
            return app.Map(path, (ap) => ap.UseMiddleware<WebSocketManagerMiddleware>());
        }
    }

}
