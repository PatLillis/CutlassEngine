using System;
using Cutlass.Managers;

namespace PirateyGame.Screens
{
    /// <summary>
    /// Pause Screen
    /// </summary>
    class PauseMenuScreen : ScrollMenuScreen
    {
        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public PauseMenuScreen()
            : base("Paused")
        {
            // Create our menu entries.
            MenuEntry resumeGameMenuEntry = new MenuEntry("Resume Game");
            MenuEntry optionsGameMenuEntry = new MenuEntry("Options");
            MenuEntry quitGameMenuEntry = new MenuEntry("Quit Game");

            // Hook up menu event handlers.
            resumeGameMenuEntry.Selected += OnCancel;
            optionsGameMenuEntry.Selected += OptionsGameMenuEntrySelected;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            // Add entries to the menu.
            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(optionsGameMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the Quit Game menu entry is selected.
        /// </summary>
        void QuitGameMenuEntrySelected(object sender, EventArgs e)
        {
            const string message = "Are you sure you want to quit this game?";

            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message, true, true);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox);
        }

        void OptionsGameMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen());
        }

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to quit" message box. This uses the loading screen to
        /// transition from the game back to the main menu screen.
        /// </summary>
        void ConfirmQuitMessageBoxAccepted(object sender, EventArgs e)
        {
            LoadingScreen.Load(false, null, new BackgroundScreen(), new MainMenuScreen());
        }

        #endregion
    }
}
