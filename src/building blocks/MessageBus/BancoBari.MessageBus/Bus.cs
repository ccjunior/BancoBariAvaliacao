using BancoBari.Core.Messages;
using EasyNetQ;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace BancoBari.MessageBus
{
    public sealed class Bus : IBus
    {

        public string HostName { get; private set; }
        public string KeyQueue { get; private set; }


        public void Initialize(string hostName, string keyQueue)
        {
            HostName = hostName;
            KeyQueue = keyQueue;
        }

        public Bus()
        {

        }

        public Bus(string hostName, string keyQueue)
        {
            Initialize(hostName, keyQueue);
        }

        public void Send(Message message)
        {

            var factory = new ConnectionFactory() { HostName = HostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: KeyQueue,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string strMessage = System.Text.Json.JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(strMessage);

                channel.BasicPublish(exchange: "",
                                     routingKey: KeyQueue,
                                     basicProperties: null,
                                     body: body);

                MessageList.Add(message);

                Console.WriteLine($"## Sent ## ServiceID: {message.ServiceId} | ID:{message.Id} | Message: {message.MessageText} | Date: {message.Date}", strMessage);
            }

        }
        public void Receive()
        {
            var factory = new ConnectionFactory() { HostName = HostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: KeyQueue,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                        var body = ea.Body.ToArray();
                        var strMessage = Encoding.UTF8.GetString(body);
                        Message message = System.Text.Json.JsonSerializer.Deserialize<Message>(strMessage);
                        MessageList.Add(message);
                    Console.WriteLine($"## Received ## ServiceID: {message.ServiceId} | ID:{message.Id} | Message: {message.MessageText} | Date: {message.Date}", strMessage);
                };
                channel.BasicConsume(queue: KeyQueue,
                                     autoAck: true,
                                     consumer: consumer);

             
            }
        }
    }
}
