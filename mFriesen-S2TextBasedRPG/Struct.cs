using System;

namespace mFriesen_S2TextBasedRPG
{
    enum Hazard
    {
        none,
        wall
    }
    struct TriggerZone
    {
        public Vector2 topCorner;
        public Vector2 bottomCorner;

    }
    struct Vector2
    {
        public int x, y;

        public Vector2(int x, int y) { this.x = x; this.y = y; }
    }
    struct Tile
    {
        public char displayChar;
        public ConsoleColor bg, fg; // Foreground and background colours
        public Hazard hazard;
    }
}