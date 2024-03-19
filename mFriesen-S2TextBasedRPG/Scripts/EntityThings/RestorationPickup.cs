namespace mFriesen_S2TextBasedRPG
{
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
}
