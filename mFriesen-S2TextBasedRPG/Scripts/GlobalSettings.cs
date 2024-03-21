using System;

// This is designed to hold defaults. Hardcoding here should be fine, though eventually the initialization should be done via DataManager.

namespace mFriesen_S2TextBasedRPG
{
    static class GlobalSettings
    {
        // Lets store some default tiles here.
        static Tile pdt, fdt, pbdt, pidt, prdt, pedt;
        public static Tile playerDefaultTile { get { return pdt.Clone(); } private set { pdt = value; } }
        public static Tile foeDefaultTile { get { return fdt.Clone(); } private set { fdt = value; } } // This is probably never going to be used, but just in case.

        public static Tile pickupBaseDefaultTile { get { return pbdt.Clone(); } private set { pbdt = value; } } // This is if for whatever reason we have a base pickup in use.
        public static Tile pickupItemDefaultTile { get { return pidt.Clone(); } private set { pidt = value; } }
        public static Tile pickupRestorationDefaultTile { get { return prdt.Clone(); } private set { prdt = value; } }
        public static Tile pickupEffectDefaultTile { get { return pedt.Clone(); } private set { pedt = value; } }

        // Declare the "null" items/effects
        public static ArmorItem unarmored { get; private set; }
        public static WeaponItem unarmed { get; private set; }
        static StatusEffect _noEffect;
        public static StatusEffect noEffect { get { return _noEffect.Clone(); } private set { _noEffect = value; } }

        // Define default render region settings, I cannot be bothered to make that yet.
        public static int renderRegionDefaultX = 10;
        public static int renderRegionDefaultY = 10;

        // We use a static constructor here because we don't intend to call it anywhere. We may have to remove this later for file loading of these settings.
        static GlobalSettings()
        {
            playerDefaultTile = new Tile('@', ConsoleColor.Blue, ConsoleColor.DarkBlue);
            foeDefaultTile = new Tile('E', ConsoleColor.Red, ConsoleColor.White);

            pickupBaseDefaultTile = new Tile('P', ConsoleColor.DarkYellow, ConsoleColor.White);
            pickupItemDefaultTile = new Tile('i', ConsoleColor.Blue, ConsoleColor.White);
            pickupRestorationDefaultTile = new Tile('+', ConsoleColor.Green, ConsoleColor.White);
            pickupEffectDefaultTile = new Tile('e', ConsoleColor.Cyan, ConsoleColor.Blue);

            unarmored = new ArmorItem(0);
            unarmed = new WeaponItem(0);

            noEffect = new StatusEffect(effectType.damageOverTime, "none", 0, 0);
        }
    }
}
