using BancoBari.MessageBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BancoBari.Consumer.Models
{
    public class BackgroundServices : BackgroundService
    {
        //private readonly IServiceScopeFactory _scopeFactory;
        private IBus _bus;
        private Timer _timer;
        public BackgroundServices(IBus bus)
            //IServiceScopeFactory scopeFactory)
        {
            _bus = bus;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }

        //public Task StartAsync(CancellationToken cancellationToken)
        //{
        //    _timer = new Timer(DoWork, null, TimeSpan.Zero,
        //    TimeSpan.FromSeconds(10));

        //    return Task.CompletedTask;
        //}
        private void DoWork(object state)
        {

            _bus.Initialize("localhost", "BariQuee");
            _bus.Receive();
            //using (var scope = _scopeFactory.CreateScope())
            //{
            //    var service = scope.ServiceProvider.GetService<IBus>();
            //    service.Receive();
            //}
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
           TimeSpan.FromSeconds(10));

            return Task.CompletedTask;
        }
    }
}
