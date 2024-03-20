using System;

namespace mFriesen_S2TextBasedRPG
{
    public enum pickupType { item, restoration, effect }
    public enum restorationType { hp, ap }
    class Pickup : Entity
    {
        /// <summary>
        /// This should be used to set the default values for things such as displayed tiles that require a default value.
        /// These should be loaded from GlobalSettings.
        /// </summary>
        virtual protected void SetDefaultValues()
        {
            // set tile
            displayTile = GlobalSettings.pickupBaseDefaultTile;
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
