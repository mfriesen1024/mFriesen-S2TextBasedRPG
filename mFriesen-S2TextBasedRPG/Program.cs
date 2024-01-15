using SimpleLogger;
using System;
using System.IO;

namespace mFriesen_S2TextBasedRPG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Log log = new Log();
            log.SetName("LogMain");

            if (!Directory.Exists("data"))
            {
                log.Write("Data folder doesn't exist!", logType.error);
                Directory.CreateDirectory("data");
                log.Write("Data folder created, yelling at player.");
                Console.WriteLine("You didn't have a data folder. We made a new one, but you'll need to download a campaign. Delete the isEmpty file when you're done.");
                Console.WriteLine("Press a key to exit.");
                File.Create("data\\isEmpty");
                Console.ReadKey();
                Environment.Exit(1);
            }
            if (File.Exists("data\\isEmpty"))
            {
                Environment.Exit(1);
            }
        }
    }
}
