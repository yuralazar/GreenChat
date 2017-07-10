using Microsoft.Extensions.Configuration;

namespace GreenChat.Client_WEB.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string GetWebApiServerString(this IConfiguration configuration)
        {
            var section = configuration?.GetSection("WebApiServer");
            return section?["url"];
        }
    }
}