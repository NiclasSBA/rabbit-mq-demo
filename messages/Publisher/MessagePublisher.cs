using messages.MessageTypes;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace messages.Publisher
{
   public  class MessagePublisher<T> where T : BaseMessage
    {
        //there is an Iconnectionfactory interface if you really want to mess around with IOC
        private readonly ConnectionFactory _connectionFactory;

        public MessagePublisher(string hostName)
        {

            _connectionFactory = new ConnectionFactory() { HostName = hostName };
        }



        public void Publish(T message)
        {
            
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: true,
                                     arguments: null);
                channel.QueueDeclare(queue: "Skrrtskrrt",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                //channel.QueueDeclare();
                channel.ExchangeDeclare(exchange: "testExchange", type: "direct");
                channel.QueueBind("hello", "testExchange", routingKey: "testRouting");
                var body =  JsonSerializer.Serialize( message, message.GetType(), new JsonSerializerOptions());
               
                var content = Encoding.UTF8.GetBytes(body);
            
                channel.BasicPublish( exchange: "testExchange",
                                     routingKey: "testRouting",
                                     basicProperties: null,
                                     body: content);
               Console.WriteLine(" [x] Sent {0}", message.GetType());
            }
        }
    }
}
