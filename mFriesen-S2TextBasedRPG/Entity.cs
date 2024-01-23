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

    abstract class Entity
    {
        // Stores entity data that isn’t player/foe/neutral specific.
        public Vector2 position;
        public Tile displayTile = new Tile();
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

        public Foe(int hp = 10, int ap = 0, int dr = 0)
        {
            ModStat(statname.hp, hp);
            ModStat(statname.ap, ap);
            ModStat(statname.dr, dr);

            // set display char
            displayTile.fg = System.ConsoleColor.Red; displayTile.bg = System.ConsoleColor.DarkRed;
            displayTile.displayChar = 'E';
        }
    }

    class Player : Entity
    {
        // Player specific things here.

        public Player(int hp = 10, int ap = 0, int dr = 0)
        {
            ModStat(statname.hp, hp);
            ModStat(statname.ap, ap);
            ModStat(statname.dr, dr);
        }
    }

    class Pickup : Entity
    {
        pickupType type;
        public pickupType GetPickupType() { return type; }

        public Item item;
    }
}
