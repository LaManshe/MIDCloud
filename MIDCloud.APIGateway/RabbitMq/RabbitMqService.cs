using System.Diagnostics;
using System.Text;
using Ardalis.GuardClauses;
using MIDCloud.Shared.Models.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MIDCloud.APIGateway.RabbitMq;

public class RabbitMqService
{
    private readonly IModel _channel;

    public RabbitMqService()
    {
        var connectionFactory = new ConnectionFactory()
        {
            HostName = "localhost"
        };

        var connection = connectionFactory.CreateConnection();
        
        _channel = connection.CreateModel();
    }

    public void SubscribeToQueue(string name, Action<string> handler)
    {
        Guard.Against.NullOrEmpty(name, nameof(name));
        Guard.Against.Null(handler, nameof(handler));
        
        _channel.QueueDeclare(
            name, 
            durable: true, 
            exclusive: false, 
            autoDelete: false, 
            arguments: null);

        var consumer = new EventingBasicConsumer(_channel);
        
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            
            handler(message);
        };

        _channel.BasicConsume(name, autoAck: true, consumer);
    }

    public void Publish(string message, string queue)
    {
        _channel.QueueDeclare(queue: queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: "",
            routingKey: queue,
            basicProperties: null,
            body: body);
    }

    public void Publish(object messageObject, string queue)
    {
        var message = JsonConvert.SerializeObject(messageObject);
        
        Publish(message, queue);
    }

    public void Dispose()
    {
        _channel.Dispose();
    }
}