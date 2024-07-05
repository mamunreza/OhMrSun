using API.Domains.Activities;

namespace API.Infrastructure.Data;

public class Seed
{
    public static async Task SeedData(DataContext context)
    {
        //if (context.Activities.Any()) return;

        //var activities = new List<Activity>
        //    {
        //        new() {
        //            Title = "Past Activity 1",
        //            ActivityDate = DateTime.UtcNow.AddMonths(-2),
        //            Description = "Activity 2 months ago",
        //            //Category = "boart ride",
        //            //City = "Riga",
        //            //Venue = "Pub",
        //        },
        //        new() {
        //            Title = "Past Activity 2",
        //            ActivityDate = DateTime.UtcNow.AddMonths(-1),
        //            Description = "Activity 1 month ago",
        //            //Category = "culture",
        //            //City = "Paris",
        //            //Venue = "Louvre",
        //        }
        //    };

        //await context.Activities.AddRangeAsync(activities);
        //await context.SaveChangesAsync();
    }
}
