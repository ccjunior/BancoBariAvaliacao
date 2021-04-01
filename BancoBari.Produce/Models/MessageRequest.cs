using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BancoBari.Produce.Models
{
    public class MessageRequest
    {
        public Guid ServiceId { get; set; }
        public string MessageText { get; set; }
    }
}
