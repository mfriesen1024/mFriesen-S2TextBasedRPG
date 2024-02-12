using SimpleLogger;
using System;
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
        public List<Item> inventory = new List<Item>();
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

        public abstract Vector2 GetAction(); // we'll use this to make the characters move separately.

        public int GetDamage()
        {
            int damage = 1; // base damage value. hard code it because damage should never be 0.
            if (weaponInventoryIndex != null)
            {
                damage += ((WeaponItem)inventory[(int)weaponInventoryIndex]).str;
            }
            damage += statManager.GetStat(statname.str);

            Log.Write($"Damage was requested. Got {damage}", logType.debug);
            
            return damage;
        }

        public void TakeDamage(int damage) // This should soon be depreciated.
        {
            int dr = GetArmorDR();

            dr += statManager.GetStat(statname.dr);
            statManager.TakeDamage(dr, damage);
        }

        private int GetArmorDR() // This should be called by the statmanager somehow.
        {
            int dr = 0; // base dr value. Hard code because it is again, a global standard value. Everything has 0 base defense.
            if (armorInventoryIndex != null)
            {
                dr += ((ArmorItem)inventory[(int)armorInventoryIndex]).dr;
            }
            return dr;
        }

        public void Heal(healtype type, int value)
        {
            // pass the command on to statmanager.
            statManager.Heal(type, value);
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

        public override Vector2 GetAction() // Goal is to randomly generate the direction of movement.
        {
            Log.Write("Debugging random GetAction", logType.debug);
            Random r;
            int value, x = 0, y = 0;

            // this is probably a wacky way of doing this, but I need 2 bools.
            r = GameManager.GetRandom();
            bool axis = Convert.ToBoolean(r.Next(0, 2));
            r = GameManager.GetRandom();
            bool sign = Convert.ToBoolean(r.Next(0, 2));

            if (sign) { value = 1; } else { value = -1; }

            if (axis) { x = value; } else { y = value; }

            Log.Write($"End debugging random GetAction, Current pos: {position.x}, {position.y} Moving by: {x}, {y}. New pos: {position.x + x}, {position.y + y}", logType.debug);
            return new Vector2(position.x + x, position.y + y);
        }
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

        public override Vector2 GetAction()
        {
            ConsoleKey input = Console.ReadKey(true).Key;
            int x = 0, y = 0;
            switch (input)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow: y = -1; break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow: y = 1; break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow: x = 1; break;
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow: x = -1; break;
                case ConsoleKey.Escape: Program.run = false; break;
            }

            Log.Write($"Key = {input}. Old pos = {position.x}, {position.y}. Delta = {x}, {y}. new pos = {position.x+x}, {position.y+y}");

            // for testing purposes, break here.
            return new Vector2(position.x + x, position.y + y);
        }
    }
}
