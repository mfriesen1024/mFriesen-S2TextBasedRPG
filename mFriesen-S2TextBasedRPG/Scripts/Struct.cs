using SimpleLogger;
using System;

namespace mFriesen_S2TextBasedRPG
{
    enum Hazard
    {
        none,
        wall
    }
    enum effectType
    {
        damageOverTime,
        immobilized
    }
    enum triggerType
    {
        warp,
        win
    }

    struct Trigger
    {
        public Vector2 topCorner; // Should be the lower set of values
        public Vector2 bottomCorner; // Should be the higher set of values
        public triggerType type;
        public int nextArea {  get; private set; }

        public Trigger(Vector2 tc, Vector2 bc, triggerType t, int area)
        {
            topCorner = tc;
            bottomCorner = bc;
            type = t;
            nextArea = area;
        }

        public Trigger Clone()
        {
            Trigger t = (Trigger)MemberwiseClone();
            t.topCorner = topCorner.Clone();
            t.bottomCorner = bottomCorner.Clone();
            return t;
        }

        // This checks if the given mob is in the triggerzone defined by the bounds topCorner, bottomCorner
        // Normally, this should be the player, but we take into account the possibility that it won't be.
        public void CheckTrigger(Mob mob) 
        {
            bool xCheck = mob.position.x >= topCorner.x && mob.position.x <= bottomCorner.x;
            bool yCheck = mob.position.y >= topCorner.y && mob.position.y <= bottomCorner.y;

            if(xCheck && yCheck)
            {
                OnTriggerActivate();
            }

            Log.Write($"Ran CheckTrigger, mob position is {mob.position} topcorner is {topCorner} bottomcorner is {bottomCorner}. xCheck is {xCheck} yCheck is {yCheck}", logType.debug);
        }

        void OnTriggerActivate()
        {
            switch (type)
            {
                case triggerType.warp: GameManager.LoadArea(nextArea); break;
                case triggerType.win: GameManager.win = true; GameManager.run = false; break;
            }
        }
    }

    struct Vector2
    {
        public int y, x;

        public Vector2(int x, int y) { this.x = x; this.y = y; }

        public Vector2 Clone()
        {
            return (Vector2)MemberwiseClone();
        }

        public override bool Equals(object obj)
        {
            bool result = false;
            if (obj is Vector2)
            {
                Vector2 v2 = (Vector2)obj;
                if (x == v2.x && y == v2.y) { result = true; }
            }
            return result;
        }

        public override string ToString()
        {
            return $"{x}, {y}";
        }
    }
    struct Tile
    {
        public char displayChar;
        public ConsoleColor bg, fg; // Foreground and background colours
        public Hazard hazard;

        public Tile Clone()
        {
            return (Tile)MemberwiseClone();
        }
    }

    struct StatusEffect
    {
        public effectType type { get; private set; }
        public string name { get; private set; }
        public int timer { get; private set; }
        public int value { get; private set; }

        public StatusEffect(effectType type, string name, int timer, int value)
        {
            this.type = type;
            this.name = name;
            this.timer = timer;
            this.value = value;
        }

        public StatusEffect Clone()
        {
            return (StatusEffect)MemberwiseClone();
        }

        public int Tick()
        {
            timer--;
            return value;
        }
    }
}