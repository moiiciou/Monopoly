

using System;

namespace server
{

    class MainClass
    {
       public static GameServer startIt;
        public static void Main(string[] args)
        {
            startIt = new GameServer();

            if (args.Length > 0)
            {
                if (args[0] == "-log" && args[1] == "true")
                {
                    startIt.useLogging = true;
                }
            }
            startIt.Start();
        }
    }
}
