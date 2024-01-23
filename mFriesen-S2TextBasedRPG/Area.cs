using SimpleLogger;
using System;
using System.IO;

namespace mFriesen_S2TextBasedRPG
{
    internal class Area
    {
        // Class stores everything for a certain room.
        string name; // use to determine what files to read to grab data.

        // File loading strings. THIS MUST BE KEPT UP TO DATE!
        string[] fNames = new string[5];

        // Variables
        TriggerZone[] warpTriggers; // warp the player to a new area, based on the warp index.
        int[] warpIndexes; // use to determine where to warp the player.
        TriggerZone[] encounterTriggers; // launch encounter on trigger.
        string[] encounterIndexes; // use to determine what encounter to launch.
        Encounter[] encounters;
        Map map; // house map data.
        Pickup[] pickups;

        public Area(string name)
        {
            // set name, and load misc files.
            this.name = name;
            string dir = $"data\\areas\\{name}";
            fNames[0] = dir + "data.txt";
            fNames[1] = dir + "wt.txt";
            fNames[2] = dir + "wi.txt";
            fNames[3] = dir + "et.txt";
            fNames[4] = dir + "ei.txt";
            // Create map
            map = new Map(name);

            Load();
        }

        public int CheckWarps()
        {
            // Later, this will have code to determine if to warp, and if so where.
            return 0;
        }

        void Load(int fIndex = 0)
        {
            bool exit = false;
            foreach(string file in fNames)
            {
                if (!File.Exists(file))
                {
                    Log.Write($"Failed to load file {file}, attempting creation.", logType.error); exit = true;
                    File.Create(file).Close();
                }
            }
            if (exit) { Environment.Exit(-1); }


            // Get data from file
            string[] data = File.ReadAllLines(fNames[0]);

            // Load encounter indexes, then load the encounter from the parsed string.
            string[] encounterIndexes = File.ReadAllLines(fNames[4]);
            encounters = new Encounter[encounterIndexes.Length];
            for (int i = 0; i < encounterIndexes.Length; i++)
            {
                encounters[i] = TemplateManager.encounters[int.Parse(encounterIndexes[i])].CreateEncounter();
            }

        }

        public void ActivateEncounter(int localIndex)
        {
            Foe[] tempFoes = encounters[localIndex].StartEncounter();
            Entity[] newFoes = new Entity[tempFoes.Length + 1];
            
            // Add player at index 0 instead of setting it to null
            

            for (int i = 0;i < tempFoes.Length;i++) { newFoes[i+1] = tempFoes[i]; }

        }
    }
}
