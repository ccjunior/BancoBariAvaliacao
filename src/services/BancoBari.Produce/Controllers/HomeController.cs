using BancoBari.Core.Messages;
using BancoBari.MessageBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BancoBari.Produce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IBus _bus;
        public HomeController(ILogger<HomeController> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        public IActionResult Index()
        {
            

            Message message = new Message(Guid.NewGuid(), "Hello World!");
            _bus.Initialize("localhost", "BariQuee");
            _bus.Send(message);

            //var result = new List<Message>();
            //result = MessageList.GetMessages().OrderByDescending(x => x.Date).ToList();

            return View(null);
        }
    }
}
