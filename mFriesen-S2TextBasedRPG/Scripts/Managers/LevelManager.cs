﻿using System;

namespace mFriesen_S2TextBasedRPG
{
    static class LevelManager
    {
        static Area[] areas;
        static public Area currentArea { get; private set; }
        static Map map;

        static public void Init() // Call from GameManager
        {
            areas = DataManager.areas.ToArray();

            // Load area once we've set everything up.
            LoadArea(0);
        }

        public static void Update()
        {
            map.RenderMap();
            EntityManager.Update();
        }

        public static void LoadArea(int index)
        {
            currentArea = areas[index];
            map = currentArea.map;
            GameManager.currentMap = map;
            EntityManager.LoadFromArea(currentArea);
        }

        internal static actionResult CheckLocation(Vector2 target)
        {
            try
            {
                Hazard hazard = map.HazardCheck(target);
                switch (hazard)
                {
                    case Hazard.wall: return actionResult.fail;
                    default: return actionResult.move;
                }
            }
            catch (IndexOutOfRangeException ignored) { return actionResult.fail; }
        }
    }
}