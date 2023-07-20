using Microsoft.EntityFrameworkCore;

namespace MySuperBlogApp.Data
{
    public class MyAppDataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<News> News { get; set; }

        public MyAppDataContext(DbContextOptions<MyAppDataContext> contextOptions) : base(contextOptions)
        {
            Database.EnsureCreated();
        }
    }
}
