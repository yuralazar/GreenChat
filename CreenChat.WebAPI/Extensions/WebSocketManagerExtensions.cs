using GreenChat.BLL.WebSockets;
using GreenChat.WebAPI.WebSocketManagement;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace GreenChat.WebAPI.Extensions
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
