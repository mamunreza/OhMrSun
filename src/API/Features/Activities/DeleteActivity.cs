using API.Domains.Activities;
using API.Infrastructure.Logging;
using API.Infrastructure.Notification;
using AutoMapper;
using MediatR;

namespace API.Features.Activities;

public class DeleteActivity
{
    public record Command : IRequest<ActivitiesBaseResult>
    {
        //[BindNever]
        public Guid ActivityId { get; set; }
        /// <summary> Activity title </summary>
    }

    public class Handler : IRequestHandler<Command, ActivitiesBaseResult>
    {
        private readonly ILogger<Handler> _logger;
        private readonly IActivitiesRepository _activitiesRepository;
        private readonly IMapper _mapper;
        private readonly IAzureServiceBusPublisher _azureServiceBusPublisher;

        public Handler(
            ILogger<Handler> logger, 
            IActivitiesRepository activitiesRepository,
            IMapper mapper,
            IAzureServiceBusPublisher azureServiceBusPublisher)
        {
            _logger = logger;
            _activitiesRepository = activitiesRepository;
            _mapper = mapper;
            _azureServiceBusPublisher = azureServiceBusPublisher;
        }

        public async Task<ActivitiesBaseResult> Handle(Command command, CancellationToken cancellationToken)
        {
            using (_logger.BeginNamedScope(nameof(Handler),
                (Activity.FieldNames.Id, command.ActivityId)))
            {
                _logger.LogInformation("Starting to delete activity {ActivityId}", command.ActivityId);

                var activityToDelete = await _activitiesRepository.GetActivityByIdAsync(command.ActivityId, cancellationToken);

                if (activityToDelete == null)
                {
                    _logger.LogError("No activity found");
                    throw new BadHttpRequestException("No activity found");
                }

                _ = await _activitiesRepository.DeleteActivityAsync(activityToDelete, cancellationToken);

                _logger.LogInformation("Deleted activity with id: {0}", activityToDelete.Id);

                await _azureServiceBusPublisher.Publish("activity");

                return _mapper.Map<ActivitiesBaseResult>(activityToDelete);
            }
        }
    }
}
