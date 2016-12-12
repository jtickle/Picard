using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Picard
{
    public class PersistentState
    {
        protected string StateFile;
        protected JsonSerializer Serializer;

        public State CurrentState = null;

        public PersistentState()
        {
            // Set up state file name
            // Local data because it only matters on the system where you
            // have Elite installed
            string local = Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData);
            string file = @"TickleSoft\picard.state";
            StateFile = Path.Combine(local, file);
       
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
        }

        protected void create()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(StateFile));
            CurrentState = new State();
        }

        protected bool exists()
        {
            return File.Exists(StateFile);
        }

        protected void load()
        {
            using (StreamReader sr = new StreamReader(StateFile))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                CurrentState = Serializer.Deserialize<State>(reader);
            }
        }

        protected void persist()
        {
            using (StreamWriter sw = new StreamWriter(StateFile))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                Serializer.Serialize(writer, CurrentState);
            }
        }

        public IDictionary<string, int> CalculateCurrentInventory()
        {
            IDictionary<string, int> result = new Dictionary<string, int>();

            foreach(var time in CurrentState.History)
            {
                result = DeltaTools.Add(result, time.Value);
            }

            return result;
        }

        public bool HasInaraCreds()
        {
            return CurrentState.InaraU != "";
        }

        public bool HasHistory()
        {
            return CurrentState.History.Count > 0;
        }

        public DateTime GetLastUpdateTimestamp()
        {
            return CurrentState.History.First().Key;
        }

        public void UpdateInaraCreds(string user, string pass, string name)
        {
            if (user == CurrentState.InaraU
                && pass == CurrentState.InaraP
                && name == CurrentState.CmdrName)
                return;

            CurrentState.InaraU = user;
            CurrentState.InaraP = pass;
            CurrentState.CmdrName = name;
            persist();
        }

        public void AddHistory(IDictionary<string, int> d)
        {
            CurrentState.History.Add(DateTime.Now, d);

            foreach(var entry in d)
            {
                Console.WriteLine("EliteMatsLookup.Add(\"" + 
                    Regex.Replace(entry.Key.ToLower().ToString(), @"\s+", "") +
                    "\", \"" + 
                    entry.Key
                    + "\");");
            }
            persist();
        }
    }
}
