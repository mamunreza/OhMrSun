using API.Domains.Activities;
using API.Infrastructure.Logging;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Features.Activities;

public class GetActivity
{
    public record Query : IRequest<Activity>
    {
        [BindNever]
        public Guid ActivityId { get; set; }
    }

    public class Handler : IRequestHandler<Query, Activity>
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

        public async Task<Activity> Handle(Query query, CancellationToken cancellationToken)
        {
            using (_logger.BeginNamedScope(nameof(GetActivity),
                (Activity.FieldNames.Id, query.ActivityId)))
            {
                _logger.LogInformation("Starting to get single activity");

                var activity = await _repository.GetActivityByIdAsync(query.ActivityId, cancellationToken);

                if (activity != null)
                {
                    _logger.LogInformation("Activity: {Title} fetched successfully.", activity!.Title);
                }

                return activity;
            }
        }
    }
}
