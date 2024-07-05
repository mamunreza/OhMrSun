namespace API.Domains.Activities;

public class Activity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime? ActivityDate { get; set; }
    public string? Description { get; set; }

    public static class FieldNames
    {
        public const string Id = "activityId";
        public const string Title = "title";
    }
}
