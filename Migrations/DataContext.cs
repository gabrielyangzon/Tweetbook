using Microsoft.EntityFrameworkCore;
using Tweetbook.Domain;

namespace Tweetbook.Migrations
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }


        public DbSet<Post?> Posts { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=PostDb.db");
        }
    }
}
