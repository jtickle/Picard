using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibEDJournal;
using LibEDJournal.State;

namespace Picard.Lib
{
    public class PersistentState
    {
        public string StatePath { get; protected set; }
        public string StateFile { get; protected set; }
        protected JsonSerializer Serializer;

        public State CurrentState = null;

        public PersistentState()
        {
            // Set up state file name
            // Local data because it only matters on the system where you
            // have Elite installed
            string local = Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData);
            string path = @"TickleSoft";
            StatePath = Path.Combine(local, path); 
            string file = @"picard.state";
            StateFile = Path.Combine(StatePath, file);
       
            // Set us up a serializer to use
            Serializer = new JsonSerializer();
            Serializer.Formatting = Formatting.Indented;

            // Load Current State
            if(exists())
            {
                load();
            }
            else
            {
                create();
            }

            // Fix Last Post Timestamp if necessary
            if (CurrentState.History.Count > 0)
            {
                // If null and has history, set to last update
                if(CurrentState.LastPost == DateTime.MinValue)
                {
                    CurrentState.LastPost = GetLastUpdateTimestamp();
                }

                // If not null and has history, leave it alone
            }
            else
            {
                // Otherwise, set it to the minimum DateTime
                CurrentState.LastPost = DateTime.MinValue;
            }
        }

        /// <summary>
        /// Create a new State.
        /// Only really useful on construciton if no state file exists.
        /// </summary>
        protected void create()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(StateFile));
            CurrentState = new State();
        }

        /// <summary>
        /// Check if the State File Exists.
        /// </summary>
        /// <returns>
        /// Boolean representing the existence of the state file.
        /// </returns>
        protected bool exists()
        {
            return File.Exists(StateFile);
        }

        /// <summary>
        /// Load the State File
        /// </summary>
        protected void load()
        {
            using (JsonReader reader = new JsonTextReader(new StreamReader(StateFile)))
            {
                CurrentState = Serializer.Deserialize<State>(reader);
            }
        }

        /// <summary>
        /// Save State data to the State file
        /// </summary>
        public void Persist()
        {
            using (JsonWriter writer = new JsonTextWriter(new StreamWriter(StateFile)))
            {
                Serializer.Serialize(writer, CurrentState);
            }
        }

        /// <summary>
        /// Gets the current inventory from the perspective of the Picard state file
        /// </summary>
        /// <returns>
        /// A dictionary of all the materials and changes that have been recorded
        /// in the Picard state file
        /// </returns>
        public InventorySet CalculateCurrentInventory()
        {
            InventorySet result = new InventorySet();

            foreach(var time in CurrentState.History)
            {
                result = result + time.Value;
            }

            return result;
        }

        public void ApplyUpdates(InventorySet result)
        {
            var dm = DataMangler.GetInstance();

            var cshk = CurrentState.History.Keys;

            InventorySet patch =
                CurrentState.History[cshk.Max()];

            foreach (var update in dm.GetUpdates(result,
                CurrentState.DataVersion))
            {
                if(!result.ContainsKey(update))
                    result[update] = 0;
                if(!patch.ContainsKey(update))
                    patch[update] = 0;
            }

            if (patch.Count == 0)
                return;

            CurrentState.DataVersion = dm.DataVersion;
        }

        /// <summary>
        /// Determine if the state currently contains Inara authentication credentials
        /// </summary>
        /// <returns>A boolean representing whether this Inara credentials are available</returns>
        public bool HasInaraCreds()
        {
            return CurrentState.InaraU != "";
        }

        /// <summary>
        /// Determine if any history has been stored
        /// </summary>
        /// <returns>
        /// A boolean representing whether any history has been stored
        /// </returns>
        public bool HasHistory()
        {
            return CurrentState.History.Count > 0;
        }

        /// <summary>
        /// Gets the last time history was stored
        /// </summary>
        /// <returns>
        /// The last time that history was stored to state
        /// </returns>
        public DateTime GetLastUpdateTimestamp()
        {
            return CurrentState.History.Last().Key;
        }

        public void UpdateLastPostToCurrent()
        {
            if (CurrentState.History.Keys.Count > 0) {
                CurrentState.LastPost =
                    CurrentState.History.Keys.Last();
            }
        }

        /// <summary>
        /// Updates the Inara credentials stored in the state
        /// </summary>
        /// <param name="user">Inara Username</param>
        /// <param name="pass">Inara Password</param>
        /// <param name="name">Scraped Commander Name</param>
        public void UpdateInaraCreds(string user, string pass, string name)
        {
            if (user == CurrentState.InaraU
                && pass == CurrentState.InaraP
                && name == CurrentState.CmdrName)
                return;

            CurrentState.InaraU = user;
            CurrentState.InaraP = pass;
            CurrentState.CmdrName = name;
        }

        /// <summary>
        /// Adds a set of materials to the history
        /// </summary>
        /// <param name="d">
        /// The current set of materials from this run
        /// </param>
        public void AddHistory(InventorySet d)
        {
            var now = DateTime.UtcNow;
            if(CurrentState.History.ContainsKey(now))
            {
                CurrentState.History[now] += d;
            }
            else
            {
                CurrentState.History.Add(DateTime.UtcNow, d);
            }
        }
    }
}
