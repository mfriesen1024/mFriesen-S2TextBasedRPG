using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// This is designed to hold defaults. Hardcoding here should be fine, though eventually the initialization should be done via DataManager.

namespace mFriesen_S2TextBasedRPG.Scripts
{
    static class GlobalConstants
    {
        // Lets store some default tiles here.
        public static Tile playerDefaultTile { get { return playerDefaultTile.Clone(); } private set { playerDefaultTile = value; } }
        public static Tile foeDefaultTile { get { return foeDefaultTile.Clone(); } private set { foeDefaultTile = value; } } // This is probably never going to be used, but just in case.

        public static Tile pickupBaseDefaultTile { get { return pickupBaseDefaultTile.Clone(); } private set { pickupBaseDefaultTile = value; } } // This is if for whatever reason we have a base pickup in use.
        public static Tile pickupItemDefaultTile { get { return pickupItemDefaultTile.Clone(); } private set { pickupItemDefaultTile = value; } }
        public static Tile pickupRestorationDefaultTile { get { return pickupRestorationDefaultTile.Clone(); } private set { pickupRestorationDefaultTile = value; } }
        public static Tile pickupEffectDefaultTile { get { return pickupEffectDefaultTile.Clone(); } private set { pickupEffectDefaultTile = value; } }

        // Declare the "null" items/effects
        public static ArmorItem unarmored { get; private set; }
        public static WeaponItem unarmed { get; private set; }
        public static StatusEffect noEffect { get; private set; }

        // Define default render region settings, I cannot be bothered to make that yet.
        public static int renderRegionDefaultX = 10;
        public static int renderRegionDefaultY = 10;

        // We use a static constructor here because we don't intend to call it anywhere. We may have to remove this later for file loading of these settings.
        static GlobalConstants()
        {

        }
    }
}
