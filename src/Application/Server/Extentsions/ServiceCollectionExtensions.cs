using Application.BBL.BusinessServices;
using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.BusinessServiceCommon;
using Application.DAL;
using Application.EntitiesModels.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.EntityFrameworkCore.Extensions;
using System.Text;

namespace Application.Server.Extentsions
{
    public static class ServiceCollectionExtensions
    {
        private enum SqlType
        {
            SqlLite = 1,
            MsSql = 2,
            MySql = 3
        }

        public static IServiceCollection AddCustomDevDbContext(this IServiceCollection services)
        {           
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var useSqLite = (SqlType)int.Parse(Startup.Configuration["Data:SQLType"]);

                switch (useSqLite)
                {
                    case SqlType.SqlLite:
                        options.UseSqlite(Startup.Configuration["Data:Dev:SqlLiteConnectionString"]);
                        break;
                    case SqlType.MsSql:
                        options.UseSqlServer(Startup.Configuration["Data:Dev:SqlServerConnectionString"], b => b.MigrationsAssembly("Application.DAL"));
                        break;
                    case SqlType.MySql:
                        options.UseMySQL(Startup.Configuration["Data:Dev:MySqlConnectionString"], b => b.MigrationsAssembly("Application.DAL"));
                        break;
                    default:
                        throw new System.Exception("Connection string was mot found");   
                }               
            });
            return services;
        }

        public static IServiceCollection AddCustomProdDbContext(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var useSqLite = (SqlType)int.Parse(Startup.Configuration["Data:SQLType"]);

                switch (useSqLite)
                {
                    case SqlType.SqlLite:
                        options.UseSqlite(Startup.Configuration["Data:Prod:SqlLiteConnectionString"]);
                        break;
                    case SqlType.MsSql:
                        options.UseSqlServer(Startup.Configuration["Data:Prod:SqlServerConnectionString"], b => b.MigrationsAssembly("Application.DAL"));
                        break;
                    case SqlType.MySql:
                        options.UseMySQL(Startup.Configuration["Data:Prod:MySqlConnectionString"], b => b.MigrationsAssembly("Application.DAL"));
                        break;
                    default:
                        throw new System.Exception("Connection string was mot found");
                }             
            });
            return services;
        }

        public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
                // Allow all character Unicode
                options.User.AllowedUserNameCharacters = null;
            })
           .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthService.KEY)),
                };
            });
            return services;
        }

        public static IServiceCollection AddDependencies ( this IServiceCollection services)
        {
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            services.AddScoped<IDbContextFactory, DbContextFactory>();           
            services.AddTransient<IAnnouncementService, AnnouncementService>();
            services.AddTransient<IChatService, ChatService>();
            services.AddTransient<IBrandService, BrandService>();
            services.AddTransient<IPictureAttacherService, PictureAttacherService>();
            services.AddTransient<IPromotionService, PromotionService>();
            services.AddTransient<IBlogService, BlogService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IWareService, WareService>();
            services.AddTransient<ISearchService, SearchService>();
            services.AddTransient<ICategoryValuesService, CategoryValuesService>();
            services.AddTransient<IGOWService, GOWService>();
            services.AddTransient<IWaresCategoryValuesService, WaresCategoryValuesService>();
            services.AddTransient<IBasketService, BasketService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddSingleton<IModelMapper, ModelMapper>();
            services.AddTransient<ISmtpClient, GmailSmtpClient>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUserService, UserService>();
            services.AddScoped<IWishListService, WishListService>();
            services.AddScoped<ISliderService, SliderService>();
            return services;
        }

        public static IServiceCollection AddFacebookAuth(this IServiceCollection services)
        {
            return services;
        }
    }
}
