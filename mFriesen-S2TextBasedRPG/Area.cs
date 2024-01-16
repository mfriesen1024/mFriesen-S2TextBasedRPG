using System.IO;

namespace mFriesen_S2TextBasedRPG
{
    internal class Area
    {
        // Class stores everything for a certain room.
        string name; // use to determine what files to read to grab data.

        // File loading strings.
        string[] fNames;

        // Variables
        TriggerZone[] warpTriggers; // warp the player to a new area, based on the warp index.
        int[] warpIndexes; // use to determine where to warp the player.
        TriggerZone[] encounterTriggers; // launch encounter on trigger.
        int[] encounterIndexes; // use to determine what encounter to launch.
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

            // Get data from file
            string[] data = File.ReadAllLines(fNames[0]);
        }
    }
}
