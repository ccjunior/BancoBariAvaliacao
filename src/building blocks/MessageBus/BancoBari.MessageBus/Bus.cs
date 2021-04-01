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
            //Validate();

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
            //Validate();

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
                    try
                    {
                        var body = ea.Body.ToArray();
                        var strMessage = Encoding.UTF8.GetString(body);
                        Message message = System.Text.Json.JsonSerializer.Deserialize<Message>(strMessage);
                        MessageList.Add(message);

                        Console.WriteLine($"## Received ## ServiceID: {message.ServiceId} | ID:{message.Id} | Message: {message.MessageText} | Date: {message.Date}", strMessage);

                        System.Threading.Thread.Sleep(1000);

                        //channel.BasicAck(ea.DeliveryTag, false);
                    }
                    catch (Exception)
                    {
                        channel.BasicNack(ea.DeliveryTag, false, true);
                        throw;
                    }
                };
                channel.BasicConsume(queue: KeyQueue,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                //Console.ReadLine();
            }
        }



        //public void Receive()
        //{
        //    try
        //    {
        //        var factory = new ConnectionFactory()
        //        {
        //            HostName = HostName,
        //            UserName = "guest",
        //            Password = "guest",
        //            RequestedHeartbeat = TimeSpan.FromMinutes(3),
        //            Port = AmqpTcpEndpoint.UseDefaultPort
        //        };
        //        using (var connection = factory.CreateConnection())
        //        {
        //            using (var channel = connection.CreateModel())
        //            {
        //                while (channel.IsOpen)
        //                {
        //                    Thread.Sleep(1);
        //                    channel.QueueDeclare(queue: KeyQueue,
        //                      durable: false,
        //                      exclusive: false,
        //                      autoDelete: false,
        //                      arguments: null
        //                      );


        //                    var consumer = new EventingBasicConsumer(channel);

        //                    string message = "";

        //                    consumer.Received += (model, ea) =>
        //                    {
        //                        var body = ea.Body.ToArray();
        //                        message = Encoding.UTF8.GetString(body);
        //                        var obj = JsonConvert.DeserializeObject<Message>(message);

        //                        MessageList.Add(obj);
        //                    };

        //                    channel.BasicConsume(queue: KeyQueue,
        //                        autoAck: true,
        //                        consumer: consumer
        //                        );
        //                }
        //            }
        //            connection.Close();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e.Message);
        //        //Consumer();
        //    }
        //}

        //public void Send(Message message)
        //{
        //    try
        //    {
        //        if (message == null)
        //            message = new Message(Guid.NewGuid(), "Hello World!");


        //        var factory = new ConnectionFactory()
        //        {
        //            HostName = "localhost",
        //            UserName = "guest",
        //            Password = "guest",
        //            RequestedHeartbeat = TimeSpan.FromMinutes(3),
        //            Port = AmqpTcpEndpoint.UseDefaultPort
        //        };

        //        using (var connection = factory.CreateConnection())
        //        {
        //            using (var channel = connection.CreateModel())
        //            {
        //                channel.QueueDeclare(queue: "BancoBariQueue",
        //                    durable: false,
        //                    exclusive: false,
        //                    autoDelete: false,
        //                    arguments: null
        //                    );

        //                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

        //                channel.BasicPublish(exchange: "",
        //                    routingKey: "BancoBariQueue",
        //                    basicProperties: null,
        //                    body: body
        //                    );

        //                //MessageListModel.AddInList(message);
        //            }
        //            connection.Close();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e.Message);
        //        //AddQueue();
        //    }
        //}





        //private IBus _bus;
        //private IAdvancedBus _advancedBus;

        //private readonly string _connectionString;

        //public MessageBus(string connectionString)
        //{
        //    _connectionString = connectionString;
        //    TryConnect();
        //}

        //public bool IsConnected => _bus?.IsConnected ?? false;
        //public IAdvancedBus AdvancedBus => _bus?.Advanced;

        //public void Publish<T>(T message) where T : IntegrationEvent
        //{
        //    TryConnect();
        //    _bus.Publish(message);
        //}

        //public async Task PublishAsync<T>(T message) where T : IntegrationEvent
        //{
        //    TryConnect();
        //    await _bus.PublishAsync(message);
        //}

        //public void Subscribe<T>(string subscriptionId, Action<T> onMessage) where T : class
        //{
        //    TryConnect();
        //    _bus.Subscribe(subscriptionId, onMessage);
        //}

        //public void SubscribeAsync<T>(string subscriptionId, Func<T, Task> onMessage) where T : class
        //{
        //    TryConnect();
        //    _bus.SubscribeAsync(subscriptionId, onMessage);
        //}

        //public TResponse Request<TRequest, TResponse>(TRequest request) where TRequest : IntegrationEvent
        //    where TResponse : ResponseMessage
        //{
        //    TryConnect();
        //    return _bus.Request<TRequest, TResponse>(request);
        //}

        //public async Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request)
        //    where TRequest : IntegrationEvent where TResponse : ResponseMessage
        //{
        //    TryConnect();
        //    return await _bus.RequestAsync<TRequest, TResponse>(request);
        //}

        //public IDisposable Respond<TRequest, TResponse>(Func<TRequest, TResponse> responder)
        //    where TRequest : IntegrationEvent where TResponse : ResponseMessage
        //{
        //    TryConnect();
        //    return _bus.Respond(responder);
        //}

        //public IDisposable RespondAsync<TRequest, TResponse>(Func<TRequest, Task<TResponse>> responder)
        //    where TRequest : IntegrationEvent where TResponse : ResponseMessage
        //{
        //    TryConnect();
        //    return _bus.RespondAsync(responder);
        //}

        //private void TryConnect()
        //{
        //    if (IsConnected) return;

        //    var policy = Policy.Handle<EasyNetQException>()
        //        .Or<BrokerUnreachableException>()
        //        .WaitAndRetry(3, retryAttempt =>
        //            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        //    policy.Execute(() =>
        //    {
        //        _bus = RabbitHutch.CreateBus(_connectionString);
        //        _advancedBus = _bus.Advanced;
        //        _advancedBus.Disconnected += OnDisconnect;
        //    });
        //}

        //private void OnDisconnect(object s, EventArgs e)
        //{
        //    var policy = Policy.Handle<EasyNetQException>()
        //        .Or<BrokerUnreachableException>()
        //        .RetryForever();

        //    policy.Execute(TryConnect);
        //}

        //public void Dispose()
        //{
        //    _bus.Dispose();
        //}

    }
}
