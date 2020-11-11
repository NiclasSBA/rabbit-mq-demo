using messages.MessageTypes;
using messages.Publisher;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace messages.Subscriber
{
    public class MessageController
    {
        private readonly MessageSubscriber<TestMessage> _subscriber;


        private readonly MessagePublisher<TestMessage> _publisher;

        public MessageController()
        {
            _subscriber = new MessageSubscriber<TestMessage>("localhost");
            _publisher = new MessagePublisher<TestMessage>("localhost");
        }

        public async Task StartConsuming()
        {
            //so far we end up here, and have to set up message consuming correctly
           

            await _subscriber.SubcribeAsync(async (message) =>  await HandleMessage(message));
            

        }

        public async Task HandleMessage(TestMessage message)
        {
            await Task.Run(() => {


                Console.WriteLine(message.Payload);
               
            });
        }


        internal void TestPublish(TestMessage v)
        {
            
             _publisher.Publish(v);
        }
    }
}
