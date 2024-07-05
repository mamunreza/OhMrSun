namespace API.Infrastructure.Notification;

public interface IAzureServiceBusPublisher
{
    Task<PublishResult> Publish(string message);
}

public class AzureServiceBusPublisher : IAzureServiceBusPublisher
{
    public async Task<PublishResult> Publish(string message)
    {
        await Task.CompletedTask;
        return new PublishResult();
    }
}
