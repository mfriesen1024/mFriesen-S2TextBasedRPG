// This is intended to fulfil the "Health System" requirement. Its sole purpose is to manage stats.


// To be used in Mod/Get stat.
using SimpleLogger;
using System;

enum statname
{
    hp,
    ap,
    dr,
    str
}

enum healtype
{
    health,
    absorption
}

namespace mFriesen_S2TextBasedRPG
{
    internal class StatManager
    {
        Mob owner;

        int hp;
        int ap;
        int dr;
        int str;
        public int maxHP {  get; private set; }

        public bool isDying = false;

        public StatManager(int hp, int ap, int dr, int str, Mob owner)
        {
            this.owner = owner;

            this.hp = hp;
            this.ap = ap;
            this.dr = dr;
            maxHP = hp;
            this.str = str;
        }

        public void ModStat(statname stat, int value)
        {
            switch (stat)
            {
                case statname.hp: hp = value; break;
                case statname.ap: ap = value; break;
                case statname.dr: dr = value; break;
                case statname.str: str = value; break;
            }
        }
        // Get method.
        public int GetStat(statname stat)
        {
            int Return = 0;
            switch (stat)
            {
                case statname.hp: Return = hp; break;
                case statname.ap: Return = ap; break;
                case statname.dr: Return = dr; break;
                case statname.str: Return = str; break;
            }
            return Return;
        }

        public StatManager ShallowClone()
        {
            return (StatManager)MemberwiseClone();
        }

        // Need to add damage/heal death things.

        public void TakeDamage(int damage)
        {
            // Get damage reduction
            int dr = this.dr + owner.GetArmorDR();

            // Is damage greater than 0?
            if (damage < 1)
            {
                string txt = "Damage must be greater than 0. If you want to do wacky things with health, use ModStat()";
                Log.Write(txt, logType.error);
                throw new ArgumentOutOfRangeException("damage", damage, txt);
            }

            // Next, subtract damage resistance from damage.
            if (dr >= 0)
            {
                damage -= dr;

                if (damage < 1) { return; }
            }
            else
            {
                string txt = $"DR must be greater than 0.";
                Log.Write(txt, logType.error);
                return;
            }

            // Now, try dealing damage to absorption. Else, deal damage to health.
            if (ap > 0)
            {
                ap -= damage; if (ap < 0) { ap = 0; }
            }
            else
            {
                ap = 0;
                hp -= damage; if (hp < 0) { hp = 0; isDying = true; }
            }
        }

        public void TakeDamage(int dr, int damage) { TakeDamage(damage); } // temporary overload

        public int GetDamage()
        {
            int damage = 1; // base damage value. hard code it because damage should never be 0.
            if (owner.weaponInventoryIndex != null)
            {
                damage += ((WeaponItem)owner.inventory[(int)owner.weaponInventoryIndex]).str;
            }
            damage += str;

            Log.Write($"Damage was requested. Got {damage}", logType.debug);

            return damage;
        }

        public void Heal(healtype type, int value) // This should be used to heal. Negative HP/AP values should be set via ModStat
        {
            if (value < 0)
            {
                Log.Write("Heal value was less than 0.", logType.warning);
            }
            else if (value == 0)
            {
                Log.Write("Heal value was 0.", logType.error); return;
            }

            switch (type)
            {
                case healtype.health:
                    {
                        hp += value;
                        if (hp < 0) { hp = 0; }
                        if (hp > maxHP) { hp = maxHP; }
                        break;
                    }
                case healtype.absorption:
                    {
                        ap += value;
                        if (ap < 0) { ap = 0; }
                        break;
                    }
            }
        }
    }
}
