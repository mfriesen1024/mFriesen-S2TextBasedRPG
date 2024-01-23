using SimpleLogger;
using System;
using System.IO;

namespace mFriesen_S2TextBasedRPG
{
    static class GameManager
    {
        // Temporarily load encounter 1 automatically for firstplayable
        static bool autoRunFirstEncounter;

        public static string areasFName;
        static Area[] areas; // stores the areas. (to be loaded from a file)
        static Area currentArea; // tracks the current area
        static Encounter currentEncounter;
        public static Encounter CurrentEncounter { set { currentEncounter = value; } }
        static string[] storedDialogue; // store dialogue (load from file)
        static string[] currentDialogue; // store the current dialogue passage to read

        public static void LoadAreas()
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
            else
            {
                File.Create(areasFName).Close();
                Log.Write($"Areas file didn't exist. Is {areasFName} correct?", logType.error);
                Environment.Exit(-1);
            }
        }

        public static void Start()
        {
            // set first area active, and if we want to start first encounter, we do so.
            currentArea = areas[0];
            if(autoRunFirstEncounter)
            {
                currentArea.ActivateEncounter(0);
            }
        }

        public static void Update()
        {

        }
    }
}
