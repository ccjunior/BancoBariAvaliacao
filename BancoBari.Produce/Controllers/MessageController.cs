using BancoBari.Core.Messages;
using BancoBari.MessageBus;
using BancoBari.Produce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using RouteAttribute = Microsoft.AspNetCore.Components.RouteAttribute;

namespace BancoBari.Produce.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public MessageController(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send(MessageRequest request)
        {
            if (request != null)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var service = scope.ServiceProvider.GetService<IBus>();
                        var message = new Message(request.ServiceId, request.MessageText);
                        service.Send(message);
                    }

                    return Ok();
                }
                catch
                {
                    return BadRequest();
                }
            }
            return BadRequest();

        }
    }
}
