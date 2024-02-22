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

                        if(f > 0) // if fileIndex is greater than characterfile, try convert from hex to int.
                        {
                            try { i = int.Parse(s); } catch (Exception ignored) {
                                switch (c)
                                {
                                    case 'a': i = 10; break;
                                    case 'b': i = 11; break;
                                    case 'c': i = 12; break;
                                    case 'd': i = 13; break;
                                    case 'e': i = 14; break;
                                    case 'f': i = 15; break;
                                }
                            }
                        }

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
        public void RenderMap(Entity[] entities = null)
        {
            // Run clear first, make sure console is clear before we do crap.
            Console.Clear();

            // Check if things are null, then add entities.
            if (entities == null)
            {
                Log.Write("Entities are null. This may be intentional, it may not.", logType.warning);
            }
            // create local tile array so I don't break the array when I add entities.
            Tile[,] localTiles = tiles;
            if (entities != null)
            {
                localTiles = AddEntitiesToMap(localTiles, entities);
            }
            else if (this.entities != null)
            {
                localTiles = AddEntitiesToMap(localTiles, this.entities);
            }

            // Create top/bottom borders.
            string end = string.Empty;
            for (int x = -2; x < localTiles.GetLength(1); x++)
            {
                end += "#";
            }

            // Write border
            Console.WriteLine(end);

            // Render the main map
            for (int x = 0; x < localTiles.GetLength(0); x++)
            {
                // write the border before the map.
                Console.ForegroundColor = ConsoleColor.White; Console.BackgroundColor = ConsoleColor.Black; Console.Write("#");

                for (int y = 0; y < localTiles.GetLength(1); y++)
                {
                    Console.ForegroundColor = localTiles[x, y].fg; Console.BackgroundColor = localTiles[x, y].bg;
                    Console.Write(localTiles[x, y].displayChar);
                }

                // write the border after the map.
                Console.ForegroundColor = ConsoleColor.White; Console.BackgroundColor = ConsoleColor.Black; Console.Write("#");
                Console.WriteLine();
            }

            // Write border
            Console.ForegroundColor = ConsoleColor.White; Console.BackgroundColor = ConsoleColor.Black; Console.WriteLine(end);
        
            // Hud update should be called upon rendering map, so do that now.
            HUD.Update();
        }

        /*
         * Input:
         * Vector2 topLeft, bottomRight; records the coordinates of corners.
         * Result: renders the region defined by the corners.
         */
        public void RenderRegion(Vector2 topLeft, Vector2 bottomRight) { }

        Tile[,] AddEntitiesToMap(Tile[,] tiles, Entity[] entities)
        {
            // create local tile array so I don't break the array when I add entities.
            Tile[,] localTiles = new Tile[tiles.GetLength(0), tiles.GetLength(1)];
            // for each tile
            for (int y = 0; y < localTiles.GetLength(0); y++)
            {
                for (int x = 0; x < localTiles.GetLength(1); x++)
                {
                        localTiles[y, x] = tiles[y, x]; // set tile to original position first to avoid headache.

                    // Check if the position matches an entity position
                    for (int posIndex = 0; posIndex < entities.Length; posIndex++)
                    {

                        Vector2 currentPos = entities[posIndex].position;
                        if (currentPos.x == x && currentPos.y == y) // if the position matches, set the tile.
                        {
                            Tile newTile = entities[posIndex].displayTile;
                            localTiles[y, x] = newTile;
                            // Log.Write($"Found entity id {posIndex} at position {currentPos.y}, {currentPos.x}. Displaytile details: {newTile.displayChar}", logType.debug);
                        }
                    }
                }
            }
            return localTiles;
        }
    }
}
