using SimpleLogger;
using System;
using System.Collections.Generic;
using System.IO;

namespace mFriesen_S2TextBasedRPG
{
    internal class DataManager
    {
        static string indexAdd = "\\index.txt";
        static string[] dirs;

        // Data goes here.
        public static List<StatusEffect> statusEffects;
        public static List<Item> items;
        public static Player player;
        public static List<Foe> foes;
        public static List<Area> areas;

        public static void Startup(string[] directories)
        {
            dirs = directories;
            foreach (string d in directories)
            {
                Directory.CreateDirectory(d);
                string index = d + indexAdd;
                if (!File.Exists(index)) { File.Create(index).Close(); }
            }
            Load();
        }

        public static void Load()
        {
            LoadEffects();
            LoadItems();
            LoadEntities();
            LoadAreas();
        }

        static void LoadEffects(string extension = "txt")
        {
            string dir = dirs[1];
            string[] fileNames = File.ReadAllLines(dir + indexAdd);
            statusEffects = new List<StatusEffect>();

            foreach (string file in fileNames)
            {
                try
                {
                    string fileName = dir + "\\" + file + "." + extension;
                    if (!File.Exists(fileName)) { File.Create(fileName); throw new FileNotFoundException($"{fileName} was not found, so it was created."); }
                    string[] data = File.ReadAllLines(fileName);

                    statusEffects.Add(new StatusEffect(
                        type: (effectType)int.Parse(data[0]),
                        name: data[1],
                        timer: int.Parse(data[2]),
                        value: int.Parse(data[3])
                        ));
                }
                catch (Exception e) { Log.Write("Failed to load an effect: " + e.Message, logType.error); Log.Write(e.StackTrace, logType.debug); }
            }
            Log.Write($"Loaded {statusEffects.Count} effects.");
        }

        enum itemType { armor, weapon }

        static void LoadItems(string extension = "txt")
        {
            string dir = dirs[2];
            string[] fileNames = File.ReadAllLines(dir + indexAdd);
            items = new List<Item>();

            foreach (string file in fileNames)
            {
                try
                {
                    string fileName = dir + "\\" + file + "." + extension;
                    if (!File.Exists(fileName)) { File.Create(fileName); throw new FileNotFoundException($"{fileName} was not found, so it was created."); }
                    string[] data = File.ReadAllLines(fileName);

                    if (data[0] != "")
                    {
                        switch ((itemType)int.Parse(data[0]))
                        {
                            case itemType.armor: items.Add(new ArmorItem(int.Parse(data[1]))); break;
                            case itemType.weapon: items.Add(new WeaponItem(int.Parse(data[1]))); break;
                            default: throw new NotImplementedException("Unsupported item type.");
                        }
                    }
                    else { throw new NotImplementedException("Currently, only armor and weapons are supported."); }
                }
                catch (Exception e) { Log.Write("Failed to load an item: " + e.Message, logType.error); Log.Write(e.StackTrace, logType.debug); }
            }
            Log.Write($"Loaded {items.Count} items.");
        }


        // Load entities from files. Format will be documented via github wiki, if I ever make one.
        static void LoadEntities(string extension = "txt")
        {
            string dir = dirs[3];
            string[] fileNames = File.ReadAllLines(dir + indexAdd);
            foes = new List<Foe>();
            int count = 0;

            // Load player separately
            try
            {
                string fileName = dir + "\\" + fileNames[0] + "." + extension;
                if (!File.Exists(fileName)) { File.Create(fileName); throw new FileNotFoundException($"{fileName} was not found, so it was created."); }
                string[] playerData = File.ReadAllLines(fileName);

                player = new Player(int.Parse(playerData[0]), int.Parse(playerData[1]), int.Parse(playerData[2]), int.Parse(playerData[3]))
                {
                    attackEffect = statusEffects[int.Parse(playerData[4])],
                    weapon = (WeaponItem)items[int.Parse(playerData[5])],
                    armor = (ArmorItem)items[int.Parse(playerData[6])]
                };
                count++;
            }
            catch (Exception e) { Log.Write("Failed to load the player: " + e.Message, logType.error); Log.Write(e.StackTrace, logType.debug); }

            // Now load foes
            for (int fileIndex = 1; fileIndex < fileNames.Length; fileIndex++)
            {
                try
                {
                    // Get data
                    string fileName = dir + "\\" + fileNames[fileIndex] + "." + extension;
                    if (!File.Exists(fileName)) { File.Create(fileName); throw new FileNotFoundException($"{fileName} was not found, so it was created."); }
                    string[] data = File.ReadAllLines(fileName);

                    // Make tile
                    object[] tileData = { data[5][0], Map.TryHexParse(data[5][1]), Map.TryHexParse(data[5][2]), 0 };
                    Tile tile = new Tile { displayChar = (char)tileData[0], fg = (ConsoleColor)tileData[1], bg = (ConsoleColor)tileData[2], hazard = (Hazard)tileData[3] };

                    // Create foe.
                    Foe foe = new Foe(int.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3]), int.Parse(data[4]), false)
                    {
                        name = data[0],
                        displayTile = tile,
                        movement = (Foe.movementType)int.Parse(data[6]),
                        attackEffect = statusEffects[int.Parse(data[7])],
                        weapon = (WeaponItem)items[int.Parse(data[8])],
                        armor = (ArmorItem)items[int.Parse(data[9])]
                    };
                    foes.Add(foe);
                    count++;
                }
                catch (Exception e) { Log.Write("Failed to load an entity: " + e.Message, logType.error); Log.Write(e.StackTrace, logType.debug); }
            }
            Log.Write($"Loaded {count} entities.");
        }

        private static Pickup[] LoadPickups(string location, string extension = "txt")
        {
            if (!Directory.Exists(location)) { Directory.CreateDirectory(location); File.Create(location + indexAdd); throw new DirectoryNotFoundException($"{location} was not found, so it was created."); }
            string[] fileNames = File.ReadAllLines(location + indexAdd);
            List<Pickup> pickups = new List<Pickup>();
            foreach (string file in fileNames)
            {
                try
                {
                    string fileName = location + "\\" + file + "." + extension;
                    if (!File.Exists(fileName)) { File.Create(fileName); throw new FileNotFoundException($"{fileName} was not found, so it was created."); }
                    string[] data = File.ReadAllLines(fileName);

                    int iPType = int.Parse(data[0]); Pickup.pickupType pType = (Pickup.pickupType)iPType;
                    int value = int.Parse(data[1]);
                    int iRType; Pickup.restorationType rType = 0;
                    int x = int.Parse(data[2]); int y = int.Parse(data[3]); Vector2 pos = new Vector2(x, y);
                    try { iRType = int.Parse(data[4]); rType = (Pickup.restorationType)iRType; } catch (Exception ignored) { }

                    Pickup pickup;
                    switch (pType)
                    {
                        case Pickup.pickupType.item: pickup = new Pickup(pos, items[value]); break;
                        case Pickup.pickupType.restoration: pickup = new Pickup(pos, rType, value); break;
                        default: throw new NotImplementedException($"{pType} is not implemented in pickup loader.");
                    }

                    pickups.Add(pickup);
                }
                catch (Exception e) { Log.Write("Failed to load a pickup: " + e.Message, logType.error); Log.Write(e.StackTrace, logType.debug); }
            }
            Log.Write($"Loaded {pickups.Count} pickups.");

            return pickups.ToArray();
        }

        // Load triggers from files.
        static Trigger[] LoadTriggers(string location, string extension = "txt")
        {
            if (!Directory.Exists(location)) { Directory.CreateDirectory(location); File.Create(location + indexAdd).Close(); throw new DirectoryNotFoundException($"{location} was not found, so it was created."); }
            string[] fileNames = File.ReadAllLines(location + indexAdd);
            List<Trigger> triggers = new List<Trigger>();


            foreach (string file in fileNames)
            {
                try
                {
                    string fileName = location + "triggers\\" + file + "." + extension;
                    if (!File.Exists(fileName)) { File.Create(fileName); throw new FileNotFoundException($"{fileName} was not found, so it was created."); }
                    string[] data = File.ReadAllLines(fileName);

                    Vector2 tc = new Vector2(int.Parse(data[0]), int.Parse(data[1]));
                    Vector2 bc = new Vector2(int.Parse(data[2]), int.Parse(data[3]));

                    triggers.Add(new Trigger(tc, bc, (triggerType)int.Parse(data[4]), int.Parse(data[5])));
                }
                catch (Exception e) { Log.Write($"Failed to load a trigger: {e.Message}", logType.error); Log.Write(e.StackTrace, logType.debug); }
            }
            Log.Write($"Loaded {triggers.Count} triggers.");

            return triggers.ToArray();
        }

        static void LoadAreas(string extension = "txt")
        {
            string dir = dirs[5];
            string[] fileNames = File.ReadAllLines(dir + indexAdd);
            areas = new List<Area>();

            foreach (string name in fileNames)
            {
                try
                {
                    string fileName = dir + "\\" + name + "." + extension;
                    if (!File.Exists(fileName)) { File.Create(fileName); throw new FileNotFoundException($"{fileName} was not found, so it was created."); }
                    string[] data = File.ReadAllLines(fileName);

                    string triggerLoc = dir + "\\" + name + "triggers";
                    string pickupsLoc = dir + "\\" + name + "pickups";
                    Trigger[] triggers = LoadTriggers(triggerLoc);
                    Pickup[] pickups = LoadPickups(pickupsLoc);

                    Area area = new Area(name);
                    area.pickups = pickups;
                    area.SetTriggers(triggers);

                    areas.Add(area);
                }
                catch (Exception e) { Log.Write($"Failed to load an area: {e.Message}", logType.error); Log.Write(e.StackTrace, logType.debug); }
            }
            Log.Write($"Loaded {areas.Count} areas");
        }
    }
}
