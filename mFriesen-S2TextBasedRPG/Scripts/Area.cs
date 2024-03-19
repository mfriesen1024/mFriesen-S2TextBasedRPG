using SimpleLogger;

namespace mFriesen_S2TextBasedRPG
{
    internal class Area
    {
        // Class stores everything for a certain room.
        string name; // use to determine what files to read to grab map.

        // Variables
        Trigger[] triggers; // warp the player to a new area, based on the warp index.
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

        public void CheckTriggers( Mob[] mobs)
        {
            foreach (Mob mob in mobs)
            {
                if (mob == null) { OnError(); return; }

                // Later, this will have code to determine if to warp, and if so where.
                foreach (Trigger trigger in triggers)
                {
                    trigger.CheckTrigger(mob);
                }

                void OnError() { Log.Write($"Unable to check the entity's warp!", logType.error); return; }

                return;
            }
        }

        public void SetTriggers(Trigger[] triggers) { this.triggers = triggers; }
    }
}
