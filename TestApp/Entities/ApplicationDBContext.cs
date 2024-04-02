using Microsoft.EntityFrameworkCore;

namespace TestApp.Entities
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> opt) : base(opt)
        {

        }

        public DbSet<User> Users => Set<User>();
    }
}
