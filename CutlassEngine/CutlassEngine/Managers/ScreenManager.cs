using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cutlass.Assets;
using Cutlass.GameComponents;
using Cutlass.Utilities;

namespace Cutlass.Managers
{
    /// <summary>
    /// The screen manager is a component which manages one or more GameScreen
    /// instances. It maintains a stack of screens, calls their Update and Draw
    /// methods at the appropriate times, and automatically routes input to the
    /// topmost active screen.
    /// </summary>
    public class ScreenManager : DrawableGameComponent
    {
        #region Fields

        private static TexId _Blank_Id;

        #endregion Fields

        #region Properties

        /// <summary>
        /// A default SpriteBatch shared by all the screens. This saves
        /// each screen having to bother creating their own local instance.
        /// </summary>
        public static SpriteBatch SpriteBatch
        {
            get { return _SpriteBatch; }
        }
        private static SpriteBatch _SpriteBatch;

        /// <summary>
        /// If true, the manager prints out a list of all the screens
        /// each time it is updated. This can be useful for making sure
        /// everything is being added and removed at the right times.
        /// </summary>
        public bool TraceEnabled
        {
            get { return _TraceEnabled; }
            set { _TraceEnabled = value; }
        }
        private bool _TraceEnabled;

        /// <summary>"Stack" of screens</summary>
        private static List<GameScreen> _Screens = new List<GameScreen>();
        /// <summary>"Stack" of screens that need to be updated.</summary>
        private static List<GameScreen> _ScreensToUpdate = new List<GameScreen>();

        /// <summary>Has the ScreenManager been initialized</summary>
        public static bool IsInitialized
        {
            get { return _IsInitialized; }
        }
        private static bool _IsInitialized;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructs a new screen manager component.
        /// </summary>
        public ScreenManager(CutlassEngine engine)
            : base(engine)
        { }

        /// <summary>
        /// Initializes the screen manager component.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            _IsInitialized = true;
        }

        /// <summary>
        /// Load graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            _SpriteBatch = new SpriteBatch(GraphicsDevice);

            _Blank_Id = TextureManager.AddTexture(new CutlassTexture("Content/Textures/blank"));

            // Tell each of the screens to load their content.
            foreach (GameScreen screen in _Screens)
            {
                screen.LoadContent();
            }
        }


        /// <summary>
        /// Unload graphics content.
        /// </summary>
        protected override void UnloadContent()
        {
            // Tell each of the screens to unload their content.
            foreach (GameScreen screen in _Screens)
            {
                screen.UnloadContent();
            }
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Allows each screen to run logic.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            // Read the keyboard and gamepad.
            CutlassEngine.Input.Update();
 
            // Make a copy of the master screen list, to avoid confusion if
            // the process of updating one screen adds or removes others.
            _ScreensToUpdate.Clear();

            foreach (GameScreen screen in _Screens)
                _ScreensToUpdate.Add(screen);

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            // Loop as long as there are screens waiting to be updated.
            while (_ScreensToUpdate.Count > 0)
            {
                // Pop the topmost screen off the waiting list.
                GameScreen screen = _ScreensToUpdate[_ScreensToUpdate.Count - 1];

                _ScreensToUpdate.RemoveAt(_ScreensToUpdate.Count - 1);

                // Update the screen.
                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.TransitionOn ||
                    screen.ScreenState == ScreenState.Active)
                {
                    // If this is the first active screen we came across,
                    // give it a chance to handle input.
                    if (!otherScreenHasFocus)
                    {
                        screen.HandleInput(CutlassEngine.Input);

                        otherScreenHasFocus = true;
                    }

                    // If this is an active non-popup, inform any subsequent
                    // screens that they are covered by it.
                    if (!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }

            // Print debug trace?
            if (_TraceEnabled)
                TraceScreens();
        }

        /// <summary>
        /// Prints a list of all the screens, for debugging.
        /// </summary>
        private void TraceScreens()
        {
            List<string> screenNames = new List<string>();

            foreach (GameScreen screen in _Screens)
                screenNames.Add(screen.GetType().Name);

            Debug.WriteLine(string.Join(", ", screenNames.ToArray()));
        }

        /// <summary>
        /// Tells each screen to draw itself.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            //Normal draws
            foreach (GameScreen screen in _Screens)
            {
                if (screen.ScreenState == ScreenState.Hidden)
                    continue;

                screen.Draw(gameTime);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a new screen to the screen manager.
        /// </summary>
        public static void AddScreen(GameScreen screen)
        {
            _Screens.Add(screen);

            // If we have a graphics device, tell the screen to load content.
            if (_IsInitialized)
            {
                screen.LoadContent();
            }
        }

        /// <summary>
        /// Removes a screen from the screen manager. You should normally
        /// use GameScreen.ExitScreen instead of calling this directly, so
        /// the screen can gradually transition off rather than just being
        /// instantly removed.
        /// </summary>
        public static void RemoveScreen(GameScreen screen)
        {
            // If we have a graphics device, tell the screen to unload content.
            if (_IsInitialized)
            {
                screen.UnloadContent();
            }

            _Screens.Remove(screen);
            _ScreensToUpdate.Remove(screen);
        }

        /// <summary>
        /// Expose an array holding all the screens. We return a copy rather
        /// than the real master list, because screens should only ever be added
        /// or removed using the AddScreen and RemoveScreen methods.
        /// </summary>
        public static GameScreen[] GetScreens()
        {
            return _Screens.ToArray();
        }

        /// <summary>
        /// Helper draws a translucent black fullscreen sprite, used for fading
        /// screens in and out, and for darkening the background behind popups.
        /// </summary>
        public static void FadeBackBufferToBlack(float alpha)
        {
            Viewport viewport = CutlassEngine.Device.Viewport;

            _SpriteBatch.Begin();

            _SpriteBatch.Draw(TextureManager.GetTexture2D(_Blank_Id),
                             new Rectangle(0, 0, viewport.Width, viewport.Height),
                             Color.Black * alpha);

            _SpriteBatch.End();
        }

        public static void ChangeViewSettings(int newVirtualWidth, Matrix newScaleMatrix)
        {
            foreach (GameScreen screen in _Screens)
            {
                screen.ChangeViewSettings(newVirtualWidth, newScaleMatrix);
            }
        }

        #endregion
    }
}
