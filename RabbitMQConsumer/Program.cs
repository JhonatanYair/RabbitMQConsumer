using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using RabbitMQConsumer;

class Program
{
    static void Main(string[] args)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost", // Cambia por la dirección de tu servidor RabbitMQ
            UserName = "guest",    // Cambia por tu nombre de usuario
            Password = "guest"     // Cambia por tu contraseña
        };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            var orderQueueConsumer = new RabbitMQOrderQueueConsumerPort(channel);

            // Declara la cola (si no existe)
            channel.QueueDeclare("order-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            Console.WriteLine("Esperando pedidos. Presiona [ENTER] para salir.");

            // Configura el consumidor para manejar mensajes entrantes
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"Mensaje recibido: {message}");
            };

            // Comienza a consumir mensajes de la cola
            channel.BasicConsume(queue: "order-queue", autoAck: true, consumer: consumer);

            // Mantén la aplicación en ejecución indefinidamente
            Console.ReadLine();
        }
    }
}
