using System;
using Google.Protobuf.WellKnownTypes;

public class Packet
{
    private byte[] buffer;
    public Any Message;

    public Packet()
    {
        this.buffer = new byte[Constants.DEFAULT_BUFFER_SIZE];
    }

    public byte[] Buffer { get { return buffer; } }

    public void SetContents(byte[] buffer, int endIndex)
    {
        Array.Copy(buffer, this.buffer, endIndex);

        try
        {
            Message = Any.Parser.ParseFrom(buffer, 0, endIndex);
        }
        catch
        {
            Message = null;
        }
    }
}
