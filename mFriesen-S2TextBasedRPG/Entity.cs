using SimpleLogger;
using System;
using System.Collections.Generic;

namespace mFriesen_S2TextBasedRPG
{

    abstract class Entity
    {
        // Stores entity data that isn’t player/foe/neutral specific.
        public Vector2 position = new Vector2(0, 0);
        public Tile displayTile = new Tile();
        

        public virtual Entity DeepClone()
        {
            Entity e = (Entity)MemberwiseClone();
            e.position = position.Clone();
            e.displayTile = displayTile.Clone();
            return e;
        }

        public virtual Vector2 GetAction() // This should return the intended move action in world coordinates
        {
            return position;
        }

        
    }

    abstract class Mob : Entity
    {
        public List<Item> inventory = new List<Item>();
        public StatManager statManager;
        public int? armorInventoryIndex;
        public int? weaponInventoryIndex;
        public StatusEffect attackDebuff;

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

        public int GetArmorDR() // This should be called by the statmanager somehow.
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

        public virtual Mob DeepClone() // idk if this should be virtual new or not.
        {
            Mob m = (Mob)MemberwiseClone();
            m.statManager = statManager.ShallowClone();
            m.position = position.Clone();
            m.displayTile = displayTile.Clone();
            m.attackDebuff = attackDebuff.Clone();
            foreach (Item item in inventory)
            {
                m.inventory.Add(item);
            }
            return m;
        }
    }

    class Foe : Mob
    {
        // Foe specific things here, if any.

        public Foe(int hp = 10, int ap = 0, int dr = 0, int str = 1)
        {
            statManager = new StatManager(hp, ap, dr, str, this);

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

    class Player : Mob
    {
        // Player specific things here.

        public Player(int hp = 10, int ap = 0, int dr = 0, int str = 1)
        {
            statManager = new StatManager(hp, ap, dr, str, this);

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
                case ConsoleKey.Escape: GameManager.run = false; break;
            }

            //Log.Write($"Key = {input}. Old pos = {position.x}, {position.y}. Delta = {x}, {y}. new pos = {position.x + x}, {position.y + y}", logType.debug);

            // for testing purposes, break here.
            return new Vector2(position.x + x, position.y + y);
        }

        public void UsePickup(Pickup pickup)
        {
            switch (pickup.pType)
            {
                case Pickup.pickupType.item:
                    {
                        if(pickup.item != null)
                        {
                            inventory.Add(pickup.item);
                            try
                            {
                                ArmorItem a = (ArmorItem)pickup.item;
                                armorInventoryIndex = inventory.Count - 1;
                            }
                            catch (Exception ignored) { }
                            try
                            {
                                WeaponItem w = (WeaponItem)pickup.item;
                                weaponInventoryIndex = inventory.Count - 1;
                            }
                            catch (Exception ignored) { }
                        }
                        else { Log.Write("Pickup item was null! This is wrong!", logType.error);}
                        break;
                    }
                case Pickup.pickupType.restoration:
                    try
                    {
                        switch (pickup.rType)
                        {
                            case Pickup.restorationType.hp: statManager.Heal(healtype.health, (int)pickup.rValue); break;
                            case Pickup.restorationType.ap: statManager.Heal(healtype.absorption, (int)pickup.rValue); break;
                        }
                    }
                    catch (NullReferenceException nre) { Log.Write(nre.Message, logType.error); Log.Write(nre.StackTrace, logType.debug); }
                    break;
                default: throw new NotImplementedException(pickup.pType.ToString() + " Is not implemented in Player.UsePickup();.");
            }
        }
    }

    class Pickup : Entity
    {
        public enum pickupType { item, restoration }
        public enum restorationType { hp, ap }

        // References and values
        public restorationType? rType;
        public pickupType pType;

        public Item item;
        public int? rValue;

        // This constructor should be used if its an item pickup. An overload will be provided for restoration pickups.
        public Pickup(Vector2 position, Item item)
        {
            this.position = position;
            rType = null;
            pType = pickupType.item;
            rValue = null;
            this.item = item;

            SetDefaultValues();
        }
        // This constructor should be used for restoration pickups.
        public Pickup(Vector2 position, restorationType rType, int value)
        {
            this.position = position;
            pType = pickupType.restoration;
            this.rType = rType; rValue = value;
            item = null;

            SetDefaultValues();
        }

        // This should be used to set the default values of the item, such as a few nulls, and its default tile.
        private void SetDefaultValues()
        {
            // set tile
            displayTile = new Tile();
            displayTile.displayChar = '+';
            displayTile.bg = ConsoleColor.DarkGreen;
            displayTile.fg = ConsoleColor.White;
            displayTile.hazard = Hazard.none;
        }

        // A deep clone method, returning a completely clean duplicate of the pickup;
        public override Entity DeepClone()
        {
            Pickup e = (Pickup)MemberwiseClone();
            e.position = position.Clone();
            e.displayTile = displayTile.Clone();
            e.item = item;
            e.rValue = rValue;
            e.rType = rType;
            e.pType = pType;
            return e;
        }
    }
}
