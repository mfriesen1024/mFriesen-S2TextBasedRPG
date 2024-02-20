using SimpleLogger;
using System.IO;

namespace mFriesen_S2TextBasedRPG
{
    static class DataManager
    {
        // File management
        static string indexFileName = "index.txt";
        static string[] folderNames; // Note that this needs to be filled before calling any load functions.
        static string[] indexFileNames;

        static void VerifyFolders() // Verifies that the folders requested exist.
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
                    File.Create(fileName);
                }
            }
        }
    }
}
