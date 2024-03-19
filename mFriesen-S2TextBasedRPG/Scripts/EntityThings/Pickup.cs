using System;

namespace mFriesen_S2TextBasedRPG
{
    public enum pickupType { item, restoration, effect }
    public enum restorationType { hp, ap }
    class Pickup : Entity
    {
        // This should be used to set the default values of the item and its default tile. 
        // We'll also use this as the template off of which RestorationPickup is made.
        virtual protected void SetDefaultValues()
        {
            // set tile
            displayTile = new Tile();
            displayTile.displayChar = '+';
            displayTile.bg = ConsoleColor.DarkGreen;
            displayTile.fg = ConsoleColor.White;
            displayTile.hazard = Hazard.none;
        }

        // A deep clone method, returning a completely clean duplicate of the pickup;
        public override Entity DeepClone()
        {
            // If I knew how to switchcase this, I would have. This is intended for redundancy, idk if I even need this.
            if(this is RestorationPickup) { return ((RestorationPickup)this).DeepClone(); }
            if(this is ItemPickup) { return ((ItemPickup)this).DeepClone(); }

            Pickup e = (Pickup)MemberwiseClone();
            e.position = position.Clone();
            e.displayTile = displayTile.Clone();
            return e;
        }
    }
}
