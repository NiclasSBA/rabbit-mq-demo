using messages.Subscriber;
using System;
using System.Collections.Generic;
using System.Text;

namespace messages.MessageTypes
{
    public class TestMessage : BaseMessage
    {
        public TestMessage() 
        {
                
        }
        public TestMessage(string payload) : base(payload, MessageCategory.Test)
        {
          
        }

    }
}
