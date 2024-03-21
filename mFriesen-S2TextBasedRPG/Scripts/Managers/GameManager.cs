using SimpleLogger;
using System;
using System.Collections.Generic;

namespace mFriesen_S2TextBasedRPG
{
    enum actionResult // to be updated if more hazards are added.
    {
        move,
        fail
    }

    static class GameManager
    {
        public static Player player;

        public static bool run = true; // This will be disabled when we want to end the game.
        public static bool win = false; // this is enabled if we want win dialogue.

        public static Map currentMap; // Track current map.

        static string tempWinText = "You won.";
        static string tempLoseText = "You died.";



        public static void Start()
        {
            DataManager.Init();
            LevelManager.Init();

            HUD.Init();

            currentMap.RenderMap();

            Run();
        }

        static void Run()
        {
            while (run) { Update(); }

            Console.Clear();
            if (win) { Console.WriteLine(tempWinText); }
            else { Console.WriteLine(tempLoseText); }
        }

        public static void Update()
        {
            LevelManager.Update();
        }
    }
}
