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

        public static void Startup(string[] directories)
        {
            dirs = directories;
            foreach (string d in directories)
            {
                Directory.CreateDirectory(d);
                string index = d + indexAdd;
                if (!File.Exists(index)) { File.Create(index).Close(); }
            }
        }

        public static void Load()
        {

        }

        void LoadEffects(string extension)
        {
            string dir = dirs[0];
            string[] fileNames = File.ReadAllLines(dir + indexAdd);
            statusEffects = new List<StatusEffect>();

            foreach (string file in fileNames)
            {
                try
                {
                    string[] data = File.ReadAllLines(dir + "\\" + file + "." + extension);

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

        void LoadItems(string extension)
        {
            string dir = dirs[1];
            string[] fileNames = File.ReadAllLines(dir + indexAdd);
            items = new List<Item>();

            foreach (string file in fileNames)
            {
                try
                {
                    string[] data = File.ReadAllLines(dir + "\\" + file + "." + extension);

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
        }
    }
}
