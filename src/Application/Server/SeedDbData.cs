using Application.DAL;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Entities.Order;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Server
{
    public class SeedDbData
    {
        readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnv;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public SeedDbData(IWebHost host, ApplicationDbContext context)
        {
            var services = (IServiceScopeFactory)host.Services.GetService(typeof(IServiceScopeFactory));
            var serviceScope = services.CreateScope();
            _hostingEnv = serviceScope.ServiceProvider.GetService<IHostingEnvironment>();
            _roleManager = serviceScope.ServiceProvider.GetService<RoleManager<ApplicationRole>>();
            _userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            _context = context;
            CreateRoles(); 
            CreateUsers();     
            CreateBrands();
            CreatePromotions();
            CreateBlog();
            CreateAnnouncements();
            CreateCategories();
            CreateGroupOfWares();

            _context.SaveChanges();

            CreateWares();  
            CreateCategoryValues();           

            _context.SaveChanges();

            CreateWaresCategoryValues();

            _context.SaveChanges();

            if (!_context.OrderStatuses.Any())
            {
                _context.OrderStatuses.AddRange(new OrderStatus() { StatusName = "Created" },
                    new OrderStatus() { StatusName = "Paid" },
                    new OrderStatus() { StatusName = "Sent" },
                    new OrderStatus() { StatusName = "Delivered" });
            };
                

            _context.SaveChanges();
        }

        private void CreateRoles()
        {
            if (!_context.ApplicationUsers.Any())
            {
                var rolesToAdd = new List<ApplicationRole>(){
                new ApplicationRole { Name= "Admin" },
                new ApplicationRole { Name= "Manager" },
                new ApplicationRole { Name= "User" },
                new ApplicationRole { Name= "Professional"}
            };
                foreach (var role in rolesToAdd)
                {
                    if (!_roleManager.RoleExistsAsync(role.Name).Result)
                    {
                        _roleManager.CreateAsync(role).Result.ToString();
                    }
                }
            }
                
        }
        private void CreateUsers()
        {
            if (!_context.ApplicationUsers.Any())
            {

                _userManager.CreateAsync(new ApplicationUser { UserName = "admin@admin.com", FirstName = "Admin first", LastName = "Admin last", Email = "admin@admin.com", EmailConfirmed = true, CreatedDate = DateTime.Now, IsEnabled = true }, "admin@admin1").Result.ToString();
                 var test = _userManager.FindByNameAsync("admin@admin.com");
                _userManager.AddToRoleAsync(test.GetAwaiter().GetResult(), "Admin").Result.ToString();



                _userManager.CreateAsync(new ApplicationUser { UserName = "user@user.com", FirstName = "First", LastName = "Last", Email = "user@user.com", EmailConfirmed = true, CreatedDate = DateTime.Now, IsEnabled = true }, "user@user1").Result.ToString();
                _userManager.AddToRoleAsync(_userManager.FindByNameAsync("user@user.com").GetAwaiter().GetResult(), "User").Result.ToString();
            }
        }


        private void CreatePromotions()
        {
            if (!_context.Promotions.Any())
            {
                _context.Promotions.Add(new Promotion()
                {
                    Title = "Promotions First",
                    Body = "Descriptions of forst promotion",
                    MetaDescription = "Promotion",
                    LastUpdateDate = new DateTime(2017, 9, 10),
                    Date = new DateTime(2017, 8, 9),
                    IsEnable = true,
                    SubUrl = "firstpromotion"
                });
                _context.Promotions.Add(new Promotion()
                {
                    Title = "Second Promotion",
                    Body = "Descriptions of second promotion",
                    IsEnable = true,
                    Date = new DateTime(2017, 8, 10),
                    LastUpdateDate = new DateTime(2017, 8, 24),
                    MetaDescription = "Second Promotion",
                    SubUrl = "secondpromotion"
                });
            }
        }

        private void CreateBrands()
        {
            if (!_context.Brands.Any())
            {
                _context.Brands.Add(new Brand()
                {
                    Body = "First Brand Info",
                    Color = "#3fa836",
                    ColorTitle = "#6b7744",
                    IsEnable = true,
                    MetaDescription = "First Brand",
                    MetaKeywords = "Brand",
                    Name = "First Brand",
                    Position = 1,
                    ShortDescription = "Some text",
                    SubUrl = "firstbrand"
                });

                _context.Brands.Add(new Brand()
                {
                    Body = "Second Brand Info",
                    Color = "#23db8a",
                    ColorTitle = "#5e7772",
                    IsEnable = true,
                    MetaDescription = "Second Brand",
                    MetaKeywords = "Brand",
                    Name = "Second Brand",
                    Position = 1,
                    ShortDescription = "Some text of second brand",
                    SubUrl = "secondbrand"
                });
            }
        }

        private void CreateBlog()
        {
            if (!_context.Blogs.Any())
            {
                _context.Blogs.Add(new Blog()
                {
                    Title = "First Post",
                    Body = "Description of First Post",
                    DateModificated = new DateTime(2017, 9, 12),
                    IsEnable = true,
                    MetaDescription = "Post",
                    SubUrl = "firstpoost"
                });

               

                _context.Blogs.Add(new Blog()
                {
                    Title = "Second Post",
                    Body = "Description of Second Post",
                    DateModificated = new DateTime(2017, 9, 20),
                    IsEnable = true,
                    MetaDescription = "Post",
                    SubUrl = "secondpoost"
                });
            }

            if (!_context.Posts.Any())
            {
                for (int i = 0; i < 10; i++)
                {
                    _context.Posts.Add(new Post()
                    {
                        Title = "Title_" + i.ToString(),
                        BlogId = 1,
                        DateCreated = DateTime.Now,
                        DateModificated = DateTime.Now,
                        Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit," +
                        " sed do eiusmod tempor incididunt ut labore et dolore magna aliqua." +
                        " Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi " +
                        "ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit " +
                        "in voluptate velit esse cillum dolore eu fugiat nulla pariatur. " +
                        "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia" +
                        " deserunt mollit anim id est laborum.",
                        MetaDescription = i.ToString() + "_post description",
                        MetaKeywords = i.ToString() + "_meta keywords",
                        SubUrl = i.ToString() + "-post-" + "sub" + "-url"
                    });
                }
            }
        }

        private void CreateAnnouncements()
        {
            if (!_context.Announcements.Any())
            {
                _context.Announcements.Add(new Announcement()
                {
                    Title = "First Announcement",
                    Body = "Body of First Annoucement",
                    Date = new DateTime(2017, 6, 10),
                    LastUpdateDate = new DateTime(2017, 10, 9),
                    IsEnable = true,
                    MetaDescription = "Announcement",
                    SubUrl = "firstannouncement"
                });

                _context.Announcements.Add(new Announcement()
                {
                    Title = "Second Announcement",
                    Body = "Body of Second Annoucement",
                    Date = new DateTime(2017, 6, 11),
                    LastUpdateDate = new DateTime(2017, 10, 10),
                    IsEnable = true,
                    MetaDescription = "Announcement",
                    SubUrl = "secondannouncement"
                });
            }
        }

        private void CreateCategories()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category()
                {
                    Name = "Тип",
                    MetaDescription = "тип",
                    MetaKeywords = "тип",
                    SubUrl = "tip",
                    IsEnable = true
                });
                _context.Categories.Add(new Category()
                {
                    Name = "Действие",
                    MetaDescription = "действие",
                    MetaKeywords = "действие",
                    SubUrl = "deystvie",
                    IsEnable = true
                });

                _context.Categories.Add(new Category()
                {
                    Name = "Назначение",
                    MetaDescription = "назначение",
                    MetaKeywords = "назначение",
                    SubUrl = "naznachenie",
                    IsEnable = true
                });

                _context.Categories.Add(new Category()
                {
                    Name = "Упаковка",
                    MetaDescription = "Упаковка",
                    MetaKeywords = "Упаковка",
                    SubUrl = "upakovka",
                    IsEnable = true
                });
            }
        }

        private void CreateWares()
        {
            if (!_context.Wares.Any())
            {
                _context.Wares.Add(new Ware()
                {
                    IsEnable = true,
                    MetaDescription = "ПИТАТЕЛЬНО-УСПОКАИВАЮЩИЙ КРЕМ",
                    MetaKeywords = "ПИТАТЕЛЬНО-УСПОКАИВАЮЩИЙ КРЕМ",
                    Name = "ПИТАТЕЛЬНО-УСПОКАИВАЮЩИЙ КРЕМ",
                    Price = 4.99,
                    SubUrl = "PITATELЬNO-USPOKAIVAYuSchIY-KREM".ToLower(),
                    VendorCode = "44556",
                    Text = "The first product enriched with Cold Cream Marine that effectively replenishes, soothes and repairs all dry and sensitive skin for guaranteed comfort for up to 24 hours.",    
                });
                _context.Wares.Add(new Ware()
                {
                    IsEnable = true,
                    MetaDescription = "ИНТЕНСИВНЫЙ ПИТАТЕЛЬНО-УСПОКАИВАЮЩИЙ КРЕМ",
                    MetaKeywords = "ИНТЕНСИВНЫЙ ПИТАТЕЛЬНО-УСПОКАИВАЮЩИЙ КРЕМ",
                    Name = "ИНТЕНСИВНЫЙ ПИТАТЕЛЬНО-УСПОКАИВАЮЩИЙ КРЕМ",
                    Price = 7.99,
                    SubUrl = "INTENSIVNYiY-PITATELЬNO-USPOKAIVAYuSchIY-KREM".ToLower(),
                    VendorCode = "45455",
                    Text = "The first product enriched with Cold Cream Marine that effectively replenishes, soothes and repairs very dry and sensitive skin for guaranteed comfort for up to 24 hours.",
                });

                _context.Wares.Add(new Ware()
                {
                    IsEnable = true,
                    MetaDescription = "КОНЦЕНТРАТ АБСОЛЮТНОЕ СИЯНИЕ",
                    MetaKeywords = "КОНЦЕНТРАТ АБСОЛЮТНОЕ СИЯНИЕ",
                    Name = "КОНЦЕНТРАТ АБСОЛЮТНОЕ СИЯНИЕ",
                    Price = 5.99,
                    SubUrl = "KONTsENTRAT-ABSOLYuTNOE-SIYaNIE".ToLower(),
                    VendorCode = "45455",
                    Text = "The first product enriched with Cold Cream Marine that effectively replenishes, soothes and repairs very dry and sensitive skin for guaranteed comfort for up to 24 hours.",
                });

                _context.Wares.Add(new Ware()
                {
                    IsEnable = true,
                    MetaDescription = "Мягкое Очищающее Молочко",
                    MetaKeywords = "Мягкое Очищающее Молочко",
                    Name = "Мягкое Очищающее Молочко",
                    Price = 5.99,
                    SubUrl = "Myagkoe-Ochischayuschee-Molochko".ToLower(),
                    VendorCode = "45455",
                    Text = "Нежное Очищающее Молочко с Sève Bleue des Océans эффективно очищает кожу, удаляет макияж загрязнения, для всех типов кожи, даже самой чувствительной.",
                });

                _context.Wares.Add(new Ware()
                {
                    IsEnable = true,
                    MetaDescription = "Мягкое Очищающее Молочко",
                    MetaKeywords = "Мягкое Очищающее Молочко",
                    Name = "Тонизирующий Лосьон, Восстанавливающий Красоту Кожи",
                    Price = 5.99,
                    SubUrl = "Toniziruyuschiy-Loson-Vosstanavlivayuschiy-Krasotu-Kozhi".ToLower(),
                    VendorCode = "45455",
                    Text = "A tonic lotion for the face and eyes with Sève Bleue des Océans that perfectly completes your cleansing routine. Suitable for all skin types, even sensitive.",
                });
                _context.Wares.Add(new Ware()
                {
                    IsEnable = true,
                    MetaDescription = "Коллагеновый крем",
                    MetaKeywords = "Коллагеновый крем",
                    Name = "Коллагеновый крем",
                    Price = 5.99,
                    SubUrl = "kollagenovyiy-krem".ToLower(),
                    VendorCode = "45455",
                    Text = "Заметно разглаживает первые морщинки уже после первого применения. Оптимально восстанавливает и увлажняет кожу.",
                });

                _context.Wares.Add(new Ware()
                {
                    IsEnable = true,
                    MetaDescription = "Сыворотка с Коллагеном",
                    MetaKeywords = "Сыворотка с Коллагеном",
                    Name = "Сыворотка с Коллагеном",
                    Price = 5.99,
                    SubUrl = "Syivorotka-s-Kollagenom".ToLower(),
                    VendorCode = "45455",
                    Text = "Сыворотка с высокой концентрацией Морского Коллагена мгновенно заполняет и разглаживает морщины. Благодая эффекту 'второй кожи' уход становится невероятно комфортным и приятным.",
                });

                _context.Wares.Add(new Ware()
                {
                    IsEnable = true,
                    MetaDescription = "Гиалуроновый крем",
                    MetaKeywords = "Гиалуроновый крем",
                    Name = "Гиалуроновый крем",
                    Price = 5.99,
                    SubUrl = "Gialuronovyiy-krem".ToLower(),
                    VendorCode = "45455",
                    Text = "Активный антивозрастной крем, стимулирует синтез гиалуроновой кислоты, заметно сокращает количество и глубину морщин.",
                });

                _context.Wares.Add(new Ware()
                {
                    IsEnable = true,
                    MetaDescription = "Гиалуроновый карандаш",
                    MetaKeywords = "Гиалуроновый карандаш",
                    Name = "Гиалуроновый карандаш",
                    Price = 5.99,
                    SubUrl = "Gialuronovyiy-karandash".ToLower(),
                    VendorCode = "45455",
                    Text = "Благодаря высокой концентрации Гиалуроновой Кислоты Морского происхождения средство проникает глубоко в клетки кожи и мгновенно заполняет морщины.",
                });



            }
        }

        private void CreateGroupOfWares()
        {
            if (!_context.GOWs.Any())
            {
                _context.GOWs.Add(new GOW()
                {
                    IsEnable = true,
                    MetaDescription = "Базовый уход для кожи",
                    MetaKeywords = "Базовый уход для кожи",
                    Name = "Базовый уход для кожи",
                    SubUrl = "bazovyiy-uhod-dlya-kozhi",                    
                });               
               
                _context.GOWs.Add(new GOW()
                {
                    IsEnable = true,
                    MetaDescription = "Увлажнение",
                    MetaKeywords = "Увлажнение",
                    Name = "Увлажнение",
                    SubUrl = "uvlazhnenie", 
                    Parent = _context.GOWs.FirstOrDefault(w => w.Name.Contains("Базовый уход для кожи"))

                });

                _context.GOWs.Add(new GOW()
                {
                    IsEnable = true,
                    MetaDescription = "Холодный морской крем",
                    MetaKeywords = "Холодный морской крем",
                    Name = "Холодный морской крем",
                    SubUrl = "holodnyiy-morskoy-krem",
                    Parent = _context.GOWs.FirstOrDefault(w => w.Name.Contains("Базовый уход для кожи"))
                });

                _context.GOWs.Add(new GOW()
                {
                    IsEnable = true,
                    MetaDescription = "Пробуждение морем",
                    MetaKeywords = "Пробуждение морем",
                    Name = "Пробуждение морем",
                    SubUrl = "probuzhdenie-morem",                    
                });

                _context.GOWs.Add(new GOW()
                {
                    IsEnable = true,
                    MetaDescription = "Снятие макияжа",
                    MetaKeywords = "Снятие макияжа",
                    Name = "Снятие макияжа",
                    SubUrl = "snyatie-makiyazha",
                    Parent = _context.GOWs.FirstOrDefault(w => w.Name.Contains("Пробуждение морем"))
                });

                _context.GOWs.Add(new GOW()
                {
                    IsEnable = true,
                    MetaDescription = "Очищение",
                    MetaKeywords = "Очищение",
                    Name = "Очищение",
                    SubUrl = "ochischenie",
                    Parent = _context.GOWs.FirstOrDefault(w => w.Name.Contains("Снятие макияжа"))
                });

                _context.GOWs.Add(new GOW()
                {
                    IsEnable = true,
                    MetaDescription = "Средства для отшелушивания",
                    MetaKeywords = "Средства для отшелушивания",
                    Name = "Средства для отшелушивания",
                    SubUrl = "sredstva-dlya-otshelushivaniya",
                    Parent = _context.GOWs.FirstOrDefault(w => w.Name.Contains("Снятие макияжа"))
                }); 

                _context.GOWs.Add(new GOW()
                {
                    IsEnable = true,
                    MetaDescription = "Антивозрастной уход",
                    MetaKeywords = "Антивозрастной уход",
                    Name = "Антивозрастной уход",
                    SubUrl = "antivozrastnoy-uhod",                    
                });

                _context.GOWs.Add(new GOW()
                {
                    IsEnable = true,
                    MetaDescription = "Коллагеновый уход после 25 лет",
                    MetaKeywords = "Коллагеновый уход после 25 лет",
                    Name = "Коллагеновый уход после 25 лет",
                    SubUrl = "kollagenovyiy-uhod-posle-25-let",
                    Parent = _context.GOWs.FirstOrDefault(w => w.Name.Contains("Антивозрастной уход"))
                });

                _context.GOWs.Add(new GOW()
                {
                    IsEnable = true,
                    MetaDescription = "Гиалуроновый уход после 35 лет",
                    MetaKeywords = "Гиалуроновый уход после 35 лет",
                    Name = "Гиалуроновый уход после 35 лет",
                    SubUrl = "gialuronovyiy-uhod-posle-35-let",
                    Parent = _context.GOWs.FirstOrDefault(w => w.Name.Contains("Антивозрастной уход"))
                });

            }
        }

        private void CreateCategoryValues()
        {
            if(!_context.CategoryValueses.Any())
            {
                _context.CategoryValueses.Add(new CategoryValues()
                {
                    Name = "Сухая кожа ",
                    Category = _context.Categories.FirstOrDefault(c => c.Name.Contains("Тип")),
                    IsEnable = true,
                });
                _context.CategoryValueses.Add(new CategoryValues()
                {
                    Name = "Чувствительная кожа",
                    Category = _context.Categories.FirstOrDefault(c => c.Name.Contains("Тип")),
                    IsEnable = false
                });
                _context.CategoryValueses.Add(new CategoryValues()
                {
                    Name = "Крем",
                    Category = _context.Categories.FirstOrDefault(c => c.Name.Contains("Действие")),
                    IsEnable = false
                });
                _context.CategoryValueses.Add(new CategoryValues()
                {
                    Name = "Питание",
                    Category = _context.Categories.FirstOrDefault(c => c.Name.Contains("Назначение")),
                    IsEnable = false
                });

                _context.CategoryValueses.Add(new CategoryValues()
                {
                    Name = "Питание.Комфорт",
                    Category = _context.Categories.FirstOrDefault(c => c.Name.Contains("Назначение")),
                    IsEnable = false
                });

                _context.CategoryValueses.Add(new CategoryValues()
                {
                    Name = "Защита.Смягчение",
                    Category = _context.Categories.FirstOrDefault(c => c.Name.Contains("Назначение")),
                    IsEnable = false
                });

                _context.CategoryValueses.Add(new CategoryValues()
                {
                    Name = "50 мл",
                    Category = _context.Categories.FirstOrDefault(c => c.Name.Contains("Упаковка")),
                    IsEnable = false
                });

                _context.CategoryValueses.Add(new CategoryValues()
                {
                    Name = "Уставшая/Тусклая Кожа",
                    Category = _context.Categories.FirstOrDefault(c => c.Name.Contains("Тип")),
                    IsEnable = false
                });

                _context.CategoryValueses.Add(new CategoryValues()
                {
                    Name = "Увлажнение",
                    Category = _context.Categories.FirstOrDefault(c => c.Name.Contains("Назначение")),
                    IsEnable = false
                });

                _context.CategoryValueses.Add(new CategoryValues()
                {
                    Name = "7*1.2 мл",
                    Category = _context.Categories.FirstOrDefault(c => c.Name.Contains("Упаковка")),
                    IsEnable = false
                });
            }
        }

        private void CreateWaresCategoryValues()
        {
            if (!_context.WCV.Any())
            {                          

                _context.WCV.Add(new WaresCategoryValues()
                {
                    CategoryValueses = _context.CategoryValueses.FirstOrDefault(c => c.Name.Contains("Сухая кожа")),
                    Ware = _context.Wares.FirstOrDefault(w => w.Name.Contains("ИНТЕНСИВНЫЙ ПИТАТЕЛЬНО-УСПОКАИВАЮЩИЙ КРЕМ"))
                });


                _context.WCV.Add(new WaresCategoryValues()
                {
                    CategoryValueses = _context.CategoryValueses.FirstOrDefault(c => c.Name.Contains("Уставшая/Тусклая Кожа")),
                    Ware = _context.Wares.FirstOrDefault(w => w.Name.Contains("Коллагеновый крем"))
                });


                _context.WCV.Add(new WaresCategoryValues()
                {
                    CategoryValueses = _context.CategoryValueses.FirstOrDefault(c => c.Name.Contains("7*1.2 мл")),
                    Ware = _context.Wares.FirstOrDefault(w => w.Name.Contains("Коллагеновый крем"))
                });

                _context.WCV.Add(new WaresCategoryValues()
                {
                    CategoryValueses = _context.CategoryValueses.FirstOrDefault(c => c.Name.Contains("Увлажнение")),
                    Ware = _context.Wares.FirstOrDefault(w => w.Name.Contains("Коллагеновый крем"))
                });
                _context.WCV.Add(new WaresCategoryValues()
                {
                    CategoryValueses = _context.CategoryValueses.FirstOrDefault(c => c.Name.Contains("Увлажнение")),
                    Ware = _context.Wares.FirstOrDefault(w => w.Name.Contains("Мягкое Очищающее Молочко"))
                });



            }
        }
    }
}
