using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQDemo
{
    public class Receive
    {
        public static void ReceiveMsg(string queueName, int id)
        {

            Random rand = new Random();
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: new Dictionary<string, object> { {  "x-consumer-timeout", 60 * 1000} });
            Console.WriteLine($"[*] Consumer {id} waiting for msg");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                string msg = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[x] Consumer {id} Received msg: {msg}");

                // Drop 50% of consumed messages before ack
                if (rand.NextDouble() < 0.5)
                {
                    Console.WriteLine($"[x] Consumer {id} dropped msg: {msg}");
                    return;
                }

                Console.WriteLine($"[x] Consumer {id} Processed msg: {msg}");
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };


            // Acknowledge the receipt
            channel.BasicConsume(queue: queueName,
                        autoAck: false,
                        consumer: consumer);


            Console.ReadLine();
        }
    }
}
