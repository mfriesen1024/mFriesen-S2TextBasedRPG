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

    struct Trigger
    {
        public Vector2 topCorner;
        public Vector2 bottomCorner;

        public Trigger Clone()
        {
            Trigger t = new Trigger();
            t.topCorner = topCorner.Clone();
            t.bottomCorner = bottomCorner.Clone();
            return t;
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