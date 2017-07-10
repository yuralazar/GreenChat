using GreenChat.DAL.Repositories;
using System;
using GreenChat.DAL.Data;
using GreenChat.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GreenChat.DAL
{
    class Program
    {
        static void Main(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            string connection = "Server=(localdb)\\MSSQLLocalDB;Database=ChatDB3;MultipleActiveResultSets=true;";
            optionsBuilder.UseSqlServer(connection, b => b.MigrationsAssembly("CreenChat.WebAPI"));
            //optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ChatDB1;MultipleActiveResultSets=true;");
            var dbcontext = new ApplicationDbContext(optionsBuilder.Options);
           // optionsBuilder.UseSqlServer(connection);
            //dbcontext.Database.BeginTransaction();

            dbcontext.Users.Add(
                new ApplicationUser
                {
                    Id = "1",
                    AccessFailedCount = 0,
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false
                });

            dbcontext.Users.Add(
                new ApplicationUser
                {
                    Id = "2",
                    AccessFailedCount = 0,
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false
                });

            dbcontext.PrivateMessages.Add(
                new PrivateMessage
                {
                    Content = "HI",
                    Date = DateTimeOffset.Now,
                    ReceiverID = "1",
                    SenderID = "2"
                });

            dbcontext.SaveChanges();
            Console.ReadKey();

            //dbcontext.Database.RollbackTransaction();

        }
    }
}