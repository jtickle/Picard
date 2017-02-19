using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Picard.Lib
{
    public static class Filesystem
    {
        public static string OldStateFile
        {
            get
            {
                return GetRelativeAppData(
                    "TickleSoft",
                    "picard.state");
            }
            private set { }
        }
                
        public static string CombinePaths(string path, params string[] paths)
        {
            foreach(var p in paths)
            {
                path = Path.Combine(path, p);
            }

            return path;
        }

        public static string GetRelativeAppData(params string[] paths)
        {
            return CombinePaths(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                paths);
        }

        public static string GetRelativeUserHome(params string[] paths)
        {
            return CombinePaths(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.UserProfile),
                paths);
        }

        public static string GetRelativeStatePath(params string[] paths)
        {
            return CombinePaths(GetRelativeAppData("TickleSoft"), paths);
        }

        public static string GuessJournalPath()
        {
            var home = GetRelativeUserHome();

            var possibilities = Directory.EnumerateDirectories(home);

            foreach(var possibility in possibilities)
            {
                var journalPath = Path.Combine(possibility,
                    "Frontier Developments",
                    "Elite Dangerous");
                if(Directory.Exists(journalPath))
                {
                    var journals =
                        Directory.EnumerateFiles(journalPath, "Journal.*.log");
                    if (journals.Count() > 0)
                        return journalPath;
                }
            }

            return null;
        }

        public static bool DoesFolderHaveJournals(string path)
        {
            var logs = Directory.EnumerateFiles(path, "Journal.*.log");
            return logs.Count() > 0;
        }

        public static string GetStatePath(string Cmdr)
        {
            return GetRelativeStatePath(Cmdr + ".json");
        }
    }
}
