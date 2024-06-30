using API.Features.Forecasts;

namespace API.Infrastructure.DI;

public static class Extensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IQueryRepository, QueryRepository>();
        services.AddScoped<ICommandRepository, CommandRepository>();
        services.AddScoped<IOpenWeatherService, OpenWeatherService>();

        services.AddSingleton<MinuteScheduler>();

        return services;
    }
}
