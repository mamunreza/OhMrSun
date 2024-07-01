using API.Domains.Activities;
using API.Infrastructure.Logging;
using AutoMapper;
using MediatR;

namespace API.Features.Activities;

public class CreateActivity
{
    public record Command : IRequest<ActivitiesBaseResult>
    {
        /// <summary> Activity title </summary>
        /// <example>Title</example>
        public required string Title { get; set; }
        /// <summary> Activity date </summary>
        /// <example>2024-01-18T10:13:46Z</example>
        public DateTime? ActivityDate { get; set; }
        /// <summary> Description(content) of activity </summary>
        /// <example> Some interesting activity </example>
        public string? Description { get; set; }
    }

    public class Handler : IRequestHandler<Command, ActivitiesBaseResult>
    {
        private readonly IActivitiesRepository _activitiesRepository;
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;

        public Handler(IActivitiesRepository activitiesRepository,
            ILogger<Handler> logger,
            IMapper mapper)
        {
            _activitiesRepository = activitiesRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ActivitiesBaseResult> Handle(Command command, CancellationToken cancellationToken)
        {
            using (_logger.BeginNamedScope(nameof(Handler),
                (Activity.FieldNames.Id, command.Title)))
            {
                _logger.LogInformation("Starting to create activity");

                var activity = new Activity
                {
                    Title = command.Title,
                    ActivityDate = command.ActivityDate,
                    Description = command.Description
                };

                var createdActivity = await _activitiesRepository.CreateActivityAsync(activity, cancellationToken);

                _logger.LogInformation("Created activity with id: {0}", createdActivity.Id);

                return _mapper.Map<ActivitiesBaseResult>(createdActivity);
            }
        }
    }
}
