using API.Features.Activities;
using MediatR;

namespace Features.Activities;

public partial class GetTopNDaysWithMostActivities
{
    public record Query : IRequest<Result>
    {
        public int NumberOfDays { get; set; }
    }

    public record Result
    {
        public List<DayActivity> TopDays { get; set; } = [];
    }

    public class DayActivity
    {
        public DateTime Date { get; set; }
        public int ActivityCount { get; set; }

        public DayActivity(DateTime date, int activityCount)
        {
            Date = date;
            ActivityCount = activityCount;
        }
    }

    public class Handler : IRequestHandler<Query, Result>
    {
        private readonly ILogger<Handler> _logger;
        private readonly IActivitiesRepository _repository;

        public Handler(
            ILogger<Handler> logger,
            IActivitiesRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Result> Handle(Query query, CancellationToken cancellationToken)
        {
            var topDays = await _repository.GetTopNDaysWithMostActivities(query.NumberOfDays, cancellationToken);

            var result = new Result
            {
                TopDays = topDays.Select(x => new DayActivity(x.Day, x.ActivityCount)).ToList()
            };

            return result;
        }
    }
}
