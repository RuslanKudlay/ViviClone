using Microsoft.Extensions.DependencyInjection;

namespace Application.DAL
{
    public class DbContextFactory : IDbContextFactory
    {
        private readonly IServiceScopeFactory serviceFactory;


        public DbContextFactory(IServiceScopeFactory _serviceFactory)
        {
            serviceFactory = _serviceFactory;           
        }

        public IApplicationDbContext Create()
        {
            var serviceProvider = serviceFactory.CreateScope().ServiceProvider;
            return serviceProvider.GetService<IApplicationDbContext>();
        }
    }
}
