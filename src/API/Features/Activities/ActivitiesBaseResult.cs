namespace API.Features.Activities;

public class ActivitiesBaseResult
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public DateTime? ActivityDate { get; set; }
    public string? Description { get; set; }
}
