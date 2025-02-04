public interface IRabbitMQService
{
    void PublishMessage(string routingKey, string message);
     Task PublishConfigUpdateAsync(string projectName, Dictionary<string, string> newConfig);
}
