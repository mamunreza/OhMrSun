namespace API.Features.Activities;
public class ActivityQueryCriteria
{
    public int Page { get; set; }
    public int PageLimit { get; set; }
    public bool SortAscending { get; set; } = true;
}
