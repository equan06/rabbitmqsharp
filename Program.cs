const string queueName = "hello";

Console.WriteLine("Starting RabbitMQ Send/Receive demo");

Task.Run(() => { RabbitMQDemo.Receive.ReceiveMsg(queueName, 1); });
await Task.Delay(500);
Task.Run(() => { RabbitMQDemo.Receive.ReceiveMsg(queueName, 2); });
await Task.Delay(500);
Task.Run(() => { RabbitMQDemo.Receive.ReceiveMsg(queueName, 3); });
await Task.Delay(500);

for (int i = 0; i < 12; i++)
{
    await Task.Run(() => RabbitMQDemo.Send.PublishMsg(queueName, $"msg {i}"));
}
Console.ReadLine();




