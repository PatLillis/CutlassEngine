﻿using System;
using Cutlass;
using Cutlass.Managers;
using PirateyGame.Levels;
using Cutlass.Assets;
using Cutlass.Utilities;
using Microsoft.Xna.Framework.Media;

namespace PirateyGame.Screens
{
    /// <summary>
    /// Main Menu Screen
    /// </summary>
    class MainMenuScreen : ScrollMenuScreen
    {
        #region Fields

        SoundId _BackgroundMusic;

        #endregion Fields

        #region Initialization

        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
            : base("A Game With A Piratey Title")
        {
            // Create our menu entries.
            MenuEntry playGameMenuEntry = new MenuEntry("Play Game");
            MenuEntry optionsMenuEntry = new MenuEntry("Options");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            // Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            _BackgroundMusic = SoundManager.AddSound(new CutlassSong("Content/Sounds/Music/TellerOfTheTales"));
            SoundManager.GetSound(_BackgroundMusic).PlayFadeIn(5000);
        }
        
        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void PlayGameMenuEntrySelected(object sender, EventArgs e)
        {
            SoundManager.GetSound(_BackgroundMusic).Stop();

            LoadingScreen.Load(true, new TestLevel1());
        }

        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void OptionsMenuEntrySelected(object sende, EventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen());
        }
        
        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel()
        {
            const string message = "Are you sure you want to exit this sample?";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message, true, true);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox);
        }
        
        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, EventArgs e)
        {
            CutlassEngine.Game.Exit();
        }

        #endregion
    }
}
