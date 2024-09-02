using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Features.Activities;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ActivitiesController : ControllerBase
{
    private readonly ISender _sender;

    public ActivitiesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("", Name = "getActivitiesAsync")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetActivities.Result))]
    public async Task<ActionResult<GetActivities.Result>> GetActivitiesAsync(
        [FromQuery] GetActivities.Query query,
        CancellationToken cancellationToken)
    {
        return Ok(await _sender.Send(query, cancellationToken));
    }

    [HttpGet("{activityId:Guid}", Name = "getActivitiyAsync")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetActivities.Result))]
    public async Task<ActionResult<GetActivities.Result>> GetActivitiesAsync(
        Guid activityId,
        [FromQuery] GetActivity.Query query,
        CancellationToken cancellationToken)
    {
        query.ActivityId = activityId;
        return Ok(await _sender.Send(query, cancellationToken));
    }

    [HttpPost("", Name = "createActivityAsync")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ActivitiesBaseResult))]
    public async Task<ActionResult<ActivitiesBaseResult>> CreateActivityAsync(
        CreateActivity.Command command,
        CancellationToken cancellationToken)
    {
        return Ok(await _sender.Send(command, cancellationToken));
    }

    [HttpPatch("{activityId:Guid}", Name = "updateActivityAsync")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ActivitiesBaseResult))]
    public async Task<ActionResult<ActivitiesBaseResult>> UpdateActivityAsync(
        Guid activityId,
        UpdateActivity.Command command,
        CancellationToken cancellationToken)
    {
        command.ActivityId = activityId;
        return Ok(await _sender.Send(command, cancellationToken));
    }

    [HttpDelete("{activityId:Guid}", Name = "deleteActivityAsync")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ActivitiesBaseResult))]
    public async Task<ActionResult<ActivitiesBaseResult>> DeleteActivityAsync(
        Guid activityId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteActivity.Command { ActivityId = activityId };

        command.ActivityId = activityId;
        return Ok(await _sender.Send(command, cancellationToken));
    }
}
