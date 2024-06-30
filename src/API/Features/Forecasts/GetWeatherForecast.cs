using API.Infrastructure.Logging;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Features.Forecasts;

public class GetWeatherForecast
{
    public record Query : IRequest<WeatherForecastResult>
    {
        [BindNever]
        public decimal Latitude { get; set; }
        [BindNever]
        public decimal Longitude { get; set; }
    }

    public class Handler : IRequestHandler<Query, WeatherForecastResult>
    {
        private readonly ILogger<Handler> _logger;
        private readonly IOpenWeatherService _service;

        public Handler(
            ILogger<Handler> logger, 
            IOpenWeatherService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task<WeatherForecastResult> Handle(Query query, CancellationToken cancellationToken)
        {
            using (_logger.BeginNamedScope(nameof(GetWeatherForecast),
                ("key", "value")))
            {
                _logger.LogInformation("Starting to get weather data");

                await _service.GetOpenWeatherData(query.Latitude, query.Longitude);

                _logger.LogInformation("Weather {id} fetched successfully.", "id");

                return new WeatherForecastResult();
            }
        }
    }
}
