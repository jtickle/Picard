using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using LibEDJournal;
using LibEDJournal.Handler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Picard.Lib;

namespace Picard.CmdrChooser
{
    enum JournalDirResult
    {
        OK,
        DoesntExist,
        NoJournals
    }

    public class ChooseCmdrController
    {
        public string JournalPath { get; protected set; }
        public string CmdrName { get; protected set; }

        protected Configuration Config;
        protected ChooseCmdrForm Form;
        protected bool OneCmdr = false;

        public ChooseCmdrController(Configuration cfg)
        {
            Config = cfg;
        }

        public bool Run()
        {
            // If we have already detected that there is one commander,
            // then it is time to exit.
            if (OneCmdr)
                return false;

            // Try to load Journal path from Registry
            var pathGuess = Config.JournalPath;

            // If that is no good, then try to guess the save game path
            if (pathGuess == null)
                pathGuess = Filesystem.GuessJournalPath();

            // If that is no good, get the user home directory
            if (pathGuess == null)
                pathGuess = Filesystem.GetRelativeUserHome();

            Form = new ChooseCmdrForm(pathGuess);
            Form.SetInvalidJournalState();

            // See if the path contains journals
            if(TestJournalDirectory(pathGuess) == JournalDirResult.OK)
            {
                // Path contains journals
                JournalPath = pathGuess;
                Form.SetNoCmdrSelectedState();

                // If there is only one commander, select it and return
                var cmdrs = GetCmdrs(JournalPath);
                if(cmdrs.Count == 1)
                {
                    CmdrName = cmdrs.First();
                    OneCmdr = true;
                    return true;
                }

                // Otherwise, populate the form
                Form.AddCmdrs(cmdrs);

                // Select the last commander if applicable
                Form.SelectedCmdr = Config.LastCmdr;
            }

            // If we get this far, then show the form so the user can
            // make some decisions
            Form.Load += (o, e) => Form.FirstLoad = false;
            Form.LocationSelected += PathChanged;
            Form.CmdrSelected += OnCmdrSelected;

            var ret = Form.ShowDialog() == DialogResult.OK;

            Config.JournalPath = Form.JournalPath;

            return ret;
        }

        public void OnCmdrSelected(object o, EventArgs e)
        {
            CmdrName = Form.SelectedCmdr;
            Form.SetCmdrSelectedState();

            Config.LastCmdr = CmdrName;
        }

        JournalDirResult TestJournalDirectory(string path)
        {
            if (path == null || !Directory.Exists(path))
                return JournalDirResult.DoesntExist;

            if (!Filesystem.DoesFolderHaveJournals(path))
                return JournalDirResult.NoJournals;

            return JournalDirResult.OK;
        }

        public ICollection<string> GetCmdrs(string journalPath)
        {
            CharacterHandler ChHandler = new CharacterHandler();
            EliteJournalParser Logs = new EliteJournalParser(journalPath);

            foreach(var entry in Logs.GetLogEntries())
            {
                entry.Accept(ChHandler);
            }

            return ChHandler.Cmdrs;
        }

        public void PathChanged(object o, EventArgs e)
        {
            Form.ClearCmdrs();

            Form.SetInvalidJournalState();

            var result = TestJournalDirectory(Form.JournalPath);
            switch(result)
            {
                case JournalDirResult.OK:
                    // Path contains journals, commander is selected
                    Form.AddCmdrs(GetCmdrs(JournalPath));
                    Form.SetNoCmdrSelectedState();
                    Form.FirstLoad = false;

                    // If a commander has been previously selected, do it agian
                    Form.SelectedCmdr = Config.LastCmdr;
                    break;
                case JournalDirResult.DoesntExist:
                    Form.ShowDirectoryNotExists();
                    break;
                case JournalDirResult.NoJournals:
                    Form.ShowDirectoryNotJournal();
                    break;
            }

            Form.FirstLoad = false;
        }
    }
}
