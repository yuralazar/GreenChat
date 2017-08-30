using AutoMapper;
using GreenChat.BLL.DTO;
using GreenChat.WebAPI.Models;
using Microsoft.Extensions.DependencyInjection;

namespace GreenChat.WebAPI.Extensions
{
    public static class AutoMapperServiceCollectionExtensions
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new WebApiAutoMapperProfile());
                cfg.AddProfile(new BllAutoMapperProfile());

            });
            services.AddSingleton(sp => autoMapperConfig.CreateMapper());

            return services;
        }
    }
}
