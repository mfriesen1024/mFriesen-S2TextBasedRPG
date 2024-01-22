using System.IO;

namespace mFriesen_S2TextBasedRPG
{
    static class GameManager
    {
        // Temporarily load encounter 1 automatically for firstplayable
        static bool autoRunFirstEncounter;

        static string areasFName;
        static Area[] areas; // stores the areas. (to be loaded from a file)
        static Area currentArea; // tracks the current area
        static Encounter currentEncounter;
        static string[] storedDialogue; // store dialogue (load from file)
        static string[] currentDialogue; // store the current dialogue passage to read

        static void LoadAreas()
        {
            if(File.Exists(areasFName))
            {
                string[] aNames = File.ReadAllLines(areasFName);
                areas = new Area[aNames.Length];
                for(int i = 0; i < aNames.Length; i++)
                {
                    areas[i] = new Area(aNames[i]);
                }
            }
        }
    }
}
