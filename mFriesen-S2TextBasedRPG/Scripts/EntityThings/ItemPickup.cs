namespace mFriesen_S2TextBasedRPG
{
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
            displayTile = GlobalSettings.pickupItemDefaultTile;
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
