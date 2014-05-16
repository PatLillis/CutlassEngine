﻿using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Cutlass;
using Cutlass.GameComponents;
using Cutlass.Managers;
using Cutlass.Utilities;
using PirateyGame.SceneObjects;
using Cutlass.Assets;
using System.Collections.Generic;

namespace PirateyGame.Screens
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

        float pauseAlpha;

        #endregion

        #region Properties

        public Player Player
        {
            get { return _Player; }
        }
        private Player _Player;

        public Camera Camera
        {
            get { return _Camera; }
        }
        private Camera _Camera;

        public List<Scenery> Scenery
        {
            get
            {
                if (_Scenery == null)
                    _Scenery = new List<Scenery>();
                return _Scenery;
            }
        }
        private List<Scenery> _Scenery;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
            : base()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            _Player = new Player(new CutlassAnimatedTexture("Content/Textures/Sprites/playerTest", 3), "Wuuuuut");
            _Camera = new Camera(this, GameSettingsManager.Default.ResolutionWidth, GameSettingsManager.Default.ResolutionHeight);

            _Player.PlayerMoved += _Camera.UpdateCameraPosition;

            Scenery.Add(new Scenery(new Vector2(300, 300), new CutlassAnimatedTexture("Content/Textures/Sprites/playerTest", 3)));

            ObjectManager.AddObjects(Player, Camera);
            ObjectManager.AddObjects(Scenery.ToArray());

            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            CutlassEngine.Game.ResetElapsedTime();
        }

        #endregion

        #region Public Methods

        #endregion

        #region Update and Draw

        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
        }

        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(Input input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            GamePadState gamePadState = input.CurrentGamePadState;

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected;

            if (input.PauseGame || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen());
            }
            else
            {
                Player.HandleInput(input);
            }
        }

         ///<summary>
         ///Draws the gameplay screen.
         ///</summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            CutlassEngine.Device.Clear(ClearOptions.Target, Palette.OffWhite, 0, 0);

            base.Draw(gameTime);

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        #endregion
    }
}
