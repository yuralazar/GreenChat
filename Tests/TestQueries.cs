using System;
using GreenChat.DAL.Data;
using GreenChat.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Tests
{
    public class TestQueries
    {
        [Fact]
        public void GetSent_GivesRightRowsCount()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            const string connection = "Server=(localdb)\\MSSQLLocalDB;Database=ChatDB3;MultipleActiveResultSets=true;";
            optionsBuilder.UseSqlServer(connection);            
            var dbcontext = new ApplicationDbContext(optionsBuilder.Options);
            var privateMessageRepository = new PrivateMessageRepository(dbcontext, new LoggerFactory());
            var privateMessageStatusesRepository = new PrivateMessageRepository(dbcontext, new LoggerFactory());            

        }
    }
}
