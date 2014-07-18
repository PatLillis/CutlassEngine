using System;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cutlass.Utilities;

namespace Cutlass.Managers
{
    public static class ResolutionManager
    {
        #region Fields

        public const int VIRTUAL_HEIGHT = 720;

        private static GraphicsDeviceManager _GraphicsDeviceManager;

        private static bool _Initialized = false;

        private static Form _GameForm = null;

        #endregion Fields

        #region Properties

        public static int VirtualWidth { get; set; }

        public static int PhysicalHeight { get; set; }
        public static int PhysicalWidth { get; set; }

        public static Rectangle VirtualFullscreen
        {
            get
            {
                return new Rectangle(0, 0, VirtualWidth, VIRTUAL_HEIGHT);
            }
        }

        public static Rectangle PhysicalFullscreen
        {
            get
            {
                return new Rectangle(0, 0, PhysicalWidth, PhysicalHeight);
            }
        }

        #endregion Properties

        #region Initialization

        public static void Initialize(Game game, GraphicsDeviceManager graphicsDeviceManager)
        {
            //Get the WinForm for the Game
            IntPtr hWnd = game.Window.Handle;
            Control control = System.Windows.Forms.Control.FromHandle(hWnd);
            _GameForm = control.FindForm();

            _GraphicsDeviceManager = graphicsDeviceManager;
            _Initialized = true;

            ApplyResolutionChanges();
        }

        #endregion Initialization

        #region Public Methods

        /// <summary>
        /// Apply any changes made (most likely via the "Options" screen)
        /// </summary>
        public static void ApplyResolutionChanges()
        {
            //If no GraphicsDeviceManager has been set up yet, don't apply changes.
            if (!_Initialized)
                return;

            //Set width/height
            PhysicalWidth = GameSettingsManager.Default.ResolutionWidth;
            PhysicalHeight = GameSettingsManager.Default.ResolutionHeight;

            //Make sure width/height is at least minimum
            if (PhysicalWidth <= 0 || PhysicalHeight <= 0)
            {
                PhysicalWidth = GameSettingsManager.MinimumResolutionWidth;
                PhysicalHeight = GameSettingsManager.MinimumResolutionHeight;
            }

#if XBOX360
            // Xbox 360 graphics settings are fixed
            _GraphicsDeviceManager.IsFullScreen = true;
            _GraphicsDeviceManager.PreferredBackBufferWidth =
                CutlassEngine.Device.Adapter.CurrentDisplayMode.Width;
            _GraphicsDeviceManager.PreferredBackBufferHeight =
                CutlassEngine.Device.Adapter.CurrentDisplayMode.Height;
#else
            _GraphicsDeviceManager.IsFullScreen = GameSettingsManager.Default.IsFullscreen;
            _GraphicsDeviceManager.PreferredBackBufferWidth = PhysicalWidth;
            _GraphicsDeviceManager.PreferredBackBufferHeight = PhysicalHeight;

            if (GameSettingsManager.Default.IsBorderless)
                _GameForm.FormBorderStyle = FormBorderStyle.None;
            else
                _GameForm.FormBorderStyle = FormBorderStyle.FixedSingle;
#endif
            //Calculate new aspect ratio
            float aspectRatio = (float)PhysicalWidth / PhysicalHeight;
            VirtualWidth = (int)(VIRTUAL_HEIGHT * aspectRatio);

            //Update screens
            ScreenManager.ChangeViewSettings(VirtualWidth);

            //Apply new settings on Graphics Device
            _GraphicsDeviceManager.ApplyChanges();

            //Save new settings out to disk.
            GameSettingsManager.Save();
        }

        #endregion Public Methods
    }
}
