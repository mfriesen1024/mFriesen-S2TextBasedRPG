using SimpleLogger;
using System.Collections.Generic;

namespace mFriesen_S2TextBasedRPG
{
    static class EntityManager
    {
        static List<Pickup> pickups;
        public static Player player;
        static List<Foe> foes;

        public static void LoadFromArea(Area area) // Call this from levelmanager.
        {
            pickups = new List<Pickup>(area.pickups);
            player = DataManager.player;
            foes = new List<Foe>(area.encounter);
        }

        public static void Update() // Should update everything. Call from GameManager
        {
            player.Update();
            foreach (Foe foe in foes.ToArray()) { foe.Update();
                if (foe.GetisDying()) { foes.Remove(foe); }
            }
            if (player.GetisDying()) { GameManager.run = false; }
        }

        internal static void DeleteItem(Entity entity)
        {
            if (entity is Pickup) { pickups.Remove((Pickup)entity); }
            if (entity is Foe) { foes.Remove((Foe)entity); }
        }

        internal static void CheckCoords(Vector2 coords, out Pickup pickup, out Mob mob)
        {
            Log.Write($"CheckCoords was called, coords are {coords}", logType.debug);

            // Get mob list.
            List<Mob> mobs = new List<Mob> { player }; mobs.AddRange(foes);
            pickup = null; mob = null;

            foreach (Mob mob2 in mobs)
            {
                bool debug = true;
                if (mob2.position.Equals(coords))
                {
                    mob = mob2;
                    Log.Write($"Mob coords found, coords are {mob.position}", logType.debug);
                }
            }
            foreach (Pickup pickup2 in pickups)
            {
                if (pickup2.position.Equals(coords))
                {
                    pickup = pickup2;
                    Log.Write($"Pickup coords found, coords are {pickup.position}", logType.debug);
                }
            }
        }

        public static Entity[] GetDisplayEntities() // Returns all entities to display.
        {
            List<Entity> entities = new List<Entity>(foes) { player }; entities.AddRange(pickups); return entities.ToArray();
        }
    }
}
