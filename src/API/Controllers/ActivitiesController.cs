using API.Features.Activities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class ActivitiesController : ControllerBase
{
    private readonly ISender _sender;

    public ActivitiesController(ISender sender)
    {
        _sender = sender;
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
