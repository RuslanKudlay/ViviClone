using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Entities.Order;
using Application.EntitiesModels.Entities.Chat;
using Microsoft.EntityFrameworkCore;
using System;
using Application.EntitiesModels.Entities.WishList;

namespace Application.DAL
{
    public interface IApplicationDbContext : IDisposable
    {
        DbSet<ApplicationUser> ApplicationUsers { get; set; }
        DbSet<ApplicationRole> ApplicationRoles { get; set; }
        DbSet<ApplicationUserRole> ApplicationUsersRoles { get; set; }
        DbSet<Brand> Brands { get; set; }
        DbSet<Blog> Blogs { get; set; }
        DbSet<Post> Posts { get; set; }
        DbSet<Announcement> Announcements { get; set; }
        DbSet<Chat> Chats { get; set; }
        DbSet<ChatMessage> ChatMessages { get; set; }
        DbSet<Promotion> Promotions { get; set; }
        DbSet<Image> Images { get; set; }
        DbSet<Ware> Wares { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<CategoryValues> CategoryValueses { get; set; }
        DbSet<GOW> GOWs { get; set; }
        DbSet<WaresCategoryValues> WCV { get; set; }
        DbSet<WareVote> WareVotes { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<OrderDetails> OrderDetails { get; set; }
        DbSet<OrderHistory> OrderHistories { get; set; }
        DbSet<WareGOW> WareGOWs { get; set; }
        DbSet<OrderStatus> OrderStatuses { get; set; }
        DbSet<DeliveryService> DeliveryServices { get; set; }
        DbSet<WishList> WishLists { get; set; }
        DbSet<WishListWare> WishListWares { get; set; }
        DbSet<Slider> Sliders { get; set; }

        int SaveChanges();
    }
}
