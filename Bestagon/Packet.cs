using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

public class Packet : IDisposable
{
    private byte[] buffer;
    private int readPosition;
    private int messageLength;

    /// <summary>Creates a new empty packet (without an ID).</summary>
    public Packet()
    {
        buffer = new byte[Constants.DEFAULT_BUFFER_SIZE];
        readPosition = 0;
    }

    public byte[] Buffer { get { return buffer; } }
    public int ReadPosition { get { return readPosition; } }
    public int Length { get { return messageLength; } }

    /// <summary>Sets the packet's content and prepares it to be read.</summary>
    /// <param name="_data">The bytes to add to the packet.</param>
    public void SetBytes(byte[] _data)
    {
        Append(_data);
    }

    /// <summary>Gets the length of the unread data contained in the packet.</summary>
    public int UnreadLength()
    {
        return Length - readPosition; // Return the remaining length (unread)
    }

    public void Reset()
    {
        readPosition = 0; // Reset readPos
    }

    public void Append(byte[] value)
    {
        Array.Copy(value, 0, buffer, Length, value.Length);
        messageLength += value.Length;
    }

    /// <summary>Adds a short to the packet.</summary>
    /// <param name="_value">The short to add.</param>
    public void Append(short _value)
    {
        Append(BitConverter.GetBytes(_value));
    }

    /// <summary>Adds an int to the packet.</summary>
    /// <param name="_value">The int to add.</param>
    public void Append(int _value)
    {
        Append(BitConverter.GetBytes(_value));
    }

    /// <summary>Adds a long to the packet.</summary>
    /// <param name="_value">The long to add.</param>
    public void Append(long _value)
    {
        Append(BitConverter.GetBytes(_value));
    }

    /// <summary>Adds a float to the packet.</summary>
    /// <param name="_value">The float to add.</param>
    public void Append(float _value)
    {
        Append(BitConverter.GetBytes(_value));
    }

    /// <summary>Adds a bool to the packet.</summary>
    /// <param name="_value">The bool to add.</param>
    public void Append(bool _value)
    {
        Append(BitConverter.GetBytes(_value));
    }

    /// <summary>Adds a string to the packet.</summary>
    /// <param name="_value">The string to add.</param>
    public void Append(string _value)
    {
        Append(Encoding.ASCII.GetBytes(_value)); // Add the string itself
    }

    /// <summary>Adds a Vector3 to the packet.</summary>
    /// <param name="_value">The Vector3 to add.</param>
    public void Append(Vector3 _value)
    {
        Append(_value.X);
        Append(_value.Y);
        Append(_value.Z);
    }

    /// <summary>Adds a Quaternion to the packet.</summary>
    /// <param name="_value">The Quaternion to add.</param>
    public void Append(Quaternion _value)
    {
        Append(_value.X);
        Append(_value.Y);
        Append(_value.Z);
        Append(_value.W);
    }

    /// <summary>Reads a byte from the packet.</summary>
    /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
    public byte ReadByte(bool _moveReadPos = true)
    {
        if (messageLength > readPosition)
        {
            // If there are unread bytes
            byte _value = buffer[readPosition]; // Get the byte at readPos' position
            if (_moveReadPos)
            {
                // If _moveReadPos is true
                readPosition += 1; // Increase readPos by 1
            }
            return _value; // Return the byte
        }
        else
        {
            throw new Exception("Could not read value of type 'byte'!");
        }
    }

    /// <summary>
    /// Reads an array of bytes from the packet.
    /// </summary>
    public byte[] ReadBytes(int length, bool moveReadPos = true)
    {
        if (readPosition + length > messageLength)
        {
            length = Math.Max(0, messageLength - readPosition);
        }

        byte[] value = new byte[length];
        Array.Copy(buffer, readPosition, value, 0, length);

        if (moveReadPos)
        {
            readPosition += length;
        }
        return value;
    }

    /// <summary>Reads a short from the packet.</summary>
    /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
    public short ReadShort(bool _moveReadPos = true)
    {
        if (messageLength > readPosition)
        {
            // If there are unread bytes
            short _value = BitConverter.ToInt16(buffer, readPosition); // Convert the bytes to a short
            if (_moveReadPos)
            {
                // If _moveReadPos is true and there are unread bytes
                readPosition += 2; // Increase readPos by 2
            }
            return _value; // Return the short
        }
        else
        {
            throw new Exception("Could not read value of type 'short'!");
        }
    }

    /// <summary>Reads an int from the packet.</summary>
    /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
    public int ReadInt(bool _moveReadPos = true)
    {
        if (messageLength > readPosition)
        {
            // If there are unread bytes
            int _value = BitConverter.ToInt32(buffer, readPosition); // Convert the bytes to an int
            if (_moveReadPos)
            {
                // If _moveReadPos is true
                readPosition += 4; // Increase readPos by 4
            }
            return _value; // Return the int
        }
        else
        {
            throw new Exception("Could not read value of type 'int'!");
        }
    }

    /// <summary>Reads a long from the packet.</summary>
    /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
    public long ReadLong(bool _moveReadPos = true)
    {
        if (messageLength > readPosition)
        {
            // If there are unread bytes
            long _value = BitConverter.ToInt64(buffer, readPosition); // Convert the bytes to a long
            if (_moveReadPos)
            {
                // If _moveReadPos is true
                readPosition += 8; // Increase readPos by 8
            }
            return _value; // Return the long
        }
        else
        {
            throw new Exception("Could not read value of type 'long'!");
        }
    }

    /// <summary>Reads a float from the packet.</summary>
    /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
    public float ReadFloat(bool _moveReadPos = true)
    {
        if (messageLength > readPosition)
        {
            // If there are unread bytes
            float _value = BitConverter.ToSingle(buffer, readPosition); // Convert the bytes to a float
            if (_moveReadPos)
            {
                // If _moveReadPos is true
                readPosition += 4; // Increase readPos by 4
            }
            return _value; // Return the float
        }
        else
        {
            throw new Exception("Could not read value of type 'float'!");
        }
    }

    /// <summary>Reads a bool from the packet.</summary>
    /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
    public bool ReadBool(bool _moveReadPos = true)
    {
        if (messageLength > readPosition)
        {
            // If there are unread bytes
            bool _value = BitConverter.ToBoolean(buffer, readPosition); // Convert the bytes to a bool
            if (_moveReadPos)
            {
                // If _moveReadPos is true
                readPosition += 1; // Increase readPos by 1
            }
            return _value; // Return the bool
        }
        else
        {
            throw new Exception("Could not read value of type 'bool'!");
        }
    }

    /// <summary>Reads a string from the packet.</summary>
    /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
    public string ReadString(bool _moveReadPos = true)
    {
        try
        {
            int _length = ReadInt(); // Get the length of the string
            string _value = Encoding.ASCII.GetString(buffer, readPosition, _length); // Convert the bytes to a string
            if (_moveReadPos && _value.Length > 0)
            {
                // If _moveReadPos is true string is not empty
                readPosition += _length; // Increase readPos by the length of the string
            }
            return _value; // Return the string
        }
        catch
        {
            throw new Exception("Could not read value of type 'string'!");
        }
    }

    /// <summary>Reads a Vector3 from the packet.</summary>
    /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
    public Vector3 ReadVector3(bool _moveReadPos = true)
    {
        return new Vector3(ReadFloat(_moveReadPos), ReadFloat(_moveReadPos), ReadFloat(_moveReadPos));
    }

    /// <summary>Reads a Quaternion from the packet.</summary>
    /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
    public Quaternion ReadQuaternion(bool _moveReadPos = true)
    {
        return new Quaternion(ReadFloat(_moveReadPos), ReadFloat(_moveReadPos), ReadFloat(_moveReadPos), ReadFloat(_moveReadPos));
    }

    private bool disposed = false;

    protected virtual void Dispose(bool _disposing)
    {
        if (!disposed)
        {
            if (_disposing)
            {
                buffer = null;
                buffer = null;
                readPosition = 0;
            }

            disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
