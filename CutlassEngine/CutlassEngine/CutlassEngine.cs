using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Cutlass.GameComponents;
//using Cutlass.Screens;
using Cutlass.Utilities;
using Cutlass.Managers;

namespace Cutlass
{
    /// <summary>
    /// This is the main engine type.
    /// </summary>
    public class CutlassEngine : Microsoft.Xna.Framework.Game
    {
        #region Fields

        /// <summary>
        /// Width and Height of visible render area.
        /// </summary>
        protected static int _Width, _Height;

        /// <summary>
        /// Width of visible render area.
        /// </summary>
        public static int Width
        {
            get { return _Width; }
        }

        /// <summary>
        /// Height of visible render area.
        /// </summary>
        public static int Height
        {
            get { return _Height; }
        }

        /// <summary>
        /// Aspect ratio of render area.
        /// </summary>
        private static float _AspectRatio = 1.0f;
        /// <summary>
        /// Aspect ratio of render area.
        /// </summary>
        public static float AspectRatio
        {
            get { return _AspectRatio; }
        }

        private static Color _BackgroundColor = Color.LightBlue;
        /// <summary>
        /// Color used to redraw the background scene.
        /// </summary>
        public static Color BackgroundColor
        {
            get { return _BackgroundColor; }
            set { _BackgroundColor = value; }
        }

        public static PlatformID CurrentPlatform = Environment.OSVersion.Platform;

        private static string _WindowTitle = "";
        /// <summary>
        /// Window title for test cases.
        /// </summary>
        public static string WindowTitle
        {
            get { return _WindowTitle; }
        }

        private bool _IsAppActive = false;
        /// <summary>
        /// Is the application active.
        /// </summary>
        public bool IsAppActive
        {
            get { return _IsAppActive; }
            set { _IsAppActive = value; }
        }

        protected static GraphicsDeviceManager _GraphicsDeviceManager = null;
        /// <summary>
        /// The graphics device, used to render.
        /// </summary>
        public static GraphicsDevice Device
        {
            get { return _GraphicsDeviceManager.GraphicsDevice; }
        }

        protected static ContentManager _ContentManager = null;
        /// <summary>
        /// Content Manager
        /// </summary>
        public static ContentManager ContentManager
        {
            get { return _ContentManager; }
        }

        private static bool _checkedGraphicsOptions = false;
        private static bool _applyDeviceChanges = false;

        #endregion

        #region Game Components

        public static Input Input
        {
            get { return _Input; }
        }

        private static FpsCounter _FpsCounter = null;

        private static ScreenManager _ScreenManager = null;

        private static Input _Input = null;

        private static FontManager _FontManager = null;

        private static TextureManager _TextureManager = null;

        #endregion

        #region Initialization

        /// <summary>
        /// Default Construcotr
        /// </summary>
        public CutlassEngine()
            : this("Cutlass Engine") { }

        /// <summary>
        /// The Main PirateyGame constructor
        /// </summary>
        public CutlassEngine(string windowTitle)
        {
            Content.RootDirectory = "CutlassEngineContent";

            _GraphicsDeviceManager = new GraphicsDeviceManager(this);
            _ContentManager = new ContentManager(this.Services);

            _GraphicsDeviceManager.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(GraphicsDeviceManager_PreparingDeviceSettings);
            Window.Title = _WindowTitle = windowTitle;

            GameSettings.Initialize();

            ApplyResolutionChange();

#if DEBUG
            // Disable vertical retrace to get highest framerates possible for
            // testing performance.
            _GraphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
#endif
            // Demand to update as fast as possible, do not use fixed time steps.
            // The whole game is designed this way, if you remove this line
            // the game will not behave normal any longer!
            this.IsFixedTimeStep = false;

            // Init the screen manager component.
            _ScreenManager = new ScreenManager(this);
            Components.Add(_ScreenManager);

            _Input = new Input(this);
            Components.Add(_Input);

            _FontManager = new FontManager(this);
            Components.Add(_FontManager);

            _TextureManager = new TextureManager(this);
            Components.Add(_TextureManager);
#if DEBUG
            // Init the FpsCounter
            _FpsCounter = new FpsCounter(this, _ScreenManager);
            Components.Add(_FpsCounter);
#endif

            //TODO include other inits here!
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

        public static void CheckOptionsAndPSVersion()
        {
            if (Device == null)
            {
                throw new InvalidOperationException("Graphics Device is not created yet!");
            }

            _checkedGraphicsOptions = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            _GraphicsDeviceManager.DeviceReset += new EventHandler<System.EventArgs>(GraphicsDeviceManager_DeviceReset);
            GraphicsDeviceManager_DeviceReset(null, EventArgs.Empty);
        }

        void GraphicsDeviceManager_DeviceReset(object sender, EventArgs e) { }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: use this.Content to unload your game content here
        }

        #endregion

        #region Draw

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(BackgroundColor);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            // The real drawing happens inside the screen manager component.
            base.Draw(gameTime);

            // Apply device changes
            if (_applyDeviceChanges)
            {
                _GraphicsDeviceManager.ApplyChanges();
                ResetElapsedTime();
                _applyDeviceChanges = false;
            }
        }

        #endregion

        #region Public Methods

        public static void ApplyResolutionChange()
        {
            int resolutionWidth = GameSettings.Default.ResolutionWidth;
            int resolutionHeight = GameSettings.Default.ResolutionHeight;

            if (resolutionWidth <= 0 || resolutionWidth <= 0)
            {
                resolutionWidth = GameSettings.MinimumResolutionWidth;
                resolutionHeight = GameSettings.MinimumResolutionWidth;
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
            _GraphicsDeviceManager.IsFullScreen = GameSettings.Default.Fullscreen;

            GameSettings.Save();

            _applyDeviceChanges = true;
#endif
        }

        protected override void OnActivated(object sender, EventArgs args)
        {
            base.OnActivated(sender, args);
            IsAppActive = true;
        }

        protected override void OnDeactivated(object sender, EventArgs args)
        {
            base.OnDeactivated(sender, args);
            IsAppActive = false;
        }

        #endregion
    }
}
