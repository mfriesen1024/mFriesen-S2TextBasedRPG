using SimpleLogger;
using System;
using System.IO;

namespace mFriesen_S2TextBasedRPG
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Log.SetName("LogMain");
            Log.debug = true;

            GameManager.areasFName = "data\\areaNames.txt";
            GameManager.Start();

            Console.ReadKey();
        }
    }
}
