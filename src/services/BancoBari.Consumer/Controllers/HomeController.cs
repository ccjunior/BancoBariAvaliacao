using BancoBari.Core.Messages;
using BancoBari.MessageBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace BancoBari.Consumer.Controllers
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
            _bus.Initialize("localhost", "BariQuee");
            _bus.Receive();

            var result = new List<Message>();
            result = MessageList.GetMessages().OrderByDescending(x => x.Date).ToList();
            return View(result);
        }
    }
}
