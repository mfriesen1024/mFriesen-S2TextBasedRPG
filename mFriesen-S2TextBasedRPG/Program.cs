﻿using SimpleLogger;
using System;
using System.IO;

namespace mFriesen_S2TextBasedRPG
{
    internal class Program
    {
        static string[] directories = { "data\\maps", "data\\areas", "data\\encounters", "data\\foes", "data\\items" };

        static void Main(string[] args)
        {
            Log.SetName("LogMain");

            if (!Directory.Exists("data"))
            {
                Log.Write("Data folder doesn't exist!", logType.error);
                Directory.CreateDirectory("data");
                foreach (string d in directories)
                {
                    Directory.CreateDirectory(d);
                }
                Log.Write("Data folder created, yelling at player.");
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

            GameManager.areasFName = "data\\areaNames.txt";
            GameManager.LoadAreas();

        }
    }
}
