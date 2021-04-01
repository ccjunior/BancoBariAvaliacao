using System;
using System.Collections.Generic;
using System.Text;

namespace BancoBari.Core.Messages
{
    public sealed class Message
    {
        public Guid Id { get; set; }
        public Guid ServiceId { get; set; }
        public string MessageText { get; set; }
        public DateTime Date { get; set; }

        public Message() : this(Guid.Empty, "")
        {
           
        }

        public Message(Guid serviceId, string message)
        {
            Id = Guid.NewGuid();
            ServiceId = serviceId;
            MessageText = message;
            Date = DateTime.UtcNow;
        }

       


    }
}
