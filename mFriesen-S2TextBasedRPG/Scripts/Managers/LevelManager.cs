using System;

namespace mFriesen_S2TextBasedRPG
{
    static class LevelManager
    {
        static Area[] areas;
        static public Area currentArea { get; private set; }
        static Map map;

        static public void Init() // Call from GameManager
        {

        }

        public static void LoadArea(int index)
        {
            currentArea = areas[index];
            map = currentArea.map;
            GameManager.currentMap = map;
        }

        internal static actionResult CheckLocation(Vector2 target)
        {
            Hazard hazard = map.HazardCheck(target);
            switch (hazard)
            {
                case Hazard.wall: return actionResult.fail;
                default: return actionResult.move;
            }
        }
    }
}