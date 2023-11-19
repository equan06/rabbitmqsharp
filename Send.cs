using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;


namespace RabbitMQDemo
{
    public class Send
    {
        public static void PublishMsg(string queueName, string msg)
        {
            // specify hostname/port of rabbitmq node
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // create a queue called "hello". this is idempotent
            channel.QueueDeclare(queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: new Dictionary<string, object> { { "x-consumer-timeout", 30 * 1000 } });

            byte[] body = Encoding.UTF8.GetBytes(msg);

            channel.BasicPublish(exchange: string.Empty,
                routingKey: "hello",
                basicProperties: null,
                body: body);


            Console.WriteLine($"Sent: {msg}");

        }
    }
}



