using System;
using System.Collections.Generic;
using GreenChat.DAL.Data;
using GreenChat.DAL.Interfaces;
using GreenChat.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GreenChat.BLL
{
    public class TestDataLoader
    {
        private readonly DbContextOptionsBuilder<ApplicationDbContext> _optionsBuilder 
                            = new DbContextOptionsBuilder<ApplicationDbContext>();        
        private readonly UserManager<ApplicationUser> _manager;
        private readonly Random _rand;
        private readonly List<ApplicationUser> _users = new List<ApplicationUser>();
        private readonly Dictionary<string, List<string>> _friends 
                            = new Dictionary<string, List<string>>();
        private DateTimeOffset _startDate = new DateTimeOffset(2004, 1, 1,1,1,1, TimeSpan.Zero);
        private readonly ILogger _logger;
        private int _countUsers;
        private int _countFriends;
        private int _countMessages;

        public TestDataLoader(UserManager<ApplicationUser> manager,
                              ILoggerFactory loggerFactory)
        {
            _manager = manager;
            _logger = loggerFactory.CreateLogger<TestDataLoader>();            
            _rand = new Random();
        }

        public void LoadData()
        {
            if (_countUsers == 0)
            {
                _logger.LogWarning("NULL COUNTS TO LOAD");
                return;
            }

            RegisterUsers(_countUsers);
            AddFriends(_countFriends);
            AddPrivateMessages(_countMessages);
            _logger.LogWarning("ENDED RANDOM DATA LOAD");
        }

        private void AddPrivateMessages(int count)
        {
            _logger.LogWarning("Adding private messages");
            var j = 0;
            foreach (var friedPair in _friends)
            {
                foreach (var fr in friedPair.Value)
                {
                    for (int i = 0; i < count; i++)
                    {
                        CreateNewMessage(friedPair.Key , fr , _startDate.AddSeconds(i));
                        j++;
                        if (j % (count*10) == 0)
                            _logger.LogWarning("Messages " + j);
                    }
                }  
            }
        }

        private void CreateNewMessage(string user1Id, string user2Id, DateTimeOffset date)
        {            
            var mess1 = new PrivateMessage
            {
                Content = GetRandomText(6, 20),
                Date = date,
                SenderID = user1Id,
                ReceiverID = user2Id
            };
            var mess2 = new PrivateMessage
            {
                Content = GetRandomText(6, 20),
                Date = date,
                SenderID = user2Id,
                ReceiverID = user1Id
            };

            using (var context = new ApplicationDbContext(_optionsBuilder.Options))
            {
                context.PrivateMessages.Add(mess1);              
                context.PrivateMessages.Add(mess2);
                context.SaveChanges();
            }                        
        }

        private void AddFriends(int count)
        {
            _logger.LogWarning("Adding Friends");
            var usersCount = _users.Count;

            var j = 0;
            foreach (var user in _users)
            {
                for (var i = 0; i < count; i++)
                {
                    var index = GetRandomNumb(0, usersCount-1);
                    var userId = user.Id;
                    var id = _users[index].Id;
                    if (userId != id && !InFriendship(userId, id))
                    {
                        AddFriendship(userId, id);
                        CreateFriendship(user, _users[index]);
                        j++;
                        if (j % (count * 10) == 0)
                            _logger.LogWarning("Friends " + j);                                                
                    }
                }               
            }
        }

        private void AddFriendship(string user1Id, string user2Id)
        {
            if (_friends.ContainsKey(user1Id))
            {               
                _friends[user1Id].Add(user2Id);
            }
            else
            {
                var list = new List<string> {user2Id};
                _friends.Add(user1Id, list);
            }

            if (_friends.ContainsKey(user2Id))
            {
                _friends[user2Id].Add(user1Id);
            }
            else
            {
                var list = new List<string> { user1Id };
                _friends.Add(user2Id, list);
            }            
        }

        private bool InFriendship(string userId, string id)
        {
            return _friends.ContainsKey(userId) && _friends[userId].Contains(id);
        }

        private void CreateFriendship(ApplicationUser user1, ApplicationUser user2)
        {
            using (var context = new ApplicationDbContext(_optionsBuilder.Options))
            {
                context.Friends.Add(new Friend {Friend1ID = user1.Id, Friend2ID = user2.Id});
                context.Friends.Add(new Friend {Friend1ID = user2.Id, Friend2ID = user1.Id});
                context.SaveChanges();
            }
        }

        private void RegisterUsers(int count)
        {
            _logger.LogWarning("Registring users");
            for (int i = 0; i < count; i++)
            {
                RegisterUser();
                if (i%(count/10) == 0)
                    _logger.LogWarning("Users " + i);
            }
        }

        private void RegisterUser()
        {

            var user = CreateUser();
            var res = _manager.CreateAsync(user, GetPass());
            if (res.Result == IdentityResult.Success)
                _users.Add(user);
            
        }

        private ApplicationUser CreateUser()
        {
            var name = GetUserName();
            var user = new ApplicationUser
            {
                Email = name,
                UserName = name
            };

            return user;
        }

        private string GetUserName()
        {
            var userName = GetRandomText(6,8);

            userName += "@gmail.com";
            return userName;
        }

        private string GetRandomText(int countMin, int countMax)
        {
            var text = "";            
            var countLettersLogin = GetRandomNumb(countMin, countMax);
            for (var i = 0; i < countLettersLogin; i++)
            {
                text += GetRandomSymbol();              
            }
            text += GetRandomNumb(1, 1000);
            
            return text;
        }

        private string GetPass()
        {
            return "test2017";
        }

        private string GetRandomSymbol()
        {
            return Convert.ToChar(GetRandomNumb(97, 122)).ToString();
        }

        private int GetRandomNumb(int start, int end)
        {
            return _rand.Next(start, end);
        }

        public void SetConnectionString(string connectionString)
        {            
            _optionsBuilder.UseSqlServer(connectionString);
        }

        public void SetCounts(int users, int friends, int messages)
        {
            _countUsers = users;
            _countFriends = friends;
            _countMessages = messages;
        }
    }
}