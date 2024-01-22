using System.Collections.Generic;

namespace mFriesen_S2TextBasedRPG
{
    enum pickupType
    {
        item,
        instant
    }
    // To be used in Mod/Get stat.
    enum statname
    {
        hp,
        ap,
        dr
    }

    class Entity
    {
        // Stores entity data that isn’t player/foe/neutral specific.
        public Vector2 position;
        public List<Item> inventory;
        public int? armorInventoryIndex;
        public int? weaponInventoryIndex;

        int hp; // health
        int ap; // absorption
        int dr; // damage reduction

        // Set stats manually.
        public void ModStat(statname stat, int value)
        {
            switch (stat)
            {
                case statname.hp: hp = value; break;
                case statname.ap: ap = value; break;
                case statname.dr: dr = value; break;
            }
        }
        // Get method.
        public int GetStat(statname stat)
        {
            int Return = 0;
            switch (stat)
            {
                case statname.hp: Return = hp; break;
                case statname.ap: Return = ap; break;
                case statname.dr: Return = dr; break;
            }
            return Return;
        }
    }

    class Foe : Entity
    {
        // Foe specific things here, if any.
    }

    class Player : Entity
    {
        // Player specific things here.

        public Player()
        {
            ModStat(statname.hp, 10);
            ModStat(statname.ap, 10);
        }
    }

    class Pickup : Entity
    {
        pickupType type;
        public pickupType GetPickupType() { return type; }

        public Item item;
    }
}
