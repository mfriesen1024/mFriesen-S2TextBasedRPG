// This is intended to fulfil the "Health System" requirement. Its sole purpose is to manage stats.


// To be used in Mod/Get stat.
enum statname
{
    hp,
    ap,
    dr,
    str
}

namespace mFriesen_S2TextBasedRPG
{
    internal class StatManager
    {
        int hp;
        int ap;
        int dr;
        int str;
        int maxHP;

        public StatManager(int hp, int ap, int dr, int str)
        {
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
    }
}
