using System.Collections.Generic;

namespace mFriesen_S2TextBasedRPG
{
    enum pickupType
    {
        item,
        instant
    }
    internal class Entity
    {
        // Stores entity data that isn’t player/foe/neutral specific.
        public Vector2 position;
        public List<Item> inventory;
        public int? armorInventoryIndex;
        public int? weaponInventoryIndex;

        int hp; // health
        int ap; // absorption
        int dr; // damage reduction

    }

    class Foe : Entity
    {
        // Foe specific things here, if any.
    }

    class Player : Entity
    {
        // Player specific things here.
    }

    class Pickup : Entity
    {
        pickupType type;
        public pickupType GetPickupType() { return type; }

        public Item item;
    }
}
