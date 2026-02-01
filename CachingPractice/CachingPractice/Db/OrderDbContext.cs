using Microsoft.EntityFrameworkCore;

namespace CachingPractice.Db
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
        }
        public DbSet<Order> Order { get; set; }

    }
}
