namespace mFriesen_S2TextBasedRPG
{
    internal class GameManager
    {
        Area[] areas; // stores the areas. (to be loaded from a file)
        Area currentArea; // tracks the current area
        Encounter[] encounters; // store encounters (load from file)
        Encounter currentEncounter;
        string[] storedDialogue; // store dialogue (load from file)
        string[] currentDialogue; // store the current dialogue passage to read

    }
}
