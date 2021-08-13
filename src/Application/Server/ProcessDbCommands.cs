using Application.DAL;
using Application.Server.Extentsions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Application.Server
{
    public class ProcessDbCommands
    {
        public static void Process(string[] args, IWebHost host)
        {
            var services = (IServiceScopeFactory)host.Services.GetService(typeof(IServiceScopeFactory));
            var enviroment = services.CreateScope().ServiceProvider.GetService<IHostingEnvironment>();

            using (var scope = services.CreateScope())
            {
                var db = GetApplicationDbContext(scope);
               
                if (args.Contains("dropdb"))
                {
                    Console.WriteLine("Dropping database");
                    db.Database.EnsureDeleted();
                }
                else if (args.Contains("migratedb"))
                {
                    Console.WriteLine("Migrating database");
                    db.Database.Migrate();
                }
                else if (args.Contains("seeddb"))
                {
                    Console.WriteLine("Seeding database");
                    db.Seed(host);
                }   
                else {

                    db.Database.Migrate();
                    db.Seed(host);
                }

            }
        }

        private static ApplicationDbContext GetApplicationDbContext(IServiceScope services)
        {
            var db = services.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return db;
        }
    }
}
