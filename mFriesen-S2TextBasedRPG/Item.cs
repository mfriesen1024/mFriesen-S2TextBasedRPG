namespace mFriesen_S2TextBasedRPG
{
    public class Item // These shouldn't need to be cloned, so we won't bother making that method for these.
    {

    }

    public class ArmorItem : Item
    {
        public int dr;

        public ArmorItem(int dr = 1)
        {
            this.dr = dr;
        }
    }

    public class WeaponItem : Item
    {
        public int str;

        public WeaponItem(int str = 1)
        {
            this.str = str;
        }
    }

    // need something for pickups. either do that here, or in entity.cs.
}