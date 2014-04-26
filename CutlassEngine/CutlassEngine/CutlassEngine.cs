using System;
using Cutlass.GameComponents;
using Cutlass.Managers;
using Cutlass.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cutlass
{
    /// <summary>
    /// This is the main engine type.
    /// </summary>
    public class CutlassEngine : Microsoft.Xna.Framework.Game
    {
        #region Properties

        /// <summary>XNA Game, used to call non-static methods</summary>
        public static Game Game
        {
            get { return _Game; }
            set { _Game = value; }
        }
        private static Game _Game;

        /// <summary>Aspect ratio of render area.</summary>
        public static float AspectRatio
        {
            get { return _AspectRatio; }
        }
        private static float _AspectRatio = 1.0f;

        /// <summary>Color used to redraw the background scene.</summary>
        public static Color BackgroundColor
        {
            get { return _BackgroundColor; }
            set { _BackgroundColor = value; }
        }
        private static Color _BackgroundColor = Color.LightBlue;

        /// <summary>The platform the engine is currently running on</summary>
        public static PlatformID CurrentPlatform = Environment.OSVersion.Platform;

        /// <summary>Window title</summary>
        public static string WindowTitle
        {
            get { return _WindowTitle; }
        }
        private static string _WindowTitle = "";

        /// <summary>Is the application active.</summary>
        public bool IsAppActive
        {
            get { return _IsAppActive; }
            set { _IsAppActive = value; }
        }
        private bool _IsAppActive = false;

        /// <summary>The graphics device, used to render.</summary>
        public static GraphicsDevice Device
        {
            get { return _GraphicsDeviceManager.GraphicsDevice; }
        }
        private static GraphicsDeviceManager _GraphicsDeviceManager = null;

        /// <summary>Content Manager</summary>
        public static ContentManager ContentManager
        {
            get { return _ContentManager; }
        }
        private static ContentManager _ContentManager = null;

        /// <summary>Whether the Graphics Options have been checked yet</summary>
        private static bool _CheckedGraphicsOptions = false;

        /// <summary>Whether device changes need to be applied</summary>
        private static bool _ApplyDeviceChanges = false;

        #endregion

        #region Game Components

        /// <summary>Input component</summary>
        public static Input Input { get { return _Input; } }
        private static Input _Input = null;

        /// <summary>FPS Counter component, only displayed in #DEBUG</summary>
        private static FpsCounter _FpsCounter = null;

        /// <summary>Screen Manager component</summary>
        private static ScreenManager _ScreenManager = null;

        /// <summary>Font Manager component</summary>
        private static FontManager _FontManager = null;

        /// <summary>Texture Manager component</summary>
        private static TextureManager _TextureManager = null;

        #endregion

        #region Initialization

        /// <summary>
        /// The Main CutlassEngine constructor
        /// </summary>
        public CutlassEngine(string windowTitle = "Cutlass Engine")
        {
            _GraphicsDeviceManager = new GraphicsDeviceManager(this);
            _ContentManager = new ContentManager(this.Services);

            _GraphicsDeviceManager.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(GraphicsDeviceManager_PreparingDeviceSettings);
            Window.Title = _WindowTitle = windowTitle;

            GameSettingsManager.Initialize();

            ApplyResolutionChange();

#if DEBUG
            //Disable vertical retrace to get highest framerates possible for
            //testing performance.
            _GraphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
#endif
            //Demand to update as fast as possible, do not use fixed time steps.
            this.IsFixedTimeStep = false;

            //Init the screen manager component.
            _ScreenManager = new ScreenManager(this);
            Components.Add(_ScreenManager);

            //Init the Input component
            _Input = new Input(this);
            Components.Add(_Input);

            //Init the Font Manager component
            _FontManager = new FontManager(this);
            Components.Add(_FontManager);

            //Init the Texture Manager component
            _TextureManager = new TextureManager(this);
            Components.Add(_TextureManager);
#if DEBUG
            //Init the FpsCounter
            _FpsCounter = new FpsCounter(this);
            Components.Add(_FpsCounter);
#endif
        }

        /// <summary>
        /// Prepare the graphics device.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event args</param>
        void GraphicsDeviceManager_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                PresentationParameters presentParams =
                    e.GraphicsDeviceInformation.PresentationParameters;

                _GraphicsDeviceManager.PreferMultiSampling = true;

                if (_GraphicsDeviceManager.PreferredBackBufferHeight == 720)
                {
#if !DEBUG
                    presentParams.PresentationInterval = PresentInterval.One;
#endif
                }
                else
                {
#if !DEBUG
                    presentParams.PresentationInterval = PresentInterval.Two;
#endif
                }
            }
        }

        /// <summary>
        /// Check Graphics Options
        /// </summary>
        public static void CheckOptionsAndPSVersion()
        {
            if (Device == null)
            {
                throw new InvalidOperationException("Graphics Device is not created yet!");
            }

            _CheckedGraphicsOptions = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            _GraphicsDeviceManager.DeviceReset += new EventHandler<System.EventArgs>(GraphicsDeviceManager_DeviceReset);
            GraphicsDeviceManager_DeviceReset(null, EventArgs.Empty);
        }

        /// <summary>
        /// Used to handle a device reset
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GraphicsDeviceManager_DeviceReset(object sender, EventArgs e)
        { }

        /// <summary>
        /// LoadContent will be called once per game
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            //Load engine-specific content
        }

        /// <summary>
        /// UnloadContent will be called once per game
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();

            //Unload engine-specific content
        }

        #endregion

        #region Draw

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(BackgroundColor);

            // The real drawing happens inside the screen manager component.
            base.Draw(gameTime);

            // Apply device changes
            if (_ApplyDeviceChanges)
            {
                _GraphicsDeviceManager.ApplyChanges();
                ScreenManager.ChangeViewSettings(GameSettingsManager.Default.ResolutionWidth, GameSettingsManager.Default.ResolutionHeight);
                ResetElapsedTime();
                _ApplyDeviceChanges = false;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Apply any changes made (most likely via the "Options" screen)
        /// </summary>
        public static void ApplyResolutionChange()
        {
            //Set width/height
            int resolutionWidth = GameSettingsManager.Default.ResolutionWidth;
            int resolutionHeight = GameSettingsManager.Default.ResolutionHeight;

            //Make sure width/height is at least minimum
            if (resolutionWidth <= 0 || resolutionWidth <= 0)
            {
                resolutionWidth = GameSettingsManager.MinimumResolutionWidth;
                resolutionHeight = GameSettingsManager.MinimumResolutionHeight;
            }
#if XBOX360
            // Xbox 360 graphics settings are fixed
            _graphicsDeviceManager.IsFullScreen = true;
            _graphicsDeviceManager.PreferredBackBufferWidth =
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphicsDeviceManager.PreferredBackBufferHeight =
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
#else
            _GraphicsDeviceManager.PreferredBackBufferWidth = resolutionWidth;
            _GraphicsDeviceManager.PreferredBackBufferHeight = resolutionHeight;
            _GraphicsDeviceManager.IsFullScreen = GameSettingsManager.Default.Fullscreen;

            //Save new settings out to disk.
            GameSettingsManager.Save();

            _ApplyDeviceChanges = true;
#endif
        }

        /// <summary>
        /// Handle when this app is activated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected override void OnActivated(object sender, EventArgs args)
        {
            base.OnActivated(sender, args);
            IsAppActive = true;
        }

        /// <summary>
        /// Handle when this app is deactivated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected override void OnDeactivated(object sender, EventArgs args)
        {
            base.OnDeactivated(sender, args);
            IsAppActive = false;
        }

        #endregion
    }
}
