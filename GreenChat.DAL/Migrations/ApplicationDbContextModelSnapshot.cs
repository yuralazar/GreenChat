using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using GreenChat.DAL.Data;
using GreenChat.Data.Instances;

namespace GreenChat.DAL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GreenChat.DAL.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("GreenChat.DAL.Models.ChatMessage", b =>
                {
                    b.Property<int>("ChatMessageID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChatRoomID");

                    b.Property<int>("ChatRoomUserID");

                    b.Property<string>("Content");

                    b.Property<DateTimeOffset>("Date");

                    b.HasKey("ChatMessageID");

                    b.HasIndex("ChatRoomID");

                    b.HasIndex("ChatRoomUserID");

                    b.ToTable("ChatMessages");
                });

            modelBuilder.Entity("GreenChat.DAL.Models.ChatMessageStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChatMessageId");

                    b.Property<DateTime>("Date");

                    b.Property<int>("Status");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ChatMessageId");

                    b.HasIndex("UserId");

                    b.ToTable("ChatMessageStatuses");
                });

            modelBuilder.Entity("GreenChat.DAL.Models.ChatRoom", b =>
                {
                    b.Property<int>("ChatRoomID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("UserID");

                    b.HasKey("ChatRoomID");

                    b.HasIndex("UserID");

                    b.ToTable("ChatRooms");
                });

            modelBuilder.Entity("GreenChat.DAL.Models.ChatRoomUser", b =>
                {
                    b.Property<int>("ChatRoomUserID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChatRoomID");

                    b.Property<bool>("Confirmed");

                    b.Property<string>("UserID")
                        .IsRequired();

                    b.HasKey("ChatRoomUserID");

                    b.HasAlternateKey("UserID", "ChatRoomID");

                    b.HasIndex("ChatRoomID");

                    b.ToTable("ChatRoomUsers");
                });

            modelBuilder.Entity("GreenChat.DAL.Models.Friend", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Friend1ID")
                        .IsRequired();

                    b.Property<string>("Friend2ID")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasAlternateKey("Friend1ID", "Friend2ID");

                    b.HasIndex("Friend2ID");

                    b.ToTable("Friends");
                });

            modelBuilder.Entity("GreenChat.DAL.Models.PrivateMessage", b =>
                {
                    b.Property<int>("PrivateMessageID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<DateTimeOffset>("Date");

                    b.Property<string>("ReceiverID");

                    b.Property<string>("SenderID");

                    b.HasKey("PrivateMessageID");

                    b.HasIndex("ReceiverID");

                    b.HasIndex("SenderID");

                    b.ToTable("PrivateMessages");
                });

            modelBuilder.Entity("GreenChat.DAL.Models.PrivateMessageStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<int>("PrivateMessageId");

                    b.Property<int>("Status");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("PrivateMessageId");

                    b.HasIndex("UserId");

                    b.ToTable("PrivateMessageStatuses");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("GreenChat.DAL.Models.ChatMessage", b =>
                {
                    b.HasOne("GreenChat.DAL.Models.ChatRoom", "ChatRoom")
                        .WithMany("NavChatMessages")
                        .HasForeignKey("ChatRoomID");

                    b.HasOne("GreenChat.DAL.Models.ChatRoomUser", "ChatRoomUser")
                        .WithMany("NavChatMessages")
                        .HasForeignKey("ChatRoomUserID");
                });

            modelBuilder.Entity("GreenChat.DAL.Models.ChatMessageStatus", b =>
                {
                    b.HasOne("GreenChat.DAL.Models.ChatMessage", "ChatMessage")
                        .WithMany()
                        .HasForeignKey("ChatMessageId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GreenChat.DAL.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("GreenChat.DAL.Models.ChatRoom", b =>
                {
                    b.HasOne("GreenChat.DAL.Models.ApplicationUser", "User")
                        .WithMany("ChatRooms")
                        .HasForeignKey("UserID");
                });

            modelBuilder.Entity("GreenChat.DAL.Models.ChatRoomUser", b =>
                {
                    b.HasOne("GreenChat.DAL.Models.ChatRoom", "ChatRoom")
                        .WithMany("NavChatRoomUsers")
                        .HasForeignKey("ChatRoomID");

                    b.HasOne("GreenChat.DAL.Models.ApplicationUser", "User")
                        .WithMany("ChatRoomUsers")
                        .HasForeignKey("UserID");
                });

            modelBuilder.Entity("GreenChat.DAL.Models.Friend", b =>
                {
                    b.HasOne("GreenChat.DAL.Models.ApplicationUser", "Friend1")
                        .WithMany("Friends1")
                        .HasForeignKey("Friend1ID");

                    b.HasOne("GreenChat.DAL.Models.ApplicationUser", "Friend2")
                        .WithMany("Friends2")
                        .HasForeignKey("Friend2ID");
                });

            modelBuilder.Entity("GreenChat.DAL.Models.PrivateMessage", b =>
                {
                    b.HasOne("GreenChat.DAL.Models.ApplicationUser", "Receiver")
                        .WithMany("Receivers")
                        .HasForeignKey("ReceiverID");

                    b.HasOne("GreenChat.DAL.Models.ApplicationUser", "Sender")
                        .WithMany("Senders")
                        .HasForeignKey("SenderID");
                });

            modelBuilder.Entity("GreenChat.DAL.Models.PrivateMessageStatus", b =>
                {
                    b.HasOne("GreenChat.DAL.Models.PrivateMessage", "PrivateMessage")
                        .WithMany()
                        .HasForeignKey("PrivateMessageId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GreenChat.DAL.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("GreenChat.DAL.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("GreenChat.DAL.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GreenChat.DAL.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
