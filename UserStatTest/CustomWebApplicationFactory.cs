using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserStat.Models;


namespace UserStatTest
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {

        public CustomWebApplicationFactory()
        {
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices( services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                    typeof(DbContextOptions<UserQueryContext>));
                services.Remove(descriptor);
                services.AddDbContext<UserQueryContext>(opt =>
                {
                    opt.UseInMemoryDatabase("InMemoryDbUserStatForTesting");

                }
                );

                var serviceProvider = services.BuildServiceProvider();
                using (var scope = serviceProvider.CreateScope())
                {
                    var scopeServices = scope.ServiceProvider;
                    var context = scopeServices.GetRequiredService<UserQueryContext>();
                    context.Database.EnsureCreated();
                }
            });
        }

        
    }
}