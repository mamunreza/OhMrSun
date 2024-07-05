using API.Features.Activities;
using API.Features.Forecasts;
using API.Infrastructure.Notification;

namespace API.Infrastructure.DI;

public static class Extensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddScoped<IActivitiesRepository, ActivitiesRepository>();

        services.AddScoped<IQueryRepository, QueryRepository>();
        services.AddScoped<ICommandRepository, CommandRepository>();
        services.AddScoped<IOpenWeatherService, OpenWeatherService>();

        services.AddSingleton<MinuteScheduler>();

        return services;
    }

    public static IServiceCollection AddNotificationServices(this IServiceCollection services)
    {
        services.AddScoped<IAzureServiceBusPublisher, AzureServiceBusPublisher>();

        return services;
    }
}
