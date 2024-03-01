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
            player = GameManager.player;
            foes = new List<Foe>(area.encounter);
        }

        public static void Update() // Should update everything. Call from GameManager
        {
            player.Update();
            foreach (Foe foe in foes) { foe.Update(); }
        }

        internal static void DeleteItem(Entity entity)
        {
            if (entity is Pickup) { pickups.Remove((Pickup)entity); }
            if (entity is Foe) { foes.Remove((Foe)entity); }
        }

        internal static void CheckCoords(Vector2 coords, out Pickup pickup, out Mob mob)
        {
            // Get mob list.
            List<Mob> mobs = new List<Mob> { player }; mobs.AddRange(foes);
            pickup = null; mob = null;

            foreach (Mob mob2 in foes)
            {
                if (mob2.position.Equals(coords)) { mob = mob2; return; }
            }
            foreach (Pickup pickup2 in pickups)
            {
                if (pickup2.position.Equals(coords)) { pickup = pickup2; return; }
            }
        }

        public static Entity[] GetDisplayEntities() // Returns all entities to display.
        {
            List<Entity> entities = new List<Entity>(foes) { player }; entities.AddRange(pickups); return entities.ToArray();
        }
    }
}
