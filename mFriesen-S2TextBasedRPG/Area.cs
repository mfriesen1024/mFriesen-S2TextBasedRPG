using System.IO;

namespace mFriesen_S2TextBasedRPG
{
    internal class Area
    {
        // Class stores everything for a certain room.
        string name; // use to determine what files to read to grab data.
        string fName; // use to grab data for this area.
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
            this.fName = $"data\\areas\\{fName}.txt";
            Load();
        }

        public int CheckWarps()
        {
            // Later, this will have code to determine if to warp, and if so where.
            return 0;
        }

        void Load()
        {
            // Create map
            map = new Map(name);

            // Get data from file
            string[] data = File.ReadAllLines(fName);
        }
    }
}
