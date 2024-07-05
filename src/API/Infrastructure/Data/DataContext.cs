using API.Domains.Activities;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Data
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Activity> Activities { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder
        //        .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        //    base.OnModelCreating(modelBuilder);
        //}
    }

}
