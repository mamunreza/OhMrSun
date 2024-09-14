using API.Domains.Activities;
using API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Activities;

public interface IActivitiesRepository
{
    Task<Activity> CreateActivityAsync(Activity activity, CancellationToken cancellationToken);
    Task<(int TotalRecordCount, List<Activity> activities)> GetActivities(
        ActivityQueryCriteria criteria, CancellationToken cancellationToken);
    Task<(int TotalRecordCount, List<Activity> activities)> GetRecentActivities(
        ActivityQueryCriteria criteria, CancellationToken cancellationToken);
    Task<(int TotalRecordCount, List<Activity> activities)> GetInProgressActivities(
        ActivityQueryCriteria criteria, CancellationToken cancellationToken);
    Task<(int TotalRecordCount, List<Activity> activities)> GetFutureActivities(
        ActivityQueryCriteria criteria, CancellationToken cancellationToken);
    Task<(int TotalRecordCount, List<Activity> activities)> GetNotScheduledActivities(
        ActivityQueryCriteria criteria, CancellationToken cancellationToken);
    Task<(int TotalRecordCount, List<Activity> activities)> GetPastActivities(
        ActivityQueryCriteria criteria, CancellationToken cancellationToken);
    Task<(int TotalRecordCount, List<Activity> activities)> GetSuggestedActivities(
        ActivityQueryCriteria criteria, CancellationToken cancellationToken);

    Task<Activity?> GetActivityByIdAsync(Guid activityId, CancellationToken cancellationToken);
    Task<Activity> UpdateActivityAsync(Activity activity, CancellationToken cancellationToken);
    Task<int> DeleteActivityAsync(Activity activityToDelete, CancellationToken cancellationToken);
    Task<List<TopNDaysWithMostActivities>> GetTopNDaysWithMostActivities(int n, CancellationToken cancellationToken);
}
public partial class ActivitiesRepository : IActivitiesRepository
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
        ActivityQueryCriteria criteria,
        CancellationToken cancellationToken)
    {
        var totalRecordCount = await _context.Activities.CountAsync(cancellationToken);

        var activities = await _context.Activities
            .Skip((criteria.Page - 1) * criteria.PageLimit)
            .Take(criteria.PageLimit)
            .ToListAsync(cancellationToken);
        return (totalRecordCount, activities);
    }

    public async Task<(int TotalRecordCount, List<Activity> activities)> GetRecentActivities(
        ActivityQueryCriteria criteria,
        CancellationToken cancellationToken)
    {
        await Task.Delay(2000);

        var totalRecordCount = await _context.Activities
            .Where(x => x.ActivityDate.HasValue
                && x.ActivityDate.Value.Day == DateTime.UtcNow.Day
                && x.ActivityDate.Value.TimeOfDay > DateTime.UtcNow.TimeOfDay)
            .CountAsync(cancellationToken);

        var activities = await _context.Activities
            .Where(x => x.ActivityDate.HasValue
                && x.ActivityDate.Value.Day == DateTime.UtcNow.Day
                && x.ActivityDate.Value.TimeOfDay > DateTime.UtcNow.TimeOfDay)
            .Skip((criteria.Page - 1) * criteria.PageLimit)
            .Take(criteria.PageLimit)
            .ToListAsync(cancellationToken);
        return (totalRecordCount, activities);
    }

    public async Task<(int TotalRecordCount, List<Activity> activities)> GetInProgressActivities(
        ActivityQueryCriteria criteria,
        CancellationToken cancellationToken)
    {
        var totalRecordCount = await _context.Activities
            .Where(x => x.ActivityDate.HasValue
                && x.ActivityDate.Value.Day == DateTime.UtcNow.Day
                && x.ActivityDate.Value.TimeOfDay < DateTime.UtcNow.TimeOfDay)
            .CountAsync(cancellationToken);

        var activities = await _context.Activities
            .Where(x => x.ActivityDate.HasValue
                && x.ActivityDate.Value.Day == DateTime.UtcNow.Day
                && x.ActivityDate.Value.TimeOfDay < DateTime.UtcNow.TimeOfDay)
            .Skip((criteria.Page - 1) * criteria.PageLimit)
            .Take(criteria.PageLimit)
            .ToListAsync(cancellationToken);
        return (totalRecordCount, activities);
    }

    public async Task<(int TotalRecordCount, List<Activity> activities)> GetFutureActivities(
        ActivityQueryCriteria criteria,
        CancellationToken cancellationToken)
    {
        var totalRecordCount = await _context.Activities
            .Where(x => x.ActivityDate.HasValue && x.ActivityDate.Value.Day > DateTime.UtcNow.Day)
            .CountAsync(cancellationToken);

        var activities = await _context.Activities
            .Where(x => x.ActivityDate.HasValue && x.ActivityDate.Value.Day > DateTime.UtcNow.Day)
            .Skip((criteria.Page - 1) * criteria.PageLimit)
            .Take(criteria.PageLimit)
            .ToListAsync(cancellationToken);
        return (totalRecordCount, activities);
    }

    public async Task<(int TotalRecordCount, List<Activity> activities)> GetNotScheduledActivities(
        ActivityQueryCriteria criteria,
        CancellationToken cancellationToken)
    {
        var totalRecordCount = await _context.Activities
            .Where(x => x.ActivityDate == null)
            .CountAsync(cancellationToken);

        var activities = await _context.Activities
            .Where(x => x.ActivityDate == null)
            .Skip((criteria.Page - 1) * criteria.PageLimit)
            .Take(criteria.PageLimit)
            .ToListAsync(cancellationToken);
        return (totalRecordCount, activities);
    }

    public async Task<(int TotalRecordCount, List<Activity> activities)> GetPastActivities(
        ActivityQueryCriteria criteria,
        CancellationToken cancellationToken)
    {
        var totalRecordCount = await _context.Activities
            .Where(x => x.ActivityDate.HasValue && x.ActivityDate.Value.Day < DateTime.UtcNow.Day - 1)
            .CountAsync(cancellationToken);

        var activities = await _context.Activities
            .Where(x => x.ActivityDate.HasValue && x.ActivityDate.Value.Day < DateTime.UtcNow.Day - 1)
            .Skip((criteria.Page - 1) * criteria.PageLimit)
            .Take(criteria.PageLimit)
            .ToListAsync(cancellationToken);
        return (totalRecordCount, activities);
    }

    public async Task<(int TotalRecordCount, List<Activity> activities)> GetSuggestedActivities(
        ActivityQueryCriteria criteria,
        CancellationToken cancellationToken)
    {
        await Task.Delay(1500);
        var totalRecordCount = 0;

        var activities = new List<Activity>();

        await Task.CompletedTask;
        return (totalRecordCount, activities);
    }

    public async Task<Activity?> GetActivityByIdAsync(Guid activityId, CancellationToken cancellationToken)
    {
        return await _context.Activities
            .FirstOrDefaultAsync(x => x.Id == activityId, cancellationToken: cancellationToken);
    }

    public async Task<Activity> UpdateActivityAsync(Activity activity, CancellationToken cancellationToken)
    {
        _context.Activities.Update(activity);
        _ = await _context.SaveChangesAsync(cancellationToken);

        var updatedActivity = await _context.Activities.FirstOrDefaultAsync(x => x.Id == activity.Id);

        return updatedActivity!;
    }

    public async Task<int> DeleteActivityAsync(Activity activityToDelete, CancellationToken cancellationToken)
    {
        _context.Activities.Remove(activityToDelete);
        var deleteResponse = await _context.SaveChangesAsync(cancellationToken);

        return deleteResponse;
    }
    public async Task<List<TopNDaysWithMostActivities>> GetTopNDaysWithMostActivities(
        int n, CancellationToken cancellationToken)
    {
        var topNDays = await _context.Activities
            .Where(x => x.ActivityDate.HasValue)
            .GroupBy(x => x.ActivityDate!.Value.Date)
            .OrderByDescending(g => g.Count())
            .Take(n)
            .Select(g => new TopNDaysWithMostActivities { Day = g.Key, ActivityCount = g.Count() })
            .ToListAsync(cancellationToken);

        return topNDays;
    }
}
public class TopNDaysWithMostActivities
{
    public DateTime Day { get; set; }
    public int ActivityCount { get; set; }
}
