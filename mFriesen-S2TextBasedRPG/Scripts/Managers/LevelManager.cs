using SimpleLogger;
using System;

namespace mFriesen_S2TextBasedRPG
{
    static class LevelManager
    {
        static Area[] areas;
        public static Area currentArea { get; private set; }
        static Trigger[] triggers;
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

            CheckTriggers(EntityManager.GetMobs());
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

        static void CheckTriggers(Mob[] mobs)
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
    }
}