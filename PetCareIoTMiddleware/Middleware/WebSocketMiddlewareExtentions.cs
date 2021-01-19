using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace PetCareIoTMiddleware.Middleware
{
    public static class WebSocketMiddlewareExtentions
    {
        /// <summary>
        /// IApplicationBuilder extension for Configure function in Startup.cs
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseWebSocketServer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<WebSocketServerMiddleware>();
        }

        /// <summary>
        /// IApplicationBuilder extension for ConfigureServices function in Startup.cs
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            services.AddSingleton<WebSocketServerConnectionManager>();
            return services;
        }
    }
}