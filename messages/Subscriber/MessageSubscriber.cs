using messages.MessageTypes;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace messages.Subscriber
{
    public class MessageSubscriber<T> where T : BaseMessage
    {
        private readonly ConnectionFactory _factory;
        private readonly string _hostName;
        private readonly IModel _channel;
        private readonly AsyncEventingBasicConsumer _consumer;
        //private readonly IConnection _connection;
        public MessageSubscriber(string hostName)
        {
            _factory = new ConnectionFactory() { HostName = hostName, DispatchConsumersAsync = true };
            _hostName = hostName;
            //_connection = _factory.CreateConnection();

            //_channel = _connection.CreateModel();

            //channel.QueueDeclare(queue: "hello",
            //                 durable: false,
            //                 exclusive: false,
            //                 autoDelete: false,
            //                 arguments: null);

            _consumer = new AsyncEventingBasicConsumer(_channel);

            //_consumer.Received += Message_Received;

            //_channel.BasicConsume("hello", false, _consumer);
        }

        //it doesnt make sense to make this
        public async Task SubcribeAsync(Action<T> funcToRun)
        {

            var factory = new ConnectionFactory() { HostName = _hostName, DispatchConsumersAsync = true };

            //in case anything bad happens to the connection, we want the system to do all the setup again.
            factory.AutomaticRecoveryEnabled = true;
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            //each consumer gets a seperate prefetch count, this is visible in the UI
            //by changing the prefetch count, we can see each consumer never in sequence
            channel.BasicQos(0,2,  false);
            //right now i believe this receives all messages
            channel.QueueDeclare(queue: "hello", exclusive: false, autoDelete: true);

           
            for (int i = 0; i < 5; i++)
            {
                var consumer = new AsyncEventingBasicConsumer(channel);
                //var consumer = new AsyncEventingBasicConsumer(channel);
                var time = new Stopwatch();
                var guid = Guid.NewGuid();
                consumer.Received += async (o, a) =>
              {
                time.Start();
                  Console.WriteLine("Delivery: " + a.DeliveryTag);
                  Console.WriteLine("Consumer Guid: " + guid);
                  Console.WriteLine("time: " + time.Elapsed.TotalSeconds);
                  var test = Encoding.UTF8.GetString(a.Body.ToArray());
                  var message = JsonSerializer.Deserialize<T>(test, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                  ////Thread.Sleep(5000);
               
                      await Task.Delay(1000);
                      funcToRun(message);
              
                  
                  time.Stop();
                  //await Task.Yield();
              //We ack the message as part of the event which fires when a message is received
              channel.BasicAck(a.DeliveryTag, false);
              };

                //We explicitly say no to the auto-ack feature, since it messes with prefetch count
               
                    channel.BasicConsume("hello", false, consumer);
               
                //channel.BasicConsume("hello", false, consumer);


            }
           
            Console.ReadKey();

        }
        static async Task Message_Received(object sender, BasicDeliverEventArgs @event)
        {

            var body = Encoding.UTF8.GetString(@event.Body.ToArray(), 0, @event.Body.Length);
            Console.WriteLine("Message Get" + @event.DeliveryTag + body);

            await Task.Delay(250);
            //await Task.Yield();

        }
    }
}
