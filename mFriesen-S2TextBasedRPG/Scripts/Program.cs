using SimpleLogger;
using System;

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
