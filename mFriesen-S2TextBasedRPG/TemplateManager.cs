using SimpleLogger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mFriesen_S2TextBasedRPG
{
    static class TemplateManager
    {
        public static string indexFName = "index.txt";

        public static ItemTemplate[] items;
        public static string[,] foes;
        public static EncounterTemplate[] encounters;

        public static void Load(string[] dirs)
        {
            LoadEncounters(dirs[2]);
        }

        static void LoadEncounters(string dirName)
        {
            dirName = FixDirName(dirName);
            string _indexFName = dirName + indexFName;
            string[] fNames;

            if(File.Exists(_indexFName))
            {
                fNames = File.ReadAllLines(_indexFName);

                // fix names
                for(int i = 0; i < fNames.Length; i++)
                {
                    fNames[i] = dirName + fNames[i];
                }

                // Initialize the array for all the encounter templates.
                encounters = new EncounterTemplate[fNames.Length];

                for(int i = 0;i < fNames.Length; i++)
                {
                    Log.Write($"Attempting to load file {fNames[i]}", logType.debug);

                    // Lines 0-3 are dialogue indexes, all further lines are foe template indexes.
                    string[] lines = File.ReadAllLines(fNames[i]);

                    List<Foe> foes = new List<Foe>();

                    // Load foes here.

                    Vector2 sd, ed;
                    sd = new Vector2(int.Parse(lines[0]), int.Parse(lines[1])); ed = new Vector2(int.Parse(lines[2]), int.Parse(lines[3]));

                    // Place encounter in array as template.
                    encounters[i] = new EncounterTemplate(sd, ed, foes.ToArray());
                }
            }
            // To not have to worry about not getting anything back, just throw an exception.
            else { throw new FileNotFoundException(_indexFName + "Was not found."); }
        }
        
        static string FixDirName(string dirName)
        {
            // add a \ to the end of dirname,
            dirName += "\\";
            return dirName;
        }
    }

    struct ItemTemplate
    {
        string name;

        // other stats go here.
        //object example;

        public ItemTemplate(string name)
        {
            this.name = name;
        }
    }

    struct EncounterTemplate
    {
        Vector2 startDialogue;
        Vector2 endDialogue;
        Foe[] foes;

        public EncounterTemplate(Vector2 startDialogue, Vector2 endDialogue, Foe[] foes)
        {
            this.startDialogue = startDialogue;
            this.endDialogue = endDialogue;
            this.foes = foes;
        }
    }

    // create entity templates later, due to stat management rework.
}
