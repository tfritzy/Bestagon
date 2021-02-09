public class Client
{
    public TCPConnection TCPConnection;
    public string Id;

    public Client(string clientId)
    {
        this.Id = clientId;
        this.TCPConnection = new TCPConnection();
    }

    public void Connect(string ip, int port)
    {
        TCPConnection.Connect(ip, port);
    }
}