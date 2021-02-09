using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class PacketTests
{
    [TestMethod]
    public void Packet_Constructor()
    {
        Packet packet1 = new Packet();
        Assert.IsNotNull(packet1.Buffer);
        Assert.AreEqual(Constants.DEFAULT_BUFFER_SIZE, packet1.Buffer.Length);
        Assert.AreEqual(0, packet1.ReadPosition);
    }

    [TestMethod]
    public void Packet_SetBytes()
    {
        Packet packet1 = new Packet();
        byte[] data = Encoding.ASCII.GetBytes("Hi I am a person from Earth");
        packet1.SetBytes(data);
        CompareArrayToBuffer(data, packet1.Buffer);
        Assert.AreEqual(0, packet1.ReadPosition);
        Assert.AreEqual(data.Length, packet1.Length);
    }

    [TestMethod]
    public void Packet_InsertData()
    {
        Packet packet1 = new Packet();
        byte[] expectedData = GetBytes("hello");
        Assert.AreEqual(0, packet1.Length);

        packet1.Append(expectedData);
        Assert.AreEqual(expectedData.Length, packet1.Length);
        CompareArrayToBuffer(expectedData, packet1.Buffer);

        packet1.Append(expectedData);
        expectedData = GetBytes("hellohello");
        Assert.AreEqual(expectedData.Length, packet1.Length);
        CompareArrayToBuffer(expectedData, packet1.Buffer);
        Assert.AreEqual(0, packet1.ReadPosition);
    }

    [TestMethod]
    public void Packet_ReadData()
    {
        Packet packet = new Packet();
        byte[] expectedData = GetBytes("1 2 3 4 5");
        packet.Append(expectedData);
        Assert.AreEqual(expectedData.Length, packet.Length);
        CompareArrayToBuffer(expectedData, packet.Buffer);

        CompareArrayToBuffer(GetBytes("1 2"), packet.ReadBytes(3));
        Assert.AreEqual(3, packet.ReadPosition);
        CompareArrayToBuffer(GetBytes(" 3 4"), packet.ReadBytes(4));
        Assert.AreEqual(7, packet.ReadPosition);
        CompareArrayToBuffer(GetBytes(" 5"), packet.ReadBytes(10));
        Assert.AreEqual(9, packet.ReadPosition);
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
