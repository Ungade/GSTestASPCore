using Microsoft.EntityFrameworkCore;

namespace UserStat.Models
{
    public class UserQueryContext:DbContext
    {
        public UserQueryContext(DbContextOptions<UserQueryContext> options):base(options)
        {
            
        }

        public DbSet<Query> Queries{get;set;}
        public DbSet<UserQuery> UserQueries{get;set;}
        public DbSet<QueriesBridge> QueriesBridges{get;set;}

        
    }
}