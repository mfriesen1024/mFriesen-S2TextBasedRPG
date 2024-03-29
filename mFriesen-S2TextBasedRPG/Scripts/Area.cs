﻿namespace mFriesen_S2TextBasedRPG
{
    internal class Area
    {
        // Class stores everything for a certain room.
        string name; // use to determine what files to read to grab map.

        // Variables
        public Trigger[] triggers; // warp the player to a new area, based on the warp index.
        int[] warpIndexes; // use to determine where to warp the player.
        public Foe[] encounter;
        public Map map; // house map data.
        public Pickup[] pickups;

        public Area(string name)
        {
            // set name.
            this.name = name;

            // Entity loading here.

            // Create map
            map = new Map(name);
        }

        public void SetTriggers(Trigger[] triggers) { this.triggers = triggers; }
    }
}
