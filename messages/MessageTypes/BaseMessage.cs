using System;

namespace messages.MessageTypes
{
    public abstract class BaseMessage
    {
      
        public BaseMessage()
        {

        }
        public BaseMessage(string payload, MessageCategory category)
        {
            Payload = payload;
            EventTime = DateTime.UtcNow;
            Category = category;
        }

        //https://stackoverflow.com/questions/61869393/get-net-core-jsonserializer-to-serialize-private-members
        public string Payload { get; set; }
        public DateTime EventTime { get; set; }
        //TODO Find a way to handle this without allowing a setter
        public MessageCategory Category { get; set; }
    }
}