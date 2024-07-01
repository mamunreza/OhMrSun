using API.Domains.Activities;

namespace API.Features.Activities;

public interface IActivitiesRepository
{
    Task<Activity> CreateActivityAsync(Activity activity, CancellationToken cancellationToken);
}
public class ActivitiesRepository : IActivitiesRepository
{
    public async Task<Activity> CreateActivityAsync(Activity activity, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return new Activity();
    }
}
