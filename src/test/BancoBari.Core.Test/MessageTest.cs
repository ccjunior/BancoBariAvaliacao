using BancoBari.Core.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BancoBari.Core.Test
{
    [TestClass]
    public class MessageTest
    {
        [TestMethod]
        public void ConstructorEmpty_OK()
        {
            Message message = new Message();

            Assert.AreEqual(Guid.Empty, message.ServiceId);
            Assert.AreNotEqual(Guid.Empty, message.Id);
            Assert.IsTrue(string.IsNullOrEmpty(message.MessageText));
            Assert.AreNotEqual(new DateTime(), message.Date);
        }

        [TestMethod]
        public void ConstructorFilled_OK()
        {
            var guid = Guid.NewGuid();
            var msg = "informacao";

            Message message = new Message(guid, msg);

            Assert.AreEqual(guid, message.ServiceId);
            Assert.AreNotEqual(Guid.Empty, message.Id);
            Assert.AreEqual(msg, message.MessageText);
            Assert.AreNotEqual(new DateTime(), message.Date);

        }
    }
}
