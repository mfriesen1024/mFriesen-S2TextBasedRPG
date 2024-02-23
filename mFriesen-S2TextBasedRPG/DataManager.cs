﻿using SimpleLogger;
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
            Log.Write($"Loaded {items.Count} effects.");
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
            catch (Exception e) { Log.Write("Failed to load an effect: " + e.Message, logType.error); Log.Write(e.StackTrace, logType.debug); }

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
                    Foe foe = new Foe(int.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3]), int.Parse(data[4]))
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
    }
}