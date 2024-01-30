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

        static Random random;
        static int seed = DateTime.Now.Millisecond;

        public static string areasFName;
        static Area[] areas; // stores the areas.
        public static Foe[] foeTemplates; // store foe templates
        static Area currentArea; // tracks the current area
        static Map currentMap; // Track current map.
        public static List<Entity> entities = new List<Entity>();
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
        }

        public static void Update()
        {
            // Attempt to get actions for each player.
            List<Vector2> targetLocs = new List<Vector2>();
            foreach (Entity e in entities) { targetLocs.Add(e.GetAction()); }

            // Run action.
            for (int i = 0; i < targetLocs.Count; i++)
            {
                Vector2 target = targetLocs[i];
                Entity actor = entities[i];

                actionResult result = WallCheck(target, i.ToString());
                if (TryAttack(actor, target)) { result = actionResult.fail; }

                if(result == actionResult.move)
                {
                    actor.position = target;
                }
            }

            // If any entity is dying, remove them.
            foreach (Entity e in entities)
            {
                if (e.statManager.isDying) { entities.Remove(e); }
            }

            // End game if player died.
            if (player.statManager.isDying) { Program.run = false; }

            // At the end, render the map.
            currentMap.RenderMap(entities.ToArray());
        }

        public static void LoadArea(int index)
        {
            currentArea = areas[index];
            entities = new List<Entity> { player };
            entities.AddRange(currentArea.encounter);
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
                Tile target = currentMap.GetMap()[targetPos.x, targetPos.y];
                if (target.hazard == Hazard.wall) { result = actionResult.fail; }
            }
            catch (IndexOutOfRangeException e)
            {
                result = actionResult.fail;
                Log.Write($"Encountered exception {e.GetType()}. Triggering entity is {name}. targetPos is {targetPos.x}, {targetPos.y}.", logType.error);
            }
            finally { Log.Write($"Target Coords are {targetPos.x}, {targetPos.y}. Result is {result}", logType.debug); }
            return result;
        }

        static bool TryAttack(Entity attacker, Vector2 attackPos)
        {
            bool result = false;
            foreach (Entity target in entities)
            {
                if (attacker == target) continue; // Damaging one's self is bad. Prevent entities from doing that.
                else if (attacker == null || target == null)
                {
                    string txt = "Attacker was null somehow! This definitely shouldn't happen.";
                    Log.Write(txt, logType.error); throw new ArgumentException(txt);
                }

                int ax = attackPos.x, ay = attackPos.y;
                int tx = target.position.x, ty = target.position.y;

                if(ax == tx && ay == ty)
                {
                    // Now run attack things.
                    int damage = attacker.GetDamage();
                    target.TakeDamage(damage);
                    result = true;
                }
            }
            return result;
        }
    }
}
