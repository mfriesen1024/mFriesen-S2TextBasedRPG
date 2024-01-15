namespace mFriesen_S2TextBasedRPG
{
    internal class Area
    {
        // Class stores everything for a certain room.
        string name; // use to determine what files to read to grab data.
        TriggerZone[] warpTriggers; // warp the player to a new area, based on the warp index.
        int[] warpIndexes; // use to determine where to warp the player.
        TriggerZone[] encounterTriggers; // launch encounter on trigger.
        int[] encounterIndexes; // use to determine what encounter to launch.
        Encounter[] encounters;
        //Map map; // house map data.

    }
}
