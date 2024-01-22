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
        public static string indexFName = "index";

        public static string[] items;
        public static string[] foes;
        public static string[] encounters;

        public static void Load(string[] dirs)
        {
            LoadEncounters(dirs[2]);
        }

        static void LoadEncounters(string dirName)
        {
            dirName += "\\"; string _indexFName = dirName + indexFName;
            string[] fNames;

            if(File.Exists(_indexFName))
            {
                fNames = File.ReadAllLines(_indexFName);
            }
            else { throw new FileNotFoundException(_indexFName + "Was not found."); }
        }
    }
}
