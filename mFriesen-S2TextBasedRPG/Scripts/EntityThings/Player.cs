using SimpleLogger;
using System;

namespace mFriesen_S2TextBasedRPG
{
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
            Vector2 target = GetAction();
            Player actor = this;

            // Check the coordinates
            Mob mob; Pickup pickup;
            EntityManager.CheckCoords(target, out pickup, out mob);

            actionResult result = LevelManager.CheckLocation(target);
            if (mob != null) {
                result = actionResult.fail;
                if(mob is Foe)
                {
                    mob.statManager.TakeDamage(statManager.GetDamage());
                    if(attackEffect != null) { mob.currentEffect = currentEffect; }
                }
            }

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
}
