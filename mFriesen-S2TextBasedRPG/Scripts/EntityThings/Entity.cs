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
        public ArmorItem armor;
        public WeaponItem weapon;
        public StatusEffect? attackEffect;
        public StatusEffect? currentEffect;
        public bool immobilized = false;
        public string name;

        public int GetArmorDR() // This should be called by the statmanager somehow.
        {
            int dr = 0; // base dr value. Hard code because it is again, a global standard value. Everything has 0 base defense.
            if (armor != null)
            {
                dr += armor.dr;
            }
            return dr;
        }

        public void Heal(healtype type, int value)
        {
            // pass the command on to statmanager.
            statManager.Heal(type, value);
        }

        public virtual new Mob DeepClone() // idk if this should be virtual new or not.
        {
            Mob m = (Mob)MemberwiseClone();
            m.statManager = statManager.ShallowClone();
            m.name = name;
            m.position = position.Clone();
            m.displayTile = displayTile.Clone();
            if (attackEffect != null) { m.attackEffect = ((StatusEffect)attackEffect).Clone(); }
            foreach (Item item in inventory)
            {
                m.inventory.Add(item);
            }
            return m;
        }

        public bool TickEffect()
        {
            if (currentEffect != null)
            {
                StatusEffect effect = (StatusEffect)currentEffect;
                int value = effect.Tick();
                switch (effect.type)
                {
                    case effectType.damageOverTime: try { statManager.TakeDamage(value); } catch (Exception ignored) { } break;
                    case effectType.immobilized: immobilized = true; break;
                    default: throw new NotImplementedException("Effect type did not account for Mob.TickEffect");
                }
                // If timer <= 0, return true, so we remove the effect.
                if (effect.timer <= 0) { return true; }
            }
            return false;
        }

        protected virtual new Vector2 GetAction() { return new Vector2(); }

        public abstract void Update();
    }

    class Foe : Mob
    {
        public enum movementType { stationary, random, linear }
        public movementType movement = movementType.random;
        public Vector2 start;
        public Vector2 end;
        public int moveSpeed = 1;
        bool isReturning;

        public Foe(int hp = 10, int ap = 0, int dr = 0, int str = 1, bool useDefaultTile = true)
        {
            statManager = new StatManager(hp, ap, dr, str, this);

            // set display char
            if (useDefaultTile)
            {
                displayTile.fg = System.ConsoleColor.Red; displayTile.bg = System.ConsoleColor.DarkRed;
                displayTile.displayChar = 'E';
            }

            start = position.Clone();
            end = new Vector2(position.x + 5, position.y);
        }

        protected override Vector2 GetAction()
        {
            if (position.Equals(end)) { isReturning = true; } else if (position.Equals(start)) { isReturning = false; }
            Log.Write("Debugging random GetAction", logType.debug);
            Random r;
            int value = 0, x = 0, y = 0;

            // this is probably a wacky way of doing this, but I need 2 bools.
            r = GameManager.GetRandom();
            bool axis = false;
            if (movement == movementType.random || movement == movementType.stationary) // include stationary as it will attack randomly.
            {
                axis = Convert.ToBoolean(r.Next(0, 2));
                while (value == 0)
                {
                    GameManager.GetRandom();
                    int temp = r.Next(-10, 11);
                    if (temp > 0) { value = 1; }
                    if (temp < 0) { value = -1; }
                }
            }
            if (movement == movementType.linear)
            {
                axis = GetLinearMovementAxis();
                char axisChar = ' ';
                if (axis) { axisChar = 'y'; } else { axisChar = 'x'; }
                value = GetLinearValue(axisChar);
            }

            if (axis) { y = value; } else { x = value; }
            //if (movement == movementType.stationary || immobilized) { x = 0; y = 0; }

            Log.Write($"End debugging random GetAction, Current pos: {position.x}, {position.y} Moving by: {x}, {y}. New pos: {position.x + x}, {position.y + y}", logType.debug);
            return new Vector2(position.x + x, position.y + y);
        }

        int GetLinearValue(char axis)
        {
            Vector2 target;
            if (isReturning) { target = start; } else { target = end; }

            int x1 = target.x, y1 = target.y;
            int x2 = position.x, y2 = position.y;

            // get differences.
            int xDiff = x1 - x2;
            int yDiff = y1 - y2;

            int x, y; if (xDiff < 0) { x = -1; } else { x = 1; }
            if (yDiff < 0) { y = -1; } else { y = 1; }
            if (axis == 'x') { return x; } else if (axis == 'y') { return y; } else { throw new ArgumentException("invalid axis"); }
        }

        bool GetLinearMovementAxis()
        {
            Vector2 target;
            if (isReturning) { target = start; } else { target = end; }

            int x1 = target.x, y1 = target.y;
            int x2 = position.x, y2 = position.y;

            // get differences.
            int xDiff = x1 - x2;
            int yDiff = y1 - y2;
            if (xDiff < 0) { xDiff *= -1; }
            if (yDiff < 0) { yDiff *= -1; }

            if (yDiff > xDiff) { return true; } else { return false; }
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }

    class Player : Mob
    {
        // Player specific things here.

        public Player(int hp = 10, int ap = 0, int dr = 0, int str = 1)
        {
            statManager = new StatManager(hp, ap, dr, str, this);

            name = "player";

            // set display char
            displayTile.fg = System.ConsoleColor.Blue; displayTile.bg = System.ConsoleColor.DarkBlue;
            displayTile.displayChar = '@';
        }

        protected override Vector2 GetAction()
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
                        if (pickup.item != null)
                        {
                            inventory.Add(pickup.item);
                            try
                            {
                                ArmorItem a = (ArmorItem)pickup.item;
                                armor = (ArmorItem)pickup.item;
                            }
                            catch (Exception ignored) { }
                            try
                            {
                                WeaponItem w = (WeaponItem)pickup.item;
                                weapon = (WeaponItem)pickup.item;
                            }
                            catch (Exception ignored) { }
                        }
                        else { Log.Write("Pickup item was null! This is wrong!", logType.error); }
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

        public override void Update()
        {
            throw new NotImplementedException();

            Vector2 target = GetAction();
            Player actor = this;

            // Check the coordinates
            Mob mob; Pickup pickup;
            EntityManager.CheckCoords(target, out pickup, out mob);

            actionResult result = LevelManager.WallCheck(target);
            if (mob != null) { result = actionResult.fail; }

            // Move this to EntityManager
            if (pickup != null)
            {
                result = actionResult.fail;
                actor.UsePickup(pickup); EntityManager.DeleteItem(pickup);
            }

            // now check if immobile, and if true, cancel movement.
            if (actor.immobilized) { result = actionResult.fail; }

            if (result == actionResult.move)
            {
                actor.position = target;
            }

            if (actor.TickEffect()) { actor.currentEffect = null; }
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
