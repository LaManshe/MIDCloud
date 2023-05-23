namespace MIDCloud.Shared.Models.Interfaces.RabbitMq;

public interface IMessageTransit : IDisposable
{
    IMessageTransit GetInstance();
    
    void SubscribeToQueue(string name, Action<string> handler);
    
    void Publish(string message, string queue);

    void Publish(object messageObject, string queue);
}