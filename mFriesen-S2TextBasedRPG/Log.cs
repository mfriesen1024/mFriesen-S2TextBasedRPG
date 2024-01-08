using System;
using System.IO;

namespace SimpleLogger
{
    public enum logType
    {
        debug, info, warning, error, fatal, special
    }
    internal class Log
    {
        string fName;

        public void SetName(string fName = "Log", string dir = "Logs", bool useDate = true)
        {
            // Get date
            DateTime time = DateTime.Now;

            // Set file names as needed
            dir += "\\";
            if (useDate) { fName += time.ToShortDateString() + ".txt"; }
            else { fName += ".txt"; }
            fName = dir + fName;
            this.fName = fName;

            Directory.CreateDirectory(dir);
            if (!File.Exists(fName)) { File.Create(fName).Close(); }
        }

        public void Write(string text, logType type = logType.info)
        {
            // Make vars
            string time = DateTime.Now.ToShortTimeString();
            string writeTxt;
            string typeTxt = "";

            // Set logtype string
            switch (type)
            {
                case logType.debug:
                    typeTxt = "Debug";
                    break;
                case logType.info:
                    typeTxt = "Info";
                    break;
                case logType.warning:
                    typeTxt = "Warning";
                    break;
                case logType.error:
                    typeTxt = "Error";
                    break;
                case logType.fatal:
                    typeTxt = "FATAL";
                    break;
                case logType.special:
                    typeTxt = "Special";
                    break;
            }

            // Set the text to write.
            writeTxt = $"[{typeTxt}]:{time}: {text}";

            // Write file
            StreamWriter sw = File.AppendText(fName);
            sw.WriteLine(writeTxt);
            sw.Close();
        }
    }
}
