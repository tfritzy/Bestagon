using System;
using System.Text;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class PacketTests
{
    [TestMethod]
    public void Packet_Constructor()
    {
        Packet packet = new Packet();
        Assert.IsNotNull(packet.Buffer);
        Assert.AreEqual(Constants.DEFAULT_BUFFER_SIZE, packet.Buffer.Length);
    }

    [TestMethod]
    public void Packet_InsertInvalidData()
    {
        Packet packet = new Packet();
        byte[] message = GetBytes("Test Message");
        Assert.IsNull(packet.Message);
    }

    [TestMethod]
    public void Packet_InsertProtobuf()
    {
        Packet packet = new Packet();
        string username = "Tyler";
        PlayerJoinedGame message = new PlayerJoinedGame() { Username = username };
        Any any = Any.Pack(message);
        byte[] bytes = any.ToByteArray();
        packet.SetContents(bytes, bytes.Length);
        Assert.AreEqual(Constants.DEFAULT_BUFFER_SIZE, packet.Buffer.Length);
        CompareArrayToBuffer(any.ToByteArray(), packet.Buffer);

        Assert.IsTrue(packet.Message.Is(PlayerJoinedGame.Descriptor));
        Assert.AreEqual(username, packet.Message.Unpack<PlayerJoinedGame>().Username);
    }

    private byte[] GetBytes(string s)
    {
        return Encoding.ASCII.GetBytes(s);
    }

    private void CompareArrayToBuffer(byte[] array, byte[] buffer)
    {
        Assert.IsFalse(array.Length > buffer.Length, "The array is larger than the buffer");

        for (int i = 0; i < array.Length; i++)
        {
            Assert.AreEqual(array[i], buffer[i]);
        }
    }
}
