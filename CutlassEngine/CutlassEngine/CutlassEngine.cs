using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Cutlass.GameComponents;
using Cutlass.Managers;
using Cutlass.Utilities;

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

        public static SpriteBatch SpriteBatch
        {
            get {return _SpriteBatch;}
            set { _SpriteBatch = value; }
        }
        private static SpriteBatch _SpriteBatch = null;

        private RenderTarget2D _VirtualRenderTarget = null;
        private Texture2D _VirtualTexture2D = null;

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

            //Initialize Resolution settings.
            ResolutionManager.Initialize(_GraphicsDeviceManager);

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
            _VirtualRenderTarget = new RenderTarget2D(GraphicsDevice, ResolutionManager.VirtualWidth, ResolutionManager.VIRTUAL_HEIGHT);
            _SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();

            //Unload engine-specific content
            _VirtualRenderTarget.Dispose();
            _VirtualRenderTarget = null;
            _VirtualTexture2D = null;

            _SpriteBatch.Dispose();
            _SpriteBatch = null;
        }

        #endregion

        #region Draw

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_VirtualRenderTarget);
            GraphicsDevice.Clear(BackgroundColor);

            // The real drawing happens inside the screen manager component.
            base.Draw(gameTime);

            // Reset the device to the back buffer
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(BackgroundColor);

            _VirtualTexture2D = (Texture2D)_VirtualRenderTarget;

            _SpriteBatch.Begin();
            _SpriteBatch.Draw(_VirtualTexture2D, new Rectangle(0, 0, ResolutionManager.PhysicalWidth, ResolutionManager.PhysicalHeight), Color.White);
            _SpriteBatch.End();

            // Apply device changes
            if (GameSettingsManager.ResolutionChangesToApply)
            {
                ResolutionManager.ApplyResolutionChanges();
                GameSettingsManager.ResolutionChangesToApply = false;
                _VirtualRenderTarget = new RenderTarget2D(GraphicsDevice, ResolutionManager.VirtualWidth, ResolutionManager.VIRTUAL_HEIGHT);

                ResetElapsedTime();
            }
        }

        #endregion

        #region Public Methods

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
