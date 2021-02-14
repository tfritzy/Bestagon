using System;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        Server server = new Server(8080, 2);

        Thread mainThread = new Thread(new ThreadStart(MainThread));
        Thread updateThread = new Thread(new ThreadStart(server.Update));
        mainThread.Start();
        updateThread.Start();

        server.Start();
    }

    private static void MainThread()
    {
        DateTime _nextLoop = DateTime.Now;

        while (true)
        {
            while (_nextLoop < DateTime.Now)
            {
                // GameLogic.Update(); // Execute game logic

                _nextLoop = _nextLoop.AddMilliseconds(Constants.MS_Between_Updates);

                if (_nextLoop > DateTime.Now)
                {
                    Thread.Sleep(_nextLoop - DateTime.Now); // Let the thread sleep until it's needed again.
                }
            }
        }
    }
}

