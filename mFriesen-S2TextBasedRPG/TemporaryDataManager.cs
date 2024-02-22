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
        public static List<StatusEffect> statusEffects;
        public static Player player;

        public static void GenerateThings()
        {
            GenerateStatusEffects();
            GenerateFoes();
            GenerateItems();
            GeneratePickups();
            GenerateAreas();

            // Generate the player.
            player = new Player(20);
            player.position = new Vector2(6, 20);
            player.inventory.Add(weapons[0]); // give the player a stick.
        }

        static void GenerateFoes()
        {
            foes = new List<Foe>();
            foes.Add(new Foe(1, 5, 0)); // Create a rather weak goblin. Once its vitality goes away, its a 1hit.
            foes[0].name = "Goblin";
            foes.Add(new Foe(5, 0, 1)); // Create a more resilient skeleton. It can take a hit or 2.
            foes[1].name = "Skeleton";
            foes.Add(new Foe(15, 10, 5, 5)); // Create a formidable dragon.
            foes[2].name = "Dragon";
            foes[2].movement = Foe.movementType.stationary;
            foes[2].attackEffect = statusEffects[0];
        }

        static void GenerateAreas()
        {
            areas = new List<Area>();
            Area demoArea = new Area("demoArea");
            Foe[] demoEncounter = { (Foe)foes[0].DeepClone(), (Foe)foes[1].DeepClone(), (Foe)foes[0].DeepClone(), (Foe)foes[2].DeepClone() };
            {
                demoEncounter[0].position = new Vector2(3, 24);
                demoEncounter[1].position = new Vector2(9, 18);
                demoEncounter[2].position = new Vector2(6, 12);
                demoEncounter[3].position = new Vector2(6, 6);
            }
            demoEncounter[0].movement = Foe.movementType.linear; demoEncounter[0].start = new Vector2(2, 6); demoEncounter[0].end = new Vector2(1, 1);
            Log.Write("test string", logType.debug);
            pickups[0].position = new Vector2(3, 6);
            pickups[1].position = new Vector2(7, 4);
            pickups[2].position = new Vector2(1, 1);
            demoArea.pickups = pickups.ToArray();
            demoArea.encounter = demoEncounter;
            areas.Add(demoArea);
        }

        static void GenerateItems()
        {
            weapons = new List<WeaponItem>();
            armorItems = new List<ArmorItem>();

            // Define some weapons.
            weapons.Add(new WeaponItem());
            weapons.Add(new WeaponItem(6));
            weapons.Add(new WeaponItem(12));

            // Define some armor.
            armorItems.Add(new ArmorItem());
            armorItems.Add(new ArmorItem(4));
            armorItems.Add(new ArmorItem(8));
        }

        static void GeneratePickups()
        {
            pickups = new List<Pickup>();
            pickups.Add(new Pickup(new Vector2(9, 8), Pickup.restorationType.hp, 30));
            pickups.Add(new Pickup(new Vector2(7, 16), Pickup.restorationType.ap, 15));
            pickups.Add(new Pickup(new Vector2(1, 1), weapons[2]));
            pickups.Add(new Pickup(new Vector2(3, 5), armorItems[1]));
        }

        static void GenerateStatusEffects()
        {
            statusEffects = new List<StatusEffect>();
            statusEffects.Add(new StatusEffect(effectType.damageOverTime, "DragonFire", 3, 3)); // Dragon should produce fire.
        }
    }
}
