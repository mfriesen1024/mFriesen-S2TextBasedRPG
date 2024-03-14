using System;

namespace mFriesen_S2TextBasedRPG
{
    class Pickup : Entity
    {
        public enum pickupType { item, restoration }
        public enum restorationType { hp, ap }

        // References and values
        public restorationType? rType;
        public pickupType pType;

        public int? rValue;

        
        // This constructor should be used for restoration pickups.
        public Pickup(Vector2 position, restorationType rType, int value)
        {
            this.position = position;
            pType = pickupType.restoration;
            this.rType = rType; rValue = value;

            SetDefaultValues();
        }

        // This should be used to set the default values of the item, such as a few nulls, and its default tile.
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
            Pickup e = (Pickup)MemberwiseClone();
            e.position = position.Clone();
            e.displayTile = displayTile.Clone();
            e.rValue = rValue;
            e.rType = rType;
            e.pType = pType;
            return e;
        }
    }

    class ItemPickup : Pickup
    {
        public Item item { get; private set; }
        public ItemPickup(Vector2 position, Item item)
        {
            this.position = position;
            rType = null;
            pType = pickupType.item;
            rValue = null;
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
}
