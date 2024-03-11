using System;
using System.Collections.Generic;

namespace mFriesen_S2TextBasedRPG
{

    abstract class Entity
    {
        // Stores entity data that isn’t player/foe/neutral specific.
        public Vector2 position = new Vector2(1, 1);
        public Tile displayTile = new Tile();


        public virtual Entity DeepClone()
        {
            Entity e = (Entity)MemberwiseClone();
            e.position = position.Clone();
            e.displayTile = displayTile.Clone();
            return e;
        }

        public virtual Vector2 GetAction() // This should return the intended move action in world coordinates
        {
            return position;
        }


    }

    abstract class Mob : Entity
    {
        public List<Item> inventory = new List<Item>();
        public StatManager statManager;
        public ArmorItem armor;
        public WeaponItem weapon;
        public StatusEffect? attackEffect;
        public StatusEffect? currentEffect;
        public bool immobilized = false;
        public string name;

        public int GetArmorDR() // This should be called by the statmanager somehow.
        {
            int dr = 0; // base dr value. Hard code because it is again, a global standard value. Everything has 0 base defense.
            if (armor != null)
            {
                dr += armor.dr;
            }
            return dr;
        }

        public void Heal(healtype type, int value)
        {
            // pass the command on to statmanager.
            statManager.Heal(type, value);
        }

        public virtual new Mob DeepClone() // idk if this should be virtual new or not.
        {
            Mob m = (Mob)MemberwiseClone();
            m.statManager = statManager.ShallowClone();
            m.name = name;
            m.position = position.Clone();
            m.displayTile = displayTile.Clone();
            if (attackEffect != null) { m.attackEffect = ((StatusEffect)attackEffect).Clone(); }
            foreach (Item item in inventory)
            {
                m.inventory.Add(item);
            }
            return m;
        }

        public bool TickEffect()
        {
            if (currentEffect != null)
            {
                StatusEffect effect = (StatusEffect)currentEffect;
                int value = effect.Tick();
                switch (effect.type)
                {
                    case effectType.damageOverTime: try { statManager.TakeDamage(value); } catch (Exception ignored) { } break;
                    case effectType.immobilized: immobilized = true; break;
                    default: throw new NotImplementedException("Effect type did not account for Mob.TickEffect");
                }
                // If timer <= 0, return true, so we remove the effect.
                if (effect.timer <= 0) { return true; }
            }
            return false;
        }

        protected virtual new Vector2 GetAction() { return new Vector2(); }

        public abstract void Update();
    }
}
