using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mFriesen_S2TextBasedRPG
{
    static class EntityManager
    {
        static List<Pickup> pickups;
        public static Player player;
        static List<Foe> foes;

        public static void LoadFromArea(Area area) // Call this from levelmanager.
        {
            pickups = new List<Pickup>( area.pickups);
            player = GameManager.player;
            foes = new List<Foe>(area.encounter);
        }

        public static void Update() // Should update everything. Call from GameManager
        {

        }

        internal static Pickup PickupCheck(Vector2 target)
        {
            throw new NotImplementedException();
        }

        internal static void DeleteItem(Entity entity)
        {
            throw new NotImplementedException();
        }

        internal static void CheckCoords(Vector2 coords, out Pickup pickup, out Mob mob)
        {
        }
    }
}
