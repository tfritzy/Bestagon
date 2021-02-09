namespace Bestagons.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Net;
    using System.Net.Sockets;
    public class Server
    {
        public Server(int port, int maxPlayers)
        {
            this.MaxPlayers = maxPlayers;
            this.Port = port;
        }

        public int Port { get; }
        public int MaxPlayers { get; }
    }
}
