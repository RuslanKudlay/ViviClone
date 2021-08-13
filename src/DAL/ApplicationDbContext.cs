using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Entities.Chat;
using Application.EntitiesModels.Entities.Order;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Application.EntitiesModels.Entities.WishList;

namespace Application.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, IdentityUserClaim<int>, ApplicationUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>, IApplicationDbContext
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUsersRoles { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet <Image> Images { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Ware> Wares { get; set; }
        public DbSet<CategoryValues> CategoryValueses { get; set; }
        public DbSet<GOW> GOWs { get; set; }
        public DbSet<WaresCategoryValues> WCV { get; set; }
        public DbSet<WareVote> WareVotes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<OrderHistory> OrderHistories { get; set; }
        public DbSet<WareGOW> WareGOWs { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<DeliveryService> DeliveryServices { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<WishListWare> WishListWares { get; set; }
        public DbSet<Slider> Sliders { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureBrandEntity(modelBuilder);
            ConfigureBlogEntity(modelBuilder);
            ConfigureAnnouncementEntity(modelBuilder);
            ConfigurePromotionEntity(modelBuilder);
            ConfigureImageEntity(modelBuilder);          
            ConfigureCategoryEntity(modelBuilder);
            ConfigureCategoryValuesEntity(modelBuilder);
            ConfigureGOWEntity(modelBuilder);
            ConfigureWareEntity(modelBuilder);
            ConfigureWaresCategoryValuesEntity(modelBuilder);
            ConfigureWareVotesEntity(modelBuilder);
            ConfigureOrderEntities(modelBuilder);
            ConfigureWareGowsEntity(modelBuilder);
            ConfigureWishListEntity(modelBuilder);
            ConfigureWishListWares(modelBuilder);
            ConfigureSliders(modelBuilder);
           
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

        }

        private void ConfigureWareGowsEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WareGOW>()
                .HasKey(wg => wg.Id);
            modelBuilder.Entity<WareGOW>()
                .HasOne(wg => wg.GOW)
                .WithMany(g => g.WareGOWs)
                .HasForeignKey(wg => wg.GOWId);
            modelBuilder.Entity<WareGOW>()
                .HasOne(wg => wg.Ware)
                .WithMany(w => w.WareGOWs)
                .HasForeignKey(wg => wg.WareId);
        }

        private void ConfigureBrandEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>()
                .HasKey(br => br.Id);
            modelBuilder.Entity<Brand>()
                .Property(br => br.Position).IsRequired();
            modelBuilder.Entity<Brand>()
                .Property(br => br.IsEnable).IsRequired();
            modelBuilder.Entity<Brand>()
                .Property(br => br.SubUrl).IsRequired();
            modelBuilder.Entity<Brand>()
                .Property(br => br.Color).IsRequired().HasMaxLength(30);
            modelBuilder.Entity<Brand>()
                .Property(br => br.ColorTitle).IsRequired().HasMaxLength(30);
            modelBuilder.Entity<Brand>()
                .Property(br => br.Name).IsRequired().HasMaxLength(250);
            modelBuilder.Entity<Brand>()
                .Property(br => br.ShortDescription).IsRequired();
        }

        private void ConfigureOrderEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasKey(_ => _.Id);
            modelBuilder.Entity<Order>()
                .Property(_ => _.UserId)
                .IsRequired();
            modelBuilder.Entity<Order>()
                .Property(_ => _.CountOfWares)
                .IsRequired();
            modelBuilder.Entity<Order>()
                .Property(_ => _.CreatedDate)
                .IsRequired();
            modelBuilder.Entity<Order>()
                .Property(_ => _.OrderNumber)
                .IsRequired();
            modelBuilder.Entity<Order>()
                .HasOne(_ => _.User)
                .WithMany(_ => _.Orders)
                .HasForeignKey(_ => _.UserId);
            modelBuilder.Entity<Order>()
                .HasOne(_ => _.OrderStatus)
                .WithMany(_ => _.Orders)
                .HasForeignKey(_ => _.OrderStatusId);
            modelBuilder.Entity<Order>()
                .HasOne(_ => _.DeliveryService)
                .WithMany(_ => _.Orders)
                .HasForeignKey(_ => _.DeliveryServiceId);

            modelBuilder.Entity<OrderDetails>()
                .HasKey(_ => _.Id);
            modelBuilder.Entity<OrderDetails>()
                .Property(_ => _.OrderId)
                .IsRequired();
            modelBuilder.Entity<OrderDetails>()
                .Property(_ => _.WareId)
                .IsRequired();
            modelBuilder.Entity<OrderDetails>()
                .Property(_ => _.Count)
                .IsRequired();
            modelBuilder.Entity<OrderDetails>()
                .HasOne(_ => _.Order)
                .WithMany(_ => _.OrderDetails)
                .HasForeignKey(_ => _.OrderId);
            modelBuilder.Entity<OrderDetails>()
                .HasOne(_ => _.Ware)
                .WithMany(_ => _.OrderDetails)
                .HasForeignKey(_ => _.WareId);

            modelBuilder.Entity<OrderHistory>()
                .HasKey(_ => _.Id);
            modelBuilder.Entity<OrderHistory>()
                .Property(_ => _.OrderId)
                .IsRequired();
            modelBuilder.Entity<OrderHistory>()
                .Property(_ => _.Date)
                .IsRequired();           
            modelBuilder.Entity<OrderHistory>()
                .HasOne(_ => _.Order)
                .WithMany(_ => _.OrderHistories)
                .HasForeignKey(_ => _.OrderId);
            modelBuilder.Entity<OrderHistory>()
                .HasOne(_ => _.User)
                .WithMany(_ => _.OrderHistories)
                .HasForeignKey(_ => _.UserId);

            modelBuilder.Entity<OrderStatus>()
                .HasKey(_ => _.Id);
            modelBuilder.Entity<OrderStatus>()
                .HasAlternateKey(_ => _.StatusName);

            modelBuilder.Entity<DeliveryService>()
                .HasKey(_ => _.Id);
            modelBuilder.Entity<DeliveryService>()
                .HasAlternateKey(_ => _.Name);
        }

        private void ConfigureBlogEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>()
                .HasKey(br => br.Id);
            modelBuilder.Entity<Blog>()
                .Property(br => br.IsEnable).IsRequired();
            modelBuilder.Entity<Blog>()
                .Property(br => br.DateModificated).IsRequired();
            modelBuilder.Entity<Blog>()
                .Property(br => br.Title).IsRequired().HasMaxLength(250);
            modelBuilder.Entity<Blog>()
                .Property(br => br.SubUrl).IsRequired();
            modelBuilder.Entity<Blog>()
                .HasMany(b => b.Posts);
        }

        private void ConfigureAnnouncementEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Announcement>()
                .HasKey(br => br.Id);
            modelBuilder.Entity<Announcement>()
                .Property(br => br.IsEnable).IsRequired();
            modelBuilder.Entity<Announcement>()
                .Property(br => br.Date).IsRequired();
            modelBuilder.Entity<Announcement>()
                .Property(br => br.LastUpdateDate).IsRequired();
            modelBuilder.Entity<Announcement>()
                .Property(br => br.Title).IsRequired().HasMaxLength(250);
            modelBuilder.Entity<Announcement>()
                .Property(br => br.SubUrl).IsRequired();
        }

        private void ConfigurePromotionEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Promotion>()
                .HasKey(br => br.Id);
            modelBuilder.Entity<Promotion>()
                .Property(br => br.IsEnable).IsRequired();
            modelBuilder.Entity<Promotion>()
                .Property(br => br.Date).IsRequired();
            modelBuilder.Entity<Promotion>()
                .Property(br => br.LastUpdateDate).IsRequired();
            modelBuilder.Entity<Promotion>()
                .Property(br => br.Title).IsRequired().HasMaxLength(250);
            modelBuilder.Entity<Promotion>()
                .Property(br => br.SubUrl).IsRequired();
        }

        private void ConfigureImageEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Image>()
                .HasKey(i => i.Id);
            modelBuilder.Entity<Image>()
                .Property(i => i.Data).IsRequired();
        }

        private void ConfigureCategoryEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<Category>()
                .Property(i => i.Name).IsRequired();
            modelBuilder.Entity<Category>()
                .HasMany(c => c.CategoryValueses);
            
        }

        private void ConfigureCategoryValuesEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryValues>()
                .HasKey(c => c.Id);
            
            modelBuilder.Entity<CategoryValues>()
               .HasOne(c => c.Category)
               .WithMany(c=>c.CategoryValueses)
               .HasForeignKey(c=>c.CategoryId);
        }

        private void ConfigureWareEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ware>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Ware>()
                .HasOne(c => c.Brand)
                .WithMany(c => c.Wares)
                .HasForeignKey(c => c.BrandId);


        }

        private void ConfigureWaresCategoryValuesEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WaresCategoryValues>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<WaresCategoryValues>()
               .HasOne(c => c.CategoryValueses)
               .WithMany(c => c.WCV)
               .HasForeignKey(c => c.CategoryValueId);

            modelBuilder.Entity<WaresCategoryValues>()
               .HasOne(c => c.Ware)
               .WithMany(c => c.WCV)
               .HasForeignKey(c => c.WareId);
        }

        private void ConfigureChatEntity (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chat>()
                .HasKey(ch => ch.Id);

            modelBuilder.Entity<Chat>()
               .Property(ch => ch.CreatedDate).IsRequired();

            modelBuilder.Entity<Chat>()
               .Property(ch => ch.SessionOrUserId).IsRequired();

            modelBuilder.Entity<Chat>()
               .Property(ch => ch.IsChatEnded).IsRequired();

            modelBuilder.Entity<Chat>()
                .HasMany(ch => ch.ChatMessages);
        }

        private void ConfigureGOWEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GOW>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<GOW>()
                .HasMany(w => w.Childs)
                .WithOne(w => w.Parent)
                .HasForeignKey(w => w.ParentId);

        }

        private void ConfigureWareVotesEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WareVote>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<WareVote>()
               .HasOne(c => c.Ware)
               .WithMany(c => c.WareVote)
               .HasForeignKey(c => c.WareId);
        }

        private void ConfigureWishListEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WishList>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<WishList>()
               .HasOne(c => c.User)
               .WithMany(c => c.WishLists)
               .HasForeignKey(c => c.UserId);
        }

        private void ConfigureWishListWares(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WishList>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<WishListWare>()
               .HasOne(c => c.WishList)
               .WithMany(c => c.WishListWares)
               .HasForeignKey(c => c.WishListId);

            modelBuilder.Entity<WishListWare>()
               .HasOne(c => c.Ware)
               .WithMany(c => c.WishListWares)
               .HasForeignKey(c => c.WareId);
        }

        private void ConfigureSliders(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Slider>()
                .HasKey(s => s.Id);
            modelBuilder.Entity<Slider>()
                .Property(s => s.Image).IsRequired();
            modelBuilder.Entity<Slider>()
                .Property(s => s.Type).IsRequired();
            modelBuilder.Entity<Slider>()
                .Property(s => s.LinkToWare).IsRequired();
        }
    }
}
