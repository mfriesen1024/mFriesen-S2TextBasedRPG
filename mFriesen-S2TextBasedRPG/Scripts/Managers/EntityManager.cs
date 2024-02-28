using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mFriesen_S2TextBasedRPG.Scripts.Managers
{
    internal static class EntityManager
    {
        static Pickup[] pickups;
        public static Player player;
        static Foe[] foes;

        public static void LoadFromArea(Area area) // Call this from levelmanager.
        {
            pickups = area.pickups;
            player = GameManager.player;
            foes = area.encounter.ToArray();
        }

        public static void Update() // Should update everything. Call from GameManager
        {

        }
    }
}
