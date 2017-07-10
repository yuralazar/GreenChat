using System;
using System.Collections.Generic;
using System.Text;
using GreenChat.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GreenChat.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Friend>().HasAlternateKey(t => new { t.Friend1ID, t.Friend2ID });

            builder.Entity<Friend>()
                .HasOne(u => u.Friend1)
                .WithMany(f => f.Friends1)
                .HasForeignKey(u => u.Friend1ID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Friend>()
                .HasOne(u => u.Friend2)
                .WithMany(f => f.Friends2)
                .HasForeignKey(u => u.Friend2ID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PrivateMessage>()
                .HasOne(u => u.Receiver)
                .WithMany(f => f.Receivers)
                .HasForeignKey(u => u.ReceiverID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PrivateMessage>()
                .HasOne(u => u.Sender)
                .WithMany(f => f.Senders)
                .HasForeignKey(u => u.SenderID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UnreadPrivateMessage>()
                .HasOne(u => u.Receiver)
                .WithMany(f => f.UnreadReceivers)
                .HasForeignKey(u => u.ReceiverID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UnreadPrivateMessage>()
                .HasOne(u => u.Sender)
                .WithMany(f => f.UnreadSenders)
                .HasForeignKey(u => u.SenderID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ChatMessage>()
                .HasOne(m => m.ChatRoomUser)
                .WithMany(user => user.NavChatMessages)
                .HasForeignKey(message => message.ChatRoomUserID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ChatMessage>()
                .HasOne(m => m.ChatRoom)
                .WithMany(room => room.NavChatMessages)
                .HasForeignKey(message => message.ChatRoomID)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UnreadChatMessage>()
                .HasOne(m => m.ChatRoomUserFrom)
                .WithMany(user => user.NavUnreadChatMessages1)
                .HasForeignKey(message => message.ChatRoomUserFromID)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UnreadChatMessage>()
                .HasOne(m => m.ChatRoomUserTo)
                .WithMany(user => user.NavUnreadChatMessages2)
                .HasForeignKey(message => message.ChatRoomUserToID)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UnreadChatMessage>()
                .HasOne(m => m.ChatRoom)
                .WithMany(room => room.NavUnreadChatMessages)
                .HasForeignKey(message => message.ChatRoomID)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ChatRoomUser>().HasAlternateKey(t => new { t.UserID, t.ChatRoomID });

            builder.Entity<ChatRoomUser>()
                .HasOne(u => u.User)
                .WithMany(f => f.ChatRoomUsers)
                .HasForeignKey(u => u.UserID)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ChatRoomUser>()
                .HasOne(u => u.ChatRoom)
                .WithMany(f => f.NavChatRoomUsers)
                .HasForeignKey(u => u.ChatRoomID)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<PrivateMessage> PrivateMessages { get; set; }

        public DbSet<PrivateMessageStatus> PrivateMessageStatuses { get; set; }

        public DbSet<UnreadPrivateMessage> UnreadPrivateMessages { get; set; }

        public DbSet<Friend> Friends { get; set; }

        public DbSet<ChatMessage> ChatMessages { get; set; }

        public DbSet<UnreadChatMessage> UnreadChatMessages { get; set; }

        public DbSet<ChatMessageStatus> ChatMessageStatuses { get; set; }

        public DbSet<ChatRoom> ChatRooms { get; set; }

        public DbSet<ChatRoomUser> ChatRoomUsers { get; set; }

    }

    public class ApplicationDbContextFactory : IDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext Create(DbContextFactoryOptions options)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            //optionsBuilder.UseSqlServer("Server=192.168.0.24;Database=TestChatDb2;User Id=test;Password=GreenChatTest;MultipleActiveResultSets=true;");            
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ChatDB3");
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
