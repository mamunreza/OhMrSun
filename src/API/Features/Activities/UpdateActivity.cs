using API.Domains.Activities;
using API.Infrastructure.Logging;
using API.Infrastructure.Notification;
using AutoMapper;
using MediatR;

namespace API.Features.Activities;

public class UpdateActivity
{
    public record Command : IRequest<ActivitiesBaseResult>
    {
        //[BindNever]
        public Guid ActivityId { get; set; }
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
        private readonly IAzureServiceBusPublisher _azureServiceBusPublisher;

        public Handler(IActivitiesRepository activitiesRepository,
            ILogger<Handler> logger,
            IMapper mapper,
            IAzureServiceBusPublisher azureServiceBusPublisher)
        {
            _activitiesRepository = activitiesRepository;
            _logger = logger;
            _mapper = mapper;
            _azureServiceBusPublisher = azureServiceBusPublisher;
        }

        public async Task<ActivitiesBaseResult> Handle(Command command, CancellationToken cancellationToken)
        {
            using (_logger.BeginNamedScope(nameof(Handler),
                (Activity.FieldNames.Title, command.Title)))
            {
                _logger.LogInformation("Starting to update activity");

                var activity = await _activitiesRepository.GetActivityByIdAsync(command.ActivityId, cancellationToken);

                if (activity == null)
                {
                    _logger.LogError("No activity found");
                    throw new BadHttpRequestException("No activity found");
                }

                activity.Title = command.Title;
                activity.ActivityDate = command.ActivityDate;
                activity.Description = command.Description;

                var updatedActivity = await _activitiesRepository.UpdateActivityAsync(activity, cancellationToken);

                _logger.LogInformation("updated activity with id: {0}", updatedActivity.Id);

                await _azureServiceBusPublisher.Publish("activity");

                return _mapper.Map<ActivitiesBaseResult>(updatedActivity);
            }
        }
    }
}
