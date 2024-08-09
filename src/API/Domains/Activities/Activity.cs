namespace API.Domains.Activities;

public class Activity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime? ActivityDate { get; set; }
    public string? Description { get; set; }
    public ActivityStatus Status {  get; set; }
    public ActivityStatusReason StatusReason { get; set; }

    public static class FieldNames
    {
        public const string Id = "activityId";
        public const string Title = "title";
    }
}

public enum ActivityStatus
{
    Open = 1,
    Completed = 2,
    Canceled = 3,
    Scheduled = 4,
    Deleted = 5
}

public enum ActivityStatusReason
{
    OpenNew = 1, // Status = 1
}