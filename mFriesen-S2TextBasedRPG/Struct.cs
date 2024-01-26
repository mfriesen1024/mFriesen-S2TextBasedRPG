﻿using System;

namespace mFriesen_S2TextBasedRPG
{
    enum Hazard
    {
        none,
        wall
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

        public Vector2(int x, int y) { this.y = x; this.x = y; }

        public Vector2 Clone()
        {
            return (Vector2)MemberwiseClone();
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
}