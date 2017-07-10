using GreenChat.BLL.Services;
using GreenChat.BLL.WebSockets;
using GreenChat.DAL;
using GreenChat.DAL.Data;
using GreenChat.DAL.Interfaces;
using GreenChat.DAL.Models;
using GreenChat.DAL.Repositories;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GreenChat.BLL.Extensions
{
    public static class GreenChatServiceCollectionExtensions
    {
        public static IServiceCollection AddGreenChatServices(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("Local"))
                    ,ServiceLifetime.Transient);
            
            services.AddIdentity<ApplicationUser, IdentityRole>(
                    o =>
                    {
                        o.Password.RequireDigit = false;
                        o.Password.RequireUppercase = false;
                        o.Password.RequireLowercase = false;
                        o.Password.RequireNonAlphanumeric = false;
                        o.User.RequireUniqueEmail = true;
                        o.Cookies.ApplicationCookie.AutomaticChallenge = false;                        
                    }
                )
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<SendFormatFactory>();
            services.AddTransient<ChatHandler>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<UserManagerDto>();
            services.AddTransient<SignInManagerDto>();
            services.AddTransient<UserService>();
            services.AddTransient<IFriendRepository, FriendRepository>();
            services.AddTransient<IChatMessageRepository, ChatMessageRepository>();
            services.AddTransient<IUnreadChatMessageRepository, UnreadChatMessageRepository>();
            services.AddTransient<IPrivateMessageRepository, PrivateMessageRepository>();
            services.AddTransient<IUnreadPrivateMessageRepository, UnreadPrivateMessageRepository>();
            services.AddTransient<IPrivateMessageStatusesRepository, PrivateMessageStatusesRepository>();
            services.AddTransient<IChatMessageStatusesRepository, ChatMessageStatusesRepository>();
            services.AddTransient<IChatRoomUserRepository, ChatRoomUsersRepository>();
            services.AddTransient<IChatRoomRepository, ChatRoomRepository>();
            services.AddTransient<TestDataLoader>();

            return services;
        }
    }
}
