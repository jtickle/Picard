﻿using System;
using System.ComponentModel;
using Picard.Lib;
using LibEDJournal.State;

namespace Picard.FirstRun
{
    class FirstRunController : IDisposable
    {
        protected InaraApi api;
        protected PersistentState state;

        protected MatInitialVerifyForm form;
        protected InventorySet InitialMats;

        public FirstRunController(InaraApi api, PersistentState state)
        {
            this.api = api;
            this.state = state;

            // Set up main form and event handlers
            form = new MatInitialVerifyForm();
            form.ReloadMats += OnReloadMats;
            form.FirstLoadMats += OnFirstLoadMats;
            form.CloseWithoutSaving += OnCloseWithoutSaving;
            form.CloseAndSave += OnCloseAndSave;
            form.Activated += OnReloadMats;

            InitialMats = null;
        }

        protected void UpdateMaterialsList()
        {
            foreach (var mat in InitialMats)
            {
                // Add each material to the big list
                form.AddMaterial(mat.Key, mat.Value);
            }
        }

        protected async void OnFirstLoadMats(object sender, EventArgs e)
        {
            form.SetLoadingState();
            InitialMats = await api.GetMaterialsSheet();
            UpdateMaterialsList();
            form.SetReadyState();
        }

        /// <summary>
        /// Handle user-requested reload
        /// </summary>
        /// <param name="sender">Ignored</param>
        /// <param name="e">Ignored</param>
        protected async void OnReloadMats(object sender, EventArgs e)
        {
            // Get Materials, potentially cached from login
            form.SetLoadingState();
            api.ClearMaterialsCache();
            InitialMats = await api.GetMaterialsSheet();
            UpdateMaterialsList();
            form.SetReadyState();
        }

        /// <summary>
        /// Handle user-requested close without saving data
        /// </summary>
        /// <param name="sender">Ignored</param>
        /// <param name="e">e.Cancel can be used to prevent window close</param>
        protected void OnCloseWithoutSaving(object sender, CancelEventArgs e)
        {
            // Let the user know that no changes will be made
            if (!form.IsUserOKWithNoSync())
            {
                // If they click anything else, cancel the window close
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Handle user-requested close and save
        /// </summary>
        /// <param name="sender">Ignored</param>
        /// <param name="e">e.Cancel can be used to prevent window close</param>
        protected void OnCloseAndSave(object sender, CancelEventArgs e)
        {
            if (InitialMats.IsZero)
            {
                // If the data is all zero, that is a little unusual.
                // Warn the user that it is going to save all zeroes.
                if (!form.IsUserOKWithNoMats())
                {
                    // If they don't click OK, cancel the window close
                    e.Cancel = true;
                    return;
                }
            }

            // Either mats are not empty or the user is OK with it... save!
            state.AddHistory(InitialMats);

            // Update Post Timestamp
            state.UpdateLastPostToCurrent();

            // Persist the State
            state.Persist();
        }

        /// <summary>
        /// For the first time running Picard, go to Inara.cz and get the
        /// player's current materials (which should have been input
        /// manually), and save them to a local state file.
        /// </summary>
        public void Run()
        {
            form.ShowDialog();
        }

        public void Dispose()
        {
            form.Dispose();
        }
    }
}
