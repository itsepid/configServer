using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ConfigServer.Infrastructure.Services{
public class RabbitMQService : IRabbitMQService
{
    private readonly string _hostName;
    private readonly string _userName;
    private readonly string _password;
    private readonly string _exchange;
    private readonly int _port;

    public RabbitMQService(IConfiguration configuration)
    {
        var rabbitMQConfig = configuration.GetSection("RabbitMQ");
        _hostName = rabbitMQConfig["HostName"];
        _userName = rabbitMQConfig["UserName"];
        _password = rabbitMQConfig["Password"];
        _exchange = rabbitMQConfig["Exchange"];
        _port = int.TryParse(rabbitMQConfig["Port"], out var port) ? port : 5672;
    }

public void PublishMessage(string routingKey, string message)
{
    var factory = new ConnectionFactory()
    {
        HostName = _hostName,
        UserName = _userName,
        Password = _password,
        Port = _port
    };

    using var connection = factory.CreateConnection();
    using var channel = connection.CreateModel();

    channel.ExchangeDeclare(exchange: _exchange, type: ExchangeType.Topic, durable: true);

    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(
        exchange: _exchange,
        routingKey: routingKey,  
        basicProperties: null,
        body: body
    );
}

public async Task PublishConfigUpdateAsync(Guid projectId, string environment, Dictionary<string, string> newConfig)
{
    var routingKey = $"config.{projectId}.{environment}";

    var message = JsonSerializer.Serialize(newConfig);
    var body = Encoding.UTF8.GetBytes(message);

    var factory = new ConnectionFactory()
    {
        HostName = _hostName,
        UserName = _userName,
        Password = _password,
        Port = _port
    };

    using var connection = factory.CreateConnection();
    using var channel = connection.CreateModel();
    var exchangeName = "config_updates";
    channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic, durable: true);

     // Ensure this exchange exists in RabbitMQ

    channel.BasicPublish(
        exchange: exchangeName,
        routingKey: routingKey,
        basicProperties: null,
        body: body
    );

Console.WriteLine($"The message is: {Encoding.UTF8.GetString(body)}");

    await Task.CompletedTask; // RabbitMQ publish is synchronous, but keeping async for interface compatibility
}


    }
}

