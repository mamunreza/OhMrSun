using API.Domains.Activities;
using API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Activities;

public interface IActivitiesRepository
{
    Task<Activity> CreateActivityAsync(Activity activity, CancellationToken cancellationToken);
    Task<(int TotalRecordCount, List<Activity> activities)> GetActivities(
        int recordCount, int pageNumber, CancellationToken cancellationToken);

    Task<Activity?> GetActivityByIdAsync(Guid activityId, CancellationToken cancellationToken);
}
public class ActivitiesRepository : IActivitiesRepository
{

    private readonly DataContext _context;

    public ActivitiesRepository(DataContext dataContext)
    {
        _context = dataContext;
    }

    public async Task<Activity> CreateActivityAsync(Activity activity, CancellationToken cancellationToken)
    {
        _context.Activities.Add(activity);
        _ = await _context.SaveChangesAsync(cancellationToken);

        var createdActivity = await _context.Activities.FirstOrDefaultAsync(x => x.Id == activity.Id);

        return createdActivity!;
    }

    public async Task<(int TotalRecordCount, List<Activity> activities)> GetActivities(
        int recordCount, int pageNumber, 
        CancellationToken cancellationToken)
    {
        var activities = await _context.Activities.ToListAsync();
        return (activities.Count, activities);
    }

    public async Task<Activity?> GetActivityByIdAsync(Guid activityId, CancellationToken cancellationToken)
    {
        return await _context.Activities
            .FirstOrDefaultAsync(x => x.Id == activityId, cancellationToken: cancellationToken);
    }
}
