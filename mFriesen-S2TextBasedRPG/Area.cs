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
        Trigger[] warpTriggers; // warp the player to a new area, based on the warp index.
        int[] warpIndexes; // use to determine where to warp the player.
        public Entity[] encounter;
        Map map; // house map data.

        public Area(string name)
        {
            // set name.
            this.name = name;
            
            // Entity loading here.
            
            // Create map
            map = new Map(name);
        }

        public int CheckWarps()
        {
            // Later, this will have code to determine if to warp, and if so where.
            return 0;
        }
    }
}
