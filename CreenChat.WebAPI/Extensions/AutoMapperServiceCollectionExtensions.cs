using AutoMapper;
using CreenChat.WebAPI.Models;
using GreenChat.BLL.DTO;
using Microsoft.Extensions.DependencyInjection;

namespace CreenChat.WebAPI.Extensions
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
