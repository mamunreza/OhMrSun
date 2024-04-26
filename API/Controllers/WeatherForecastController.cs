using API.Features.Forecasts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ISender _sender;

    public WeatherForecastController(
        ILogger<WeatherForecastController> logger,
        ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<ActionResult<WeatherForecastResult>> Get(
        decimal latitude,
        decimal longitude,
        [FromQuery] GetWeatherForecast.Query query,
        CancellationToken cancellationToken)
    {
        query.Latitude = latitude;
        query.Longitude = longitude;
        return Ok(await _sender.Send(query, cancellationToken));
    }
}


