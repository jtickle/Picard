using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Picard
{
    using IDeltas = IDictionary<string, int>;
    using Deltas = Dictionary<string, int>;

    class PersistentState
    {
        protected string StateFile;
        protected JsonSerializer Serializer;

        public State CurrentState = null;

        // General settings and info
        public class State
        {
            public DateTime LastInaraTimestamp;
            public DateTime LastEliteTimestamp;
            public string CmdrName;
            public string InaraU;
            public string InaraP;
            public IDictionary<DateTime, IDeltas> History;

            public State()
            {
                History = new SortedDictionary<DateTime, IDeltas>();
            }
        }

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

        public bool HasInaraCreds()
        {
            return CurrentState.InaraU != "";
        }

        public bool HasHistory()
        {
            return CurrentState.History.Count > 0;
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

        public IDeltas CreateDeltas()
        {
            return new Deltas();
        }

        public IDeltas CreateEmptyDelta()
        {
            return new Deltas();
        }

        public void AddHistory(IDeltas d)
        {
            CurrentState.History.Add(DateTime.Now, d);
            persist();
        }
    }
}
