///
/// Copyright 2017 Jeff Tickle "CMDR VirtualPaper"
/// 
/// This file is part of LibEDJournal.
///
/// LibEDJournal is free software: you can redistribute it and/or modify
/// it under the terms of the GNU General Public License as published by
/// the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.
///
/// LibEDJournal is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
/// GNU General Public License for more details.
///
/// You should have received a copy of the GNU General Public License
/// along with LibEDJournal.  If not, see<http://www.gnu.org/licenses/>.
///

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Newtonsoft.Json;

namespace LibEDJournal
{
    public class EliteJournalParser
    {
        /// <summary>
        /// The absolute path to the Elite log files
        /// </summary>
        public string LogFilePath { get; protected set; }

        /// <summary>
        /// The JsonSerializer that will be used to read log entries
        /// </summary>
        protected JsonSerializer Serializer;

        public EliteJournalParser()
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

        public DateTime GetMostRecentLogWrite()
        {
            DateTime largest = DateTime.MinValue;
            foreach(var f in Directory.EnumerateFiles(LogFilePath, "*.log"))
            {
                var current = File.GetLastWriteTime(f);
                if(current > largest)
                {
                    largest = current;
                }
            }

            return largest;
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

                            Serializer.Error += Serializer_Error;
                        }
                    }
                }
            }
        }

        private void Serializer_Error(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EliteJournalEntry> GetLogEntries()
        {
            return GetLogEntries(SortLogFiles(GetLogFiles()));
        }

        protected void AcceptHandlers(EliteJournalEntry entry,
            IList<EliteJournalHandler> handlers)
        {
            foreach(var handler in handlers)
            {
                entry.Accept(handler);
            }
        }

        public void HandleLogEntries(IList<EliteJournalHandler> handlers)
        {
            HandleLogEntries(GetLogEntries(), handlers);
        }

        public void HandleLogEntries(
            IEnumerable<EliteJournalEntry> logEntries,
            IList<EliteJournalHandler> handlers)
        {
            foreach(var entry in logEntries)
            {
                AcceptHandlers(entry, handlers);
            }
        }

        public static IEnumerable<EliteJournalEntry> EntriesSinceFilter(
            IEnumerable<EliteJournalEntry> logEntries,
            DateTime onlySince)
        {
            foreach (var entry in logEntries)
            {
                if (entry.Timestamp > onlySince)
                {
                    yield return entry;
                }
            }
        }

        public void HandleLogEntries(
            DateTime since,
            IList<EliteJournalHandler> handlers)
        {
            HandleLogEntries(
                EntriesSinceFilter(
                    GetLogEntries(), since),
                handlers);
        }
    }
}
