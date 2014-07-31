﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cutlass;
using Cutlass.GameComponents;
using Cutlass.Managers;
using Cutlass.Utilities;

namespace PirateyGame.Screens
{
    /// <summary>
    /// The loading screen coordinates transitions between the menu system and the
    /// game itself. Normally one screen will transition off at the same time as
    /// the next screen is transitioning on, but for larger transitions that can
    /// take a longer time to load their data, we want the menu system to be entirely
    /// gone before we start loading the game. This is done as follows:
    /// 
    /// - Tell all the existing screens to transition off.
    /// - Activate a loading screen, which will transition on at the same time.
    /// - The loading screen watches the state of the previous screens.
    /// - When it sees they have finished transitioning off, it activates the real
    ///   next screen, which may take a long time to load its data. The loading
    ///   screen will be the only thing displayed while this load is taking place.
    /// </summary>
    class LoadingScreen : GameScreen
    {
        #region Properties

        /// <summary>Is the loading taking a long time</summary>
        private bool _LoadingIsSlow;

        /// <summary>Keeps track of when all screens have transitioned off, and we can start the load</summary>
        private bool _OtherScreensAreGone;

        /// <summary>Screens that need to be loaded.</summary>
        private GameScreen[] _ScreensToLoad;

        #endregion Properties

        #region Initialization

        /// <summary>
        /// The constructor is private: loading screens should
        /// be activated via the static Load method instead.
        /// </summary>
        private LoadingScreen(bool loadingIsSlow, GameScreen[] screensToLoad)
        {
            this._LoadingIsSlow = loadingIsSlow;
            this._ScreensToLoad = screensToLoad;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
        }

        /// <summary>
        /// Activates the loading screen.
        /// </summary>
        public static void Load(bool loadingIsSlow, params GameScreen[] screensToLoad)
        {
            // Tell all the current screens to transition off.
            GameScreen[] screens = ScreenManager.GetScreens();

            // Create and activate the loading screen.
            LoadingScreen loadingScreen = new LoadingScreen(loadingIsSlow, screensToLoad);
            ScreenManager.AddScreen(loadingScreen);

            foreach (GameScreen screen in screens)
                screen.ExitScreen();
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Updates the loading screen.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // If all the previous screens have finished transitioning
            // off, it is time to actually perform the load.
            if (_OtherScreensAreGone)
            {
                ScreenManager.RemoveScreen(this);

                foreach (GameScreen screen in _ScreensToLoad)
                {
                    if (screen != null)
                    {
                        ScreenManager.AddScreen(screen);
                    }
                }

                // Once the load has finished, we use ResetElapsedTime to tell
                // the  game timing mechanism that we have just finished a very
                // long frame, and that it should not try to catch up.
                CutlassEngine.Game.ResetElapsedTime();
            }
        }

        /// <summary>
        /// Draws the loading screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // If we are the only active screen, that means all the previous screens
            // must have finished transitioning off. We check for this in the Draw
            // method, rather than in Update, because it isn't enough just for the
            // screens to be gone: in order for the transition to look good we must
            // have actually drawn a frame without them before we perform the load.
            if ((ScreenState == ScreenState.Active) &&
                (ScreenManager.GetScreens().Length == 1))
            {
                _OtherScreensAreGone = true;
            }

            // The gameplay screen takes a while to load, so we display a loading
            // message while that is going on, but the menus load very quickly, and
            // it would look silly if we flashed this up for just a fraction of a
            // second while returning from the game to the menus. This parameter
            // tells us how long the loading is going to take, so we know whether
            // to bother drawing the message.
            if (_LoadingIsSlow)
            {
                // Darken down any other screens that were drawn beneath the popup.
                ScreenManager.FadeBackBufferToBlack(TransitionAlpha);

                SpriteBatch spriteBatch = CutlassEngine.SpriteBatch;
                SpriteFont font = FontManager.DefaultFont;

                const string message = "Loading...";

                // Center the text.
                Vector2 screenSize = new Vector2(ResolutionManager.VirtualWidth, ResolutionManager.VIRTUAL_HEIGHT);
                Vector2 textSize = font.MeasureString(message);
                Vector2 textPosition = (screenSize - textSize) / 2;

                Color color = Color.White * TransitionAlpha;

                // Draw the _Text.
                spriteBatch.Begin();
                spriteBatch.DrawString(font, message, textPosition, color);
                spriteBatch.End();
            }
        }

        #endregion
    }
}
