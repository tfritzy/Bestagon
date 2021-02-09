using Bestagons.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace core.tests
{
    [TestClass]
    public class ServerTests
    {
        [TestMethod]
        public void Server_TestPortAndPlayers()
        {
            Server server = new Server(8080, 4);
            Assert.AreEqual(4, server.MaxPlayers);
            Assert.AreEqual(8080, server.Port);
        }
    }
}
