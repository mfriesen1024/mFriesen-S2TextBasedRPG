﻿using SimpleLogger;
using System;
using System.IO;

namespace mFriesen_S2TextBasedRPG
{
    internal class Map
    {
        public Entity[] entities;
        Tile[,] tiles;
        string fName;

        public Map(string fName)
        {
            this.fName = "data\\maps\\"+fName;
            LoadMap(this.fName);
        }

        public Tile[,] GetMap() { return tiles; }
        public void LoadMap(string fName)
        {
            // Set file names to read
            string[] fNames = new string[4];
            fNames[0] = fName + "char.txt";
            fNames[1] = fName + "fg.txt";
            fNames[2] = fName + "bg.txt";
            fNames[3] = fName + "hazard.txt";

            // get the length of files so we can use it for making a 2d array
            string[] getLength = { " ", " "};
            try { getLength = File.ReadAllLines(fNames[0]); } catch { }

            // Dim0 is fNames index, Dim1 is map vertical axis.
            string[,] data = new string[4, getLength.Length];
            // Create the tile array, such that y (dim0) is string array index, and x (dim1) is string index.
            Tile[,] tiles = new Tile[getLength.Length, getLength[0].Length];

            for(int f = 0; f < fNames.Length; f++)
            {
                // Check if the map files exist, and if not, throw an exception.
                if (!File.Exists(fNames[f]))
                {
                    string txt = $"Failed to load map {fNames[f]} due to nonexistent file.";
                    File.Create(fNames[f]) ;
                    Log.Write(txt, logType.fatal);
                    throw new FileNotFoundException(txt);
                }

                // Read the file of the fName f.
                string[] lines = File.ReadAllLines(fNames[f]);

                // Convert lines to a 2d array of tile.
                for (int y = 0; y < tiles.GetLength(1); y++) // For loop for y axis.
                {
                    for (int x = 0; x < tiles.GetLength(0); x++) // For loop for x axis.
                    {
                        char c = lines[y][x];

                        switch (f) // Switch based on the current fName.
                        {
                            case 0:
                                tiles[y,x].displayChar = c; break;
                                case 1:
                                tiles[y,x].fg = (ConsoleColor)(int)c; break;
                                case 2:
                                tiles[y,x].bg = (ConsoleColor)(int)c;   break;
                                case 3:
                                tiles[y,x].hazard = (Hazard)(int)c; break;
                        }
                    }
                }
            }

            Log.Write($"Loaded map {fName}, by loading {fNames.Length} files. Tiles has {tiles.Length} tiles.");
        }
        public void RenderMap() { }
        public void RenderRegion(Vector2 topLeft, Vector2 bottomRight) { }
    }
}
