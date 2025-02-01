public interface IRabbitMQService
{
    void PublishMessage(string routingKey, string message);
}
