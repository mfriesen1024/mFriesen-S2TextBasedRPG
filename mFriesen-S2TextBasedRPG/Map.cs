using SimpleLogger;
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
            this.fName = "data\\maps\\" + fName;
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

            // Check if the map file exist, and if not, throw an exception.
            if (!File.Exists(fNames[0]))
            {
                string txt = $"Failed to load map {fNames[0]} due to nonexistent file.";
                File.Create(fNames[0]).Close();
                Log.Write(txt, logType.fatal);
                throw new FileNotFoundException(txt);
            }

            // get the length of files so we can use it for making a 2d array
            string[] getLength = File.ReadAllLines(fNames[0]);

            // Dim0 is fNames index, Dim1 is map vertical axis.
            //string[,] data = new string[4, getLength.Length];
            // Create the tile array, such that y (dim0) is string array index, and x (dim1) is string index.
            Tile[,] tiles = new Tile[getLength.Length, getLength[0].Length];
            Log.Write($"Tiles is size {getLength.Length}, {getLength[0].Length}", logType.debug);

            for (int f = 0; f < fNames.Length; f++)
            {
                // Check if the map files exist, and if not, throw an exception.
                if (!File.Exists(fNames[f]))
                {
                    string txt = $"Failed to load map {fNames[f]} due to nonexistent file.";
                    File.Create(fNames[f]).Close();
                    Log.Write(txt, logType.error);
                    throw new FileNotFoundException(txt);
                }

                // Read the file of the fName f.
                string[] lines = File.ReadAllLines(fNames[f]);

                // Convert lines to a 2d array of tile.
                for (int y = 0; y < tiles.GetLength(1); y++) // For loop for y axis.
                {
                    for (int x = 0; x < tiles.GetLength(0); x++) // For loop for x axis.
                    {
                        char c = lines[x][y];
                        string s = c.ToString();
                        int i = 0;
                        try { i = int.Parse(s); } catch (Exception ignored) { }

                        switch (f) // Switch based on the current fName.
                        {
                            case 0:
                                tiles[x, y].displayChar = c; break;
                            case 1:
                                tiles[x, y].fg = (ConsoleColor)i; break;
                            case 2:
                                tiles[x, y].bg = (ConsoleColor)i; break;
                            case 3:
                                tiles[x, y].hazard = (Hazard)i; break;
                        }
                    }
                }
            }
            this.tiles = tiles;

            Log.Write($"Loaded map {fName}, by loading {fNames.Length} files. Tiles has {tiles.Length} tiles.");
        }
        public void RenderMap()
        {
            // Create top/bottom borders.
            string end = string.Empty;
            for (int x = -2; x < tiles.GetLength(0); x++)
            {
                end += "#";
            }

            // Write border
            Console.WriteLine(end);

            // Render the main map
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                // write the border before the map.
                Console.ForegroundColor = ConsoleColor.White; Console.BackgroundColor = ConsoleColor.Black; Console.Write("#");

                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    Console.ForegroundColor = tiles[y, x].fg; Console.BackgroundColor = tiles[y, x].bg;
                    Console.Write(tiles[y, x].displayChar);
                }

                // write the border after the map.
                Console.ForegroundColor = ConsoleColor.White; Console.BackgroundColor = ConsoleColor.Black; Console.Write("#");
                Console.WriteLine();
            }

            // Write border
            Console.ForegroundColor = ConsoleColor.White; Console.BackgroundColor = ConsoleColor.Black; Console.WriteLine(end);
        }
        public void RenderRegion(Vector2 topLeft, Vector2 bottomRight) { }
    }
}
