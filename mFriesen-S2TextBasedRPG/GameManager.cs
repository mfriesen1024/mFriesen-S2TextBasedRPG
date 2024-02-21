﻿using SimpleLogger;
using System;
using System.Collections.Generic;
using System.IO;

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

        static Random random;
        static int seed = DateTime.Now.Millisecond;

        public static string areasFName;
        static Area[] areas; // stores the areas.
        public static Foe[] foeTemplates; // store foe templates
        static Area currentArea; // tracks the current area
        static Map currentMap; // Track current map.
        static List<Entity> mobs = new List<Entity>();
        static List<Entity> displayEntities = new List<Entity>();
        static List<Pickup> pickups = new List<Pickup>();
        static string[] storedDialogue; // store dialogue (load from file)
        static string[] currentDialogue; // store the current dialogue passage to read

        public static void LoadAreas()
        {
            if (File.Exists(areasFName))
            {
                string[] aNames = File.ReadAllLines(areasFName);
                areas = new Area[aNames.Length];
                for (int i = 0; i < aNames.Length; i++)
                {
                    areas[i] = new Area(aNames[i]);
                }
            }
            else
            {
                File.Create(areasFName).Close();
                Log.Write($"Areas file didn't exist. Is {areasFName} correct?", logType.error);
                Environment.Exit(-1);
            }
        }

        public static void Start()
        {
            LoadData();

            // set first area active.
            LoadArea(0);

            currentMap.RenderMap();

            Run();
        }

        static void Run()
        {
            while (run) { Update(); }
        }

        public static void Update()
        {
            // At the start, render the map.
            currentMap.RenderMap(displayEntities.ToArray());

            // Attempt to get actions for each player.
            List<Vector2> targetLocs = new List<Vector2>();
            foreach (Entity e in mobs) { targetLocs.Add(e.GetAction()); }

            // Get positions for items
            List<Vector2> pickupLocs = new List<Vector2>();

            // Run action.
            for (int i = 0; i < targetLocs.Count; i++)
            {
                Vector2 target = targetLocs[i];
                Entity actor = mobs[i];
                Pickup pickup = PickupCheck(target);

                actionResult result = WallCheck(target, i.ToString());
                if (TryAttack(actor, target)) { result = actionResult.fail; }
                if (pickup != null)
                {
                    result = actionResult.fail;
                    if (i == 0) { ((Player)actor).UsePickup(pickup); pickups.Remove(pickup); displayEntities.Remove(pickup); }
                }

                if (result == actionResult.move)
                {
                    actor.position = target;
                }
            }

            // If any entity is dying, remove them.
            for (int i = 0; i < mobs.Count; i++)
            {
                Entity e = mobs[i];
                if (e.statManager.isDying) { mobs.RemoveAt(i); }
            }

            // End game if player died, and render the map to tell them that they have died.
            if (player.statManager.isDying) { run = false; currentMap.RenderMap(displayEntities.ToArray()); }

            // End game if all mobs are dead.
            TempWinCheck();
        }

        public static void LoadArea(int index)
        {
            currentArea = areas[index];

            mobs = new List<Entity> { player };
            mobs.AddRange(currentArea.encounter);
            displayEntities.AddRange(mobs);
            displayEntities.AddRange(currentArea.pickups);
            pickups.AddRange(currentArea.pickups);

            currentMap = currentArea.map;
        }

        static void LoadData()
        {
            // eventually, this should be replaced with proper data loading, but for now, just load temporary things.
            TemporaryDataManager.GenerateThings();
            foeTemplates = TemporaryDataManager.foes.ToArray();
            areas = TemporaryDataManager.areas.ToArray();
            player = TemporaryDataManager.player;
        }

        public static Random GetRandom()
        {
            seed++;
            return new Random(seed);
        }

        static actionResult WallCheck(Vector2 targetPos, string name = "")
        {
            actionResult result = actionResult.move; // default to move. override if not.
            try
            {
                Tile target = currentMap.GetMap()[targetPos.y, targetPos.x];
                if (target.hazard == Hazard.wall) { result = actionResult.fail; }
            }
            catch (IndexOutOfRangeException e)
            {
                result = actionResult.fail;
                Log.Write($"Encountered exception {e.GetType()}. Triggering entity is {name}. targetPos is {targetPos.x}, {targetPos.y}.", logType.debug);
            }
            finally { Log.Write($"Target Coords are {targetPos.x}, {targetPos.y}. Result is {result}", logType.debug); }
            return result;
        }

        static Pickup PickupCheck(Vector2 targetPos)
        {
            Pickup result = null;

            foreach (Pickup p in pickups)
            {
                if (p.position.Equals(targetPos))
                {
                    result = p; break;
                }
            }

            return result;
        }

        static bool TryAttack(Entity attacker, Vector2 attackPos)
        {
            bool result = false;
            foreach (Entity target in mobs)
            {
                if (attacker == target) { Log.Write("Entity attempted to attack itself!", logType.debug); continue; } // Damaging one's self is bad. Prevent entities from doing that.
                else if (attacker == null || target == null)
                {
                    string txt = "Attacker was null somehow! This definitely shouldn't happen.";
                    Log.Write(txt, logType.error); throw new ArgumentException(txt);
                }

                // store attacked position and the target's position.
                int ax = attackPos.x, ay = attackPos.y;
                int tx = target.position.x, ty = target.position.y;

                if (ax == tx && ay == ty)
                {
                    Log.Write($"Found entity at {ax}, {ay}. Running attack!", logType.debug);

                    // Record hp.
                    //int oldHP = target.GetStat(statname.hp);

                    // Now run attack things.
                    int damage = attacker.GetDamage();
                    target.TakeDamage(damage);
                    result = true;
                }
            }
            return result;
        }

        static void TempWinCheck()
        {
            if (mobs.Count == 1)
            {
                win = true;
                run = false;
            }
        }
    }
}
