using API.Domains.Activities;
using API.Infrastructure.Logging;
using AutoMapper;
using MediatR;

namespace API.Features.Activities;

public partial class GetActivities
{
    public record Query : IRequest<Result>
    {
        public int Page { get; set; }
        /// <summary> Number of records per page, default is 5 </summary>
        /// <example>5</example>
        public int PageLimit { get; set; }
        public ActivityTimeStatus TimeStatus { get; set; }
    }

    public record Result
    {
        /// <summary> Total existing record count of found activities </summary>
        /// <example>99</example>
        public int TotalRecordCount { get; set; }
        /// <summary> List of desired amount of found activities </summary>
        public List<ActivityRecord> Activities { get; set; } = new List<ActivityRecord>();
    }
    public class ActivityRecord
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime? ActivityDate { get; set; }
        public string? Description { get; set; }
        public ActivityStatus Status { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result>
    {
        private readonly ILogger<Handler> _logger;
        private readonly IActivitiesRepository _repository;
        private readonly IMapper _mapper;

        public Handler(
            ILogger<Handler> logger,
            IActivitiesRepository repository,
            IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result> Handle(Query query, CancellationToken cancellationToken)
        {
            using (_logger.BeginNamedScope(nameof(GetActivities)))
            {
                _logger.LogInformation("Starting to get activities");

                ActivityQueryCriteria criteria = new ActivityQueryCriteria()
                {
                    Page = query.Page,
                    PageLimit = query.PageLimit
                };

                (int TotalRecordCount, List<Activity> Activities) activitiesByQuery;

                if (query.TimeStatus == ActivityTimeStatus.RECENT)
                {
                    activitiesByQuery = await _repository.GetRecentActivities(criteria, cancellationToken);
                }
                else if (query.TimeStatus == ActivityTimeStatus.IN_PROGRESS)
                {
                    activitiesByQuery = await _repository.GetInProgressActivities(criteria, cancellationToken);
                }
                else if (query.TimeStatus == ActivityTimeStatus.FUTURE)
                {
                    activitiesByQuery = await _repository.GetFutureActivities(criteria, cancellationToken);
                }
                else if (query.TimeStatus == ActivityTimeStatus.NOT_SCHECULED)
                {
                    activitiesByQuery = await _repository.GetNotScheduledActivities(criteria, cancellationToken);
                }
                else if (query.TimeStatus == ActivityTimeStatus.PAST)
                {
                    activitiesByQuery = await _repository.GetPastActivities(criteria, cancellationToken);
                }
                else if (query.TimeStatus == ActivityTimeStatus.SUGGESTED)
                {
                    activitiesByQuery = await _repository.GetSuggestedActivities(criteria, cancellationToken);
                }
                else
                {
                    activitiesByQuery = await _repository.GetActivities(criteria, cancellationToken);
                }

                var mappedActivities = _mapper.Map<List<Activity>, List<ActivityRecord>>(activitiesByQuery.Activities);

                _logger.LogInformation("{TotalRecordCount} activities fetched successfully.", activitiesByQuery.TotalRecordCount);

                return new Result
                {
                    Activities = mappedActivities,
                    TotalRecordCount = activitiesByQuery.TotalRecordCount
                };
            }
        }
    }
}
