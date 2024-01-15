using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mFriesen_S2TextBasedRPG
{
    internal class Entity
    {
        // Stores entity data that isn’t player/foe/neutral specific.
        public Vector2 position;
        public List<Item> inventory;
        int armorInventoryIndex;
        int weaponInventoryIndex;

        int hp; // health
        int ap; // absorption
        int dr; // damage reduction

    }
}
