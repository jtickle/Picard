using System;
using System.Collections.Generic;
using LibEDJournal.Entry;

namespace LibEDJournal.Handler
{
    public delegate void DeathHandler(object sender,
        DeathEventArgs e);
    public delegate void ResurrectHandler(object sender,
        ResurrectEventArgs e);

    public class CharacterHandler : EliteJournalHandler
    {
        public event DeathHandler CharacterDied;
        public event ResurrectHandler CharacterResurrected;

        public ISet<string> Cmdrs;

        public CharacterHandler()
        {
            Cmdrs = new HashSet<string>();
        }

        public override void Handle(EngineerCraft e)
        {
        }

        public override void Handle(MarketBuy e)
        {
        }

        public override void Handle(MaterialCollected e)
        {
        }

        public override void Handle(MissionAccepted e)
        {
        }

        public override void Handle(ScientificResearch e)
        {
        }

        public override void Handle(Synthesis e)
        {
        }

        public override void Handle(MissionCompleted e)
        {
        }

        public override void Handle(MaterialDiscarded e)
        {
        }

        public override void Handle(MarketSell e)
        {
        }

        public override void Handle(EngineerProgress e)
        {
        }

        /// <summary>
        /// Handle the case where the player dies and all commodities
        /// are removed
        /// </summary>
        /// <param name="e"></param>
        public override void Handle(Died e)
        {
            if(CharacterDied != null)
            {
                CharacterDied(this, new DeathEventArgs(e));
            }
        }

        public override void Handle(Resurrect e)
        {
            if(CharacterResurrected != null)
            {
                CharacterResurrected(this, new ResurrectEventArgs(e));
            }
        }

        public override void Handle(LoadGame e)
        {
            Cmdrs.Add(e.Commander);
        }

        public override void Handle(Unknown e)
        {
        }
    }
}
