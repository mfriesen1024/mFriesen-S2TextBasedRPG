using System.Collections.Generic;

namespace mFriesen_S2TextBasedRPG
{
    enum pickupType
    {
        item,
        instant
    }

    abstract class Entity
    {
        // Stores entity data that isn’t player/foe/neutral specific.
        public Vector2 position;
        public Tile displayTile = new Tile();
        public List<Item> inventory;
        public StatManager statManager;
        public int? armorInventoryIndex;
        public int? weaponInventoryIndex;

        int hp; // health
        int ap; // absorption
        int dr; // damage reduction

        // Set stats manually.
        public void ModStat(statname stat, int value)
        {
            statManager.ModStat(stat, value);
        }
        // Get method.
        public int GetStat(statname stat)
        {
            return statManager.GetStat(stat);
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

            // set display char
            displayTile.fg = System.ConsoleColor.Blue; displayTile.bg= System.ConsoleColor.DarkBlue;
            displayTile.displayChar = '@';
        }
    }

    class Pickup : Entity
    {
        pickupType type;
        public pickupType GetPickupType() { return type; }

        public Item item;
    }
}
