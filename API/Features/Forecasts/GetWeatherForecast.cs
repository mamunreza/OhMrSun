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

        public Handler(
            ILogger<Handler> logger)
        {
            _logger = logger;
        }

        public async Task<WeatherForecastResult> Handle(Query query, CancellationToken cancellationToken)
        {
            using (_logger.BeginNamedScope(nameof(GetWeatherForecast),
                ("key", "value")))
            {
                _logger.LogInformation("Starting to get weather data");

                // implement logic here

                _logger.LogInformation("Weather {id} fetched successfully.", "id");

                return new WeatherForecastResult();
            }
        }
    }
}
