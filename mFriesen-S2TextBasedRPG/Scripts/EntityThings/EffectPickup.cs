using System;

namespace mFriesen_S2TextBasedRPG
{
    class EffectPickup : Pickup
    {
        public StatusEffect effect { get; private set; }
        public EffectPickup(Vector2 position, StatusEffect effect)
        {
            this.effect = effect;
            this.position = position;

            SetDefaultValues();
        }

        protected override void SetDefaultValues()
        {
            // set tile
            displayTile = GlobalSettings.pickupEffectDefaultTile;
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
