using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using RabbitMQConsumer;

namespace RabbitMQConsumer;

public class RabbitMQOrderQueueConsumerPort : IOrderQueueConsumerPort
{
    private readonly IModel _channel;

    public RabbitMQOrderQueueConsumerPort(IModel channel)
    {
        _channel = channel;
        _channel.QueueDeclare("order-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    public string RecibirPedido()
    {
        var consumer = new EventingBasicConsumer(_channel);
        string receivedMessage = null;

        consumer.Received += (model, ea) =>
        {
            receivedMessage = Encoding.UTF8.GetString(ea.Body.ToArray());
            Console.WriteLine($"Pedido recibido de RabbitMQ: {receivedMessage}");
        };

        _channel.BasicConsume(queue: "order-queue", autoAck: true, consumer: consumer);

        return receivedMessage;
    }
}
