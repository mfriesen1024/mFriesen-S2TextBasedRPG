using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace mFriesen_S2TextBasedRPG
{
    internal class DataManager
    {
        static string[] dirs;

        // Data goes here.
        public static List<StatusEffect> statusEffects;

        public static void Startup(string[] directories)
        {
            dirs = directories;
            foreach (string d in directories)
            {
                Directory.CreateDirectory(d);
                string index = d + "\\index.txt";
                if (!File.Exists(index)) { File.Create(index).Close(); }
            }
        }

        public static void Load()
        {

        }

        void LoadEffects()
        {
            string
        }
    }
}
