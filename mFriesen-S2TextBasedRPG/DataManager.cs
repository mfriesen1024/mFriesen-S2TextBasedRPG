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
            string[] fileNames = File.ReadAllLines(dirs[0] + indexAdd);
            statusEffects = new List<StatusEffect>();

            foreach (string file in fileNames)
            {
                string[] data = File.ReadAllLines(file + "." + extension);

                statusEffects.Add(new StatusEffect(
                    type: (effectType)int.Parse(data[0]),
                    name: data[1],
                    timer: int.Parse(data[2]),
                    value: int.Parse(data[3])
                    ));
            }
        }
    }
}
