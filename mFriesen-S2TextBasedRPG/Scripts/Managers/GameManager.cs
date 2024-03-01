using SimpleLogger;
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
        public static Map currentMap; // Track current map.
        static List<Mob> mobs = new List<Mob>();
        static List<Entity> displayEntities = new List<Entity>();
        static List<Pickup> pickups = new List<Pickup>();
        static string[] storedDialogue; // store dialogue (load from file)
        static string[] currentDialogue; // store the current dialogue passage to read

        static string tempWinText = "You won.";
        static string tempLoseText = "You died.";

        

        public static void Start()
        {
            DataManager.Init();
            LevelManager.Init();

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
            // At the start, render the map.
            currentMap.RenderMap(displayEntities.ToArray());

            // Set last encountered foe to null;
            HUD.recentFoe = null;

            // Attempt to get actions for each player.
            List<Vector2> targetLocs = new List<Vector2>();
            foreach (Entity e in mobs) { targetLocs.Add(e.GetAction()); }

            // Get positions for items
            List<Vector2> pickupLocs = new List<Vector2>();

            // Run action.
            for (int i = 0; i < targetLocs.Count; i++)
            {
                Vector2 target = targetLocs[i];
                Mob actor = mobs[i];
                Pickup pickup = PickupCheck(target);

                actionResult result = WallCheck(target, i.ToString());
                if (TryAttack(actor, target)) { result = actionResult.fail; }
                if (pickup != null)
                {
                    result = actionResult.fail;
                    if (i == 0) { ((Player)actor).UsePickup(pickup); pickups.Remove(pickup); displayEntities.Remove(pickup); }
                }

                // now check if immobile, and if true, cancel movement.
                if ((actor is Foe && ((Foe)actor).movement == Foe.movementType.stationary)|| actor.immobilized) { result = actionResult.fail; }

                if (result == actionResult.move)
                {
                    actor.position = target;
                }

                if (actor.TickEffect()) { actor.currentEffect = null; }
            }

            // If any entity is dying, remove them.
            for (int i = 0; i < mobs.Count; i++)
            {
                Mob m = mobs[i];
                if (m.statManager.isDying) { mobs.Remove(m); displayEntities.Remove(m); }
            }

            // End game if player died, and render the map to tell them that they have died.
            if (player.statManager.isDying) { run = false; currentMap.RenderMap(displayEntities.ToArray()); }

            currentArea.CheckTriggers(player);

            // End game if all mobs are dead.
            //TempWinCheck();
        }

        public static void LoadArea(int index)
        {
            Log.Write($"Loading area {index}");
            currentArea = areas[index];

            mobs = new List<Mob> { player };
            foreach (Entity e in currentArea.encounter) { mobs.Add((Mob)e); }

            displayEntities = new List<Entity>();
            pickups = new List<Pickup>();

            displayEntities.AddRange(mobs);
            displayEntities.AddRange(currentArea.pickups);
            pickups.AddRange(currentArea.pickups);

            currentMap = currentArea.map;
        }

        static void LoadData() // This system is currently spaghettified. Refactoring should be done soon:tm:.
        {
            foeTemplates = DataManager.foes.ToArray();
            areas = DataManager.areas.ToArray();
            player = DataManager.player;
            HUD.player = player;
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

        static bool TryAttack(Mob attacker, Vector2 attackPos)
        {
            bool result = false;
            foreach (Mob target in mobs)
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
                    // int oldHP = target.GetStat(statname.hp);

                    // Now run attack things.
                    int damage = attacker.statManager.GetDamage();
                    if (attacker.attackEffect != null) { target.currentEffect = attacker.attackEffect; }
                    target.statManager.TakeDamage(damage);
                    result = true;

                    if (attacker == player) { HUD.recentFoe = (Foe)target; }
                }
            }
            return result;
        }
    }
}
