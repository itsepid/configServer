public interface IRabbitMQService
{
    void PublishMessage(string routingKey, string message);
     Task PublishConfigUpdateAsync(Guid projectId, string environment, Dictionary<string, string> newConfig);
}
