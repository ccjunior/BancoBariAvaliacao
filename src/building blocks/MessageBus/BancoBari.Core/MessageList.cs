using System;
using System.Collections.Generic;
using System.Text;

namespace BancoBari.Core.Messages
{
    public class MessageList
    {
        public static List<Message> listMessage = new List<Message>();

        public static void Add(Message message)
        {
            listMessage.Add(message);
        }

        public static List<Message> GetMessages()
        {
            return listMessage;
        }
    }
}
