using BancoBari.Core.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BancoBari.MessageBus.Test
{
    [TestClass]
    public class BusTest
    {
        [TestMethod]
        public void ConstructorEmpty_OK()
        {
            Bus bus = new Bus();

            Assert.IsTrue(string.IsNullOrEmpty(bus.HostName));
            Assert.IsTrue(string.IsNullOrEmpty(bus.KeyQueue));
        }

        [TestMethod]
        public void ConstructorFilled_OK()
        {
            Bus bus = new Bus("hostname", "key");

            Assert.IsFalse(string.IsNullOrEmpty(bus.HostName));
            Assert.IsFalse(string.IsNullOrEmpty(bus.KeyQueue));
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception), AllowDerivedTypes = true)]
        public void Receive_OK()
        {
            Bus bus = new Bus();

            bus.Receive();
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception), AllowDerivedTypes = true)]
        public void Send_OK()
        {
            Bus bus = new Bus();

            bus.Send(new Message());
        }
    }
}
