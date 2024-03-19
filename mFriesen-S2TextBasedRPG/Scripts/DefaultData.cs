using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// This is designed to hold defaults. Hardcoding here should be fine, though eventually the initialization should be done via DataManager.

namespace mFriesen_S2TextBasedRPG.Scripts
{
    static class DefaultData
    {
        // Lets store some default tiles here.
        public static Tile playerDefaultTile;
        public static Tile foeDefaultTile; // This is probably never going to be used, but just in case.

        public static Tile pickupBaseDefaultTile; // This is if for whatever reason we have a base pickup in use.
        public static Tile pickupItemDefaultTile;
        public static Tile pickupRestorationDefaultTile;
        public static Tile pickupEffectDefaultTile;

        // Define the "null" items/effects
        public static ArmorItem unarmored;
        public static WeaponItem unarmed;
        public static StatusEffect noEffect;

        // Define default render region settings, I cannot be bothered to make that yet.
        public static int renderRegionDefaultX = 10;
        public static int renderRegionDefaultY = 10;
    }
}
