using AutoMapper;
using GreenChat.Client_WEB.Models;
using Microsoft.Extensions.DependencyInjection;

namespace GreenChat.Client_WEB.Extensions
{
    public static class AutoMapperServiceCollectionExtensions
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());                

            });
            services.AddSingleton(sp => autoMapperConfig.CreateMapper());

            return services;
        }
    }
}
