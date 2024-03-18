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

    class RestorationPickup : Pickup
    {
        // References and values
        public restorationType rType;

        public int rValue;

        public RestorationPickup(Vector2 position, restorationType rType, int value)
        {
            this.position = position;
            this.rType = rType; rValue = value;

            SetDefaultValues();
        }

        // Do not override SetDefaultValues.

        public override Entity DeepClone()
        {
            RestorationPickup r = (RestorationPickup)MemberwiseClone();
            r.position = position.Clone();
            r.displayTile = displayTile.Clone();
            return r;
        }
    }

    class ItemPickup : Pickup
    {
        public Item item { get; private set; }
        public ItemPickup(Vector2 position, Item item)
        {
            this.position = position;
            this.item = item;

            SetDefaultValues();
        }

        protected override void SetDefaultValues()
        {
            // set tile
            displayTile = new Tile();
            displayTile.displayChar = 'i';
            displayTile.bg = ConsoleColor.White;
            displayTile.fg = ConsoleColor.Blue;
            displayTile.hazard = Hazard.none;
        }

        public override Entity DeepClone()
        {
            ItemPickup e = (ItemPickup)MemberwiseClone();
            e.position = position.Clone();
            e.displayTile = displayTile.Clone();
            e.item = item;
            return e;
        }
    }

    class EffectPickup : Pickup
    {
        public StatusEffect effect { get; private set; }
        public EffectPickup(Vector2 position, StatusEffect effect)
        {
            this.effect = effect;
            this.position = position;
        }

        protected override void SetDefaultValues()
        {
            // set tile
            displayTile = new Tile();
            displayTile.displayChar = 'e';
            displayTile.bg = ConsoleColor.White;
            displayTile.fg = ConsoleColor.DarkRed;
            displayTile.hazard = Hazard.none;
        }

        public override Entity DeepClone()
        {
            EffectPickup e = (EffectPickup)MemberwiseClone();
            e.position = position.Clone();
            e.displayTile = displayTile.Clone();
            e.effect = effect;
            return e;
        }
    }
}
