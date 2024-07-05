using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
//using static API.Features.Activities.GetActivities;

namespace API.Features.Activities;

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

    [HttpPost("", Name = "createActivityAsync")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ActivitiesBaseResult))]
    public async Task<ActionResult<ActivitiesBaseResult>> CreateActivityAsync(
        CreateActivity.Command command,
        CancellationToken cancellationToken)
    {
        return Ok(await _sender.Send(command, cancellationToken));
    }
}
