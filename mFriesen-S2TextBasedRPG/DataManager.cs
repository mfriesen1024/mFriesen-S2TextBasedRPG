using SimpleLogger;
using System;
using System.IO;

namespace mFriesen_S2TextBasedRPG
{
    static class DataManager
    {
        // File management
        static string indexFileName = "index.txt";
        public static string[] folderNames; // Note that this needs to be filled before calling any load functions.
        static string[] indexFileNames;
        static int foeFolderIndex = 3;

        // Loaded Data
        static Foe[] foes; 

        public static void VerifyFolders() // Verifies that the folders requested exist.
        {
            for (int i = 0; i < folderNames.Length; i++)
            {
                // Sort filenames
                string folderName = folderNames[i];
                string fileName = folderName + "\\" + indexFileName;
                indexFileNames[i] = fileName;

                // File checks
                if (!Directory.Exists(folderName))
                {
                    Log.Write($"Missing folder {folderName}, creating it.", logType.warning);
                    Directory.CreateDirectory(folderName);
                }
                if (!File.Exists(folderName))
                {
                    Log.Write($"Missing file {fileName}, creating it.", logType.warning);
                    File.Create(fileName).Close();
                }
            }
        }

        public static void LoadFoes()
        {
            // Read files and init arrays
            string[] foeNames = File.ReadAllLines(indexFileNames[foeFolderIndex]);
            int foeCount = foeNames.Length;
            foes = new Foe[foeCount];

            for (int i = 0;i < foeCount;i++)
            {
                string fName = foeNames[i]; // Get file name
                string[] dataStrings = File.ReadAllLines(fName); // Read file
                int[] dataInts = new int[dataStrings.Length]; // Create array with same length as string array
                for (int j = 0; j < dataStrings.Length; j++)
                {
                    try { dataInts[j] = int.Parse(dataStrings[j]); } catch (Exception ignored) { } // Parse the string into an int.
                }

                foes[i] = new Foe(dataInts[0], dataInts[1], dataInts[2], dataInts[3]);
            }
        }
    }
}
