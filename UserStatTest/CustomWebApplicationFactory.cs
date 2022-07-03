using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserStat.Models;
using Microsoft.Extensions.Configuration;


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

                descriptor = services.SingleOrDefault(
                    conf=> conf.ServiceType ==
                    typeof(IConfiguration));
                services.Remove(descriptor);
                
                services.AddDbContext<UserQueryContext>(opt =>
                {
                    opt.UseInMemoryDatabase("InMemoryDbUserStatForTesting");

                }
                );

                var builder = new ConfigurationBuilder().AddInMemoryCollection(
                    new System.Collections.Generic.Dictionary<string,string>
                    {
                        ["ProccesTime"] = "5000"
                    }
                );
                var root =builder.Build();
                services.AddSingleton<IConfiguration>(root);
            
                



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