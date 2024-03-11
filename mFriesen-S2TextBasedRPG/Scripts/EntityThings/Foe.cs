using SimpleLogger;
using System;

namespace mFriesen_S2TextBasedRPG
{
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

        public override void Update()
        {
            Vector2 target = GetAction();
            Foe actor = this;

            // Check the coordinates
            Mob mob; Pickup pickup;
            EntityManager.CheckCoords(target, out pickup, out mob);

            actionResult result = LevelManager.CheckLocation(target);
            if (mob != null)
            {
                result = actionResult.fail;
                if(mob != this)
                {
                    mob.statManager.TakeDamage(statManager.GetDamage());
                    if (attackEffect != null) { mob.currentEffect = currentEffect; }
                }
                if(mob is Player) { Log.Write("Attempted to attack player, this.debug", logType.debug); }
            }

            // now check if immobile, and if true, cancel movement.
            if (actor.immobilized) { result = actionResult.fail; }

            if (result == actionResult.move)
            {
                actor.position = target;
            }

            if (actor.TickEffect()) { actor.currentEffect = null; }
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
    }
}
