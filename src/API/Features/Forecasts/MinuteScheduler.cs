namespace API.Features.Forecasts;

public class MinuteScheduler
{
    private Timer _timer;
    private readonly ILogger<MinuteScheduler> _logger;

    public MinuteScheduler(ILogger<MinuteScheduler> logger)
    {
        _logger = logger;
        _timer = new Timer(OnTimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
    }

    private void OnTimerCallback(object state)
    {
        string lat = "59.40";
        string lon = "24.66";

        _logger.LogInformation("something is going on...");

        //_openWeatherService.GetOpenWeatherData(lat, lon);
    }

    // Clean up the timer
    public void Stop()
    {
        _timer?.Dispose();
    }
}