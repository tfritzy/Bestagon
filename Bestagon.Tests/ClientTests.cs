using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class ClientTests
{
    [TestMethod]
    public void Client_Constructor()
    {
        string id = Guid.NewGuid().ToString("N");
        Client client = new Client(id);
        Assert.AreEqual(id, client.Id);
    }

    [TestMethod]
    public void Client_TCPConnection()
    {
        Client client = new Client(Guid.NewGuid().ToString("N"));
        Assert.IsNotNull(client.TCPConnection);
        Assert.IsNotNull(client.TCPConnection.socket);
        Assert.AreEqual(TCPConnection.DATA_BUFFER_SIZE, client.TCPConnection.socket.SendBufferSize);
        Assert.AreEqual(TCPConnection.DATA_BUFFER_SIZE, client.TCPConnection.socket.ReceiveBufferSize);
        Assert.IsNotNull(client.TCPConnection.ReceiveBuffer);
        Assert.AreEqual(TCPConnection.DATA_BUFFER_SIZE, client.TCPConnection.ReceiveBuffer.Length);
        Assert.IsNotNull(client.TCPConnection.ReceivedData);
    }
}
