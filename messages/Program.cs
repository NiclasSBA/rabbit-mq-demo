using messages.MessageTypes;
using messages.Subscriber;
using System;
using System.Threading.Tasks;

namespace messages
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var messageController = new MessageController();
            
          for(var i = 0; i < 50; i++)
            {
            var message = new TestMessage("This is a test " + i);
            var task = Task.Run(() =>messageController.TestPublish(message));
                task.Wait();
            }
           await messageController.StartConsuming();
        }
    }
}
