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
        public Vector2 playerNewPos;
        bool allowNonPlayers;

        /// <summary>
        /// Creates a new trigger
        /// </summary>
        /// <param name="tc">The top corner of the trigger.</param>
        /// <param name="bc">The bottom corner of the trigger.</param>
        /// <param name="t">The type of trigger to be used</param>
        /// <param name="area">The area to be loaded, if type is warp. Win triggers can safely leave this as 0.</param>
        /// <param name="np">The position to warp the player to upon loading the new area.</param>
        /// <param name="anp">Whether or not to allow non players to activate the trigger.</param>
        public Trigger(Vector2 tc, Vector2 bc, triggerType t, int area, Vector2 np, bool anp = false)
        {
            topCorner = tc;
            bottomCorner = bc;
            type = t;
            nextArea = area;
            playerNewPos = np;
            allowNonPlayers = anp;
        }

        public Trigger Clone()
        {
            Trigger t = (Trigger)MemberwiseClone();
            t.topCorner = topCorner.Clone();
            t.bottomCorner = bottomCorner.Clone();
            t.playerNewPos = playerNewPos;
            return t;
        }

        // This checks if the given mob is in the triggerzone defined by the bounds topCorner, bottomCorner
        // Normally, this should be the player, but we take into account the possibility that it won't be.
        public void CheckTrigger(Mob mob) 
        {
            bool xCheck = mob.position.x >= topCorner.x && mob.position.x <= bottomCorner.x;
            bool yCheck = mob.position.y >= topCorner.y && mob.position.y <= bottomCorner.y;

            if(xCheck && yCheck && (mob is Player || allowNonPlayers))
            {
                OnTriggerActivate();
            }

            Log.Write($"Ran CheckTrigger, mob position is {mob.position} topcorner is {topCorner} bottomcorner is {bottomCorner}. xCheck is {xCheck} yCheck is {yCheck}", logType.debug);
        }

        void OnTriggerActivate()
        {
            switch (type)
            {
                case triggerType.warp: LevelManager.LoadArea(nextArea); EntityManager.player.position = playerNewPos; break;
                case triggerType.win: GameManager.win = true; GameManager.run = false; break;
            }
        }
    }

    struct Vector2
    {
        public static Vector2 zero { get { return new Vector2(0, 0); } }
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

        public Tile(char _char = ' ', ConsoleColor _bg = ConsoleColor.Black, ConsoleColor _fg = ConsoleColor.Black, Hazard _hazard = Hazard.none)
        {
            displayChar = _char;
            bg = _bg; fg = _fg;
            hazard = _hazard;
        }

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