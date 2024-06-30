using API.Domains.Forecasts;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Activity> Activities { get; set; }
    }

}
