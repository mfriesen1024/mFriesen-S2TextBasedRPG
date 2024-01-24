using SimpleLogger;
using System;
using System.Collections.Generic;
using System.IO;

namespace mFriesen_S2TextBasedRPG
{
    static class GameManager
    {
        public static Player player = new Player();

        public static string areasFName;
        static Area[] areas; // stores the areas.
        public static Foe[] foeTemplates; // store foe templates
        static Area currentArea; // tracks the current area
        static Map currentMap; // Track current map.
        public static List<Entity> entities = new List<Entity>();
        static string[] storedDialogue; // store dialogue (load from file)
        static string[] currentDialogue; // store the current dialogue passage to read

        public static void LoadAreas()
        {
            if (File.Exists(areasFName))
            {
                string[] aNames = File.ReadAllLines(areasFName);
                areas = new Area[aNames.Length];
                for (int i = 0; i < aNames.Length; i++)
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
            LoadData();

            // set first area active.
            LoadArea(0);

        }

        public static void Update()
        {


            // At the end, render the map.
            currentMap.RenderMap(entities.ToArray());
        }

        public static void LoadArea(int index)
        {
            currentArea = areas[index];
            entities = new List<Entity> { player };
            entities.AddRange(currentArea.encounter);
            currentMap = currentArea.map;
        }

        static void LoadData()
        {
            // eventually, this should be replaced with proper data loading, but for now, just load temporary things.
            TemporaryDataManager.GenerateThings();
            foeTemplates = TemporaryDataManager.foes.ToArray();
            areas = TemporaryDataManager.areas.ToArray();
        }
    }
}
