using API.Domains.Activities;
using API.Infrastructure.Logging;
using AutoMapper;
using MediatR;

namespace API.Features.Activities;

public class GetActivities
{
    public record Query : IRequest<Result>
    {
        public int Page { get; set; } = 1;
        /// <summary> Number of records per page, default is 5 </summary>
        /// <example>5</example>
        public int PageLimit { get; set; } = 5;
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

                (int TotalRecordCount, List<Activity> Activities) activitiesByQuery;
                activitiesByQuery = await _repository.GetActivities(query.PageLimit, query.Page, cancellationToken);

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
