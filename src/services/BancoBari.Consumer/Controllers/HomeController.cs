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

            try
            {
                _bus.Initialize("localhost", "BariQuee");
                _bus.Receive();
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
           
             var result = new List<Message>();
             result = MessageList.GetMessages().OrderByDescending(x => x.Date).ToList();
            return View(null);
        }
    }
}
