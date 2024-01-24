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
        public Vector2 position = new Vector2(0, 0);
        public Tile displayTile = new Tile();
        public List<Item> inventory;
        public StatManager statManager;
        public int? armorInventoryIndex;
        public int? weaponInventoryIndex;

        public Entity DeepClone()
        {
            Entity e = (Entity)MemberwiseClone();
            e.statManager = statManager.ShallowClone();
            e.position = position.Clone();
            e.displayTile = displayTile.Clone();
            foreach (Item item in inventory)
            {
                e.inventory.Add(item);
            }
            return e;
        }

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

        public abstract void GetMove(); // we'll use this to make the characters move separately.

        public int GetDamage()
        {
            int damage = 1; // base damage value. hard code it because damage should never be 0.
            if (weaponInventoryIndex != null)
            {
                damage += ((WeaponItem)inventory[(int)weaponInventoryIndex]).str;
            }
            damage += statManager.GetStat(statname.str);
            return damage;
        }

        public void TakeDamage(int damage)
        {
            int dr = 0; // base dr value. Hard code because it is again, a global standard value. Everything has 0 base defense.
            if (armorInventoryIndex != null)
            {
                dr += ((ArmorItem)inventory[(int)armorInventoryIndex]).dr;
            }
            dr += statManager.GetStat(statname.dr);

        }
    }

    class Foe : Entity
    {
        // Foe specific things here, if any.

        public Foe(int hp = 10, int ap = 0, int dr = 0, int str = 1)
        {
            statManager = new StatManager(hp, ap, dr, str);

            // set display char
            displayTile.fg = System.ConsoleColor.Red; displayTile.bg = System.ConsoleColor.DarkRed;
            displayTile.displayChar = 'E';
        }

        public override void GetMove() { }
    }

    class Player : Entity
    {
        // Player specific things here.

        public Player(int hp = 10, int ap = 0, int dr = 0, int str = 1)
        {
            statManager = new StatManager(hp, ap, dr, str);

            // set display char
            displayTile.fg = System.ConsoleColor.Blue; displayTile.bg = System.ConsoleColor.DarkBlue;
            displayTile.displayChar = '@';
        }

        public override void GetMove() { }
    }
}
