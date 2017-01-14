using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Newtonsoft.Json;
using Picard.Lib.JournalEntry;

namespace Picard.Lib
{
    public class EliteLogs
    {
        /// <summary>
        /// The absolute path to the Elite log files
        /// </summary>
        public string LogFilePath { get; protected set; }

        /// <summary>
        /// The JsonSerializer that will be used to read log entries
        /// </summary>
        protected JsonSerializer Serializer;

        public EliteLogs()
        {
            // Get User Profile Folder
            string profile = Environment.GetFolderPath(
                Environment.SpecialFolder.UserProfile);

            // Elite Log Path relative to User Profile Folder
            string eliteLogPath = @"Saved Games\Frontier Developments\Elite Dangerous";

            // Combine to get the log file path
            LogFilePath = Path.Combine(profile, eliteLogPath);
            
            // Initialize our serializer which can be reused
            Serializer = new JsonSerializer();
        }
        /// <summary>
        /// Get the absolute paths of all log files
        /// </summary>
        /// <returns>
        /// An IEnumerable of all the absolute paths of all Elite log files
        /// </returns>
        public IEnumerable<string> GetLogFiles()
        {
            // Return all log files
            return Directory.EnumerateFiles(LogFilePath, "*.log");
        }

        /// <summary>
        /// Sorts log files by last modified time
        /// </summary>
        /// <param name="logFiles">An IEnumerable of strings of log file names</param>
        /// <returns>The sorted set of log file names</returns>
        public IEnumerable<string> SortLogFiles(IEnumerable<string> logFiles)
        {
            // Sort log files by write time
            return from logFile in logFiles
                   orderby File.GetLastWriteTime(logFile)
                   select logFile;
        }

        /// <summary>
        /// Extracts all entries from a list of absolute paths to log files
        /// </summary>
        /// <param name="logFiles">A list of absolute paths to log files</param>
        /// <returns>A set of log entries represented as parsed JSON</returns>
        public IEnumerable<EliteJournalEntry> GetLogEntries(IEnumerable<string> logFiles)
        {

            // Loop over all log files
            foreach (var file in logFiles)
            {
                using (var fileReader = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var streamReader = new StreamReader(fileReader))
                {
                    // Loop over all lines in the log file
                    while (!streamReader.EndOfStream)
                    {
                        var line = streamReader.ReadLine();

                        // Parse and read a line
                        using (var lineReader = new StringReader(line))
                        using (var jsonReader = new JsonTextReader(lineReader))
                        {
                            yield return
                                Serializer.Deserialize<EliteJournalEntry>(jsonReader);

                        }
                    }
                }
            }
        }
        
        public IEnumerable<EliteJournalEntry> GetLogEntries()
        {
            return GetLogEntries(SortLogFiles(GetLogFiles()));
        }

        public void HandleLogEntries(EliteLogMaterialHandler handler)
        {
            foreach(var entry in GetLogEntries())
            {
                if(entry.GetType() == typeof(EngineerCraft))
                {
                    handler.Handle((EngineerCraft)entry);
                }
                else if (entry.GetType() == typeof(EngineerProgress))
                {
                    handler.Handle((EngineerProgress)entry);
                }
                else if (entry.GetType() == typeof(MarketBuy))
                {
                    handler.Handle((MarketBuy)entry);
                }
                else if (entry.GetType() == typeof(MarketSell))
                {
                    handler.Handle((MarketSell)entry);
                }
                else if (entry.GetType() == typeof(MaterialCollected))
                {
                    handler.Handle((MaterialCollected)entry);
                }
                else if (entry.GetType() == typeof(MaterialDiscarded))
                {
                    handler.Handle((MaterialDiscarded)entry);
                }
                else if (entry.GetType() == typeof(MissionAccepted))
                {
                    handler.Handle((MissionAccepted)entry);
                }
                else if (entry.GetType() == typeof(MissionCompleted))
                {
                    handler.Handle((MissionCompleted)entry);
                }
                else if (entry.GetType() == typeof(Synthesis))
                {
                    handler.Handle((Synthesis)entry);
                }
            }
        }
    }
}
