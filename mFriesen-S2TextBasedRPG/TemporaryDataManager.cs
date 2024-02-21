using SimpleLogger;
using System.Collections.Generic;

namespace mFriesen_S2TextBasedRPG
{
    static class TemporaryDataManager
    {
        public static List<Foe> foes;
        public static List<Area> areas;
        public static List<WeaponItem> weapons;
        public static List<ArmorItem> armorItems;
        public static List<Pickup> pickups;
        public static Player player;

        public static void GenerateThings()
        {
            GenerateFoes();
            GenerateItems();
            GeneratePickups();
            GenerateAreas();

            // Generate the player.
            player = new Player();
            player.position = new Vector2 (8, 15);
            player.inventory.Add(weapons[0]); // give the player a stick.
        }

        static void GenerateFoes()
        {
            foes = new List<Foe>();
            foes.Add(new Foe(1, 5, 0)); // Create a rather weak goblin. Once its vitality goes away, its a 1hit.
            foes.Add(new Foe(5, 0, 2)); // Create a more resilient skeleton. It can take a hit or 2.
            foes.Add(new Foe(15, 10, 5)); // Create a formidable dragon.
        }

        static void GenerateAreas()
        {
            areas = new List<Area>();
            Area demoArea = new Area("demoArea");
            Foe[] demoEncounter = { (Foe)foes[0].DeepClone(), (Foe)foes[0].DeepClone(), (Foe)foes[0].DeepClone() };
            { demoEncounter[0].position = new Vector2(2, 8); demoEncounter[1].position = new Vector2(6, 2); demoEncounter[2].position = new Vector2(4, 5); }
            Log.Write("test string", logType.debug);
            demoArea.pickups[0].position = new Vector2(3, 6);
            demoArea.pickups[1].position = new Vector2(7, 4);
            demoArea.encounter = demoEncounter;
            areas.Add(demoArea);
        }

        static void GenerateItems()
        {
            weapons = new List<WeaponItem>();
            armorItems = new List<ArmorItem>();

            // Define some weapons.
            weapons.Add(new WeaponItem());
            weapons.Add(new WeaponItem(3));
            weapons.Add(new WeaponItem(6));

            // Define some armor.
            armorItems.Add(new ArmorItem());
            armorItems.Add(new ArmorItem(3));
            armorItems.Add(new ArmorItem(6));
        }

        static void GeneratePickups()
        {
            pickups = new List<Pickup>();
            pickups.Add(new Pickup(new Vector2(0, 0), Pickup.restorationType.hp, 30));
            pickups.Add(new Pickup(new Vector2(0, 0), Pickup.restorationType.ap, 15));
            pickups.Add(new Pickup(new Vector2(0, 0), weapons[1]));
        }
    }
}
