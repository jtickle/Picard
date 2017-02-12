using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibEDJournal;
using LibEDJournal.Handler;
using Picard.Lib;

namespace Picard.CheapHack
{
    public class CheapHackController
    {
        protected EliteJournalParser Logs;
        protected PersistentState State;

        protected ChooseCmdrForm Form;

        public CheapHackController(EliteJournalParser logs,
            PersistentState state)
        {
            Logs = logs;
            State = state;
        }

        public void Run()
        {
            var ChHandler = new CharacterHandler();
            var handlers = new List<EliteJournalHandler>() { ChHandler };
            Logs.HandleLogEntries(DateTime.MinValue, handlers);

            if(ChHandler.Cmdrs.Count < 1)
            {
                // TODO: Play the game a bit
                return;
            }
            else if(ChHandler.Cmdrs.Count == 1)
            {
                State.CurrentState.EliteCmdrName =
                    ChHandler.Cmdrs.First();
                State.Persist();
                return;
            }

            Form = new ChooseCmdrForm();
            foreach(var cmdr in ChHandler.Cmdrs)
            {
                Form.AddCmdr(cmdr);
            }

            Form.CommanderChosen += HandleCommanderChosen;
            Form.ShowDialog();
        }

        public void HandleCommanderChosen(object sender, EventArgs e)
        {
            State.CurrentState.EliteCmdrName = Form.SelectedCommander;
            State.Persist();
        }
    }
}
