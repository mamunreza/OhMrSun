using API.Domains.Activities;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Data
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Activity> Activities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder
            //    .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Activity>()
                .Property(p => p.Status)
                .HasDefaultValue(ActivityStatus.Open)
                .HasSentinel(default);

            modelBuilder.Entity<Activity>()
                .Property(p => p.StatusReason)
                .HasDefaultValue(ActivityStatusReason.OpenNew)
                .HasSentinel(default);
        }
    }

}
