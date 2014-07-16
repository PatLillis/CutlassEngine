using System;
using Microsoft.Xna.Framework;
using Cutlass.Utilities;

namespace Cutlass.Managers
{
    public class ResolutionManager
    {
        #region Fields

        public const int VIRTUAL_HEIGHT = 720;

        private GraphicsDeviceManager _GraphicsDeviceManager;

        #endregion Fields

        #region Properties

        public int VirtualWidth { get; set; }

        public int PhysicalHeight { get; set; }
        public int PhysicalWidth { get; set; }

        public Matrix ResolutionScale { get; set; }

        #endregion Properties

        #region Initialization

        public ResolutionManager(GraphicsDeviceManager graphicsDeviceManager)
        {
            _GraphicsDeviceManager = graphicsDeviceManager;
            ApplyResolutionChanges();
        }

        #endregion Initialization

        #region Public Methods

        /// <summary>
        /// Apply any changes made (most likely via the "Options" screen)
        /// </summary>
        public void ApplyResolutionChanges()
        {
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
            _graphicsDeviceManager.IsFullScreen = true;
            _graphicsDeviceManager.PreferredBackBufferWidth =
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphicsDeviceManager.PreferredBackBufferHeight =
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
#else
            _GraphicsDeviceManager.PreferredBackBufferWidth = PhysicalWidth;
            _GraphicsDeviceManager.PreferredBackBufferHeight = PhysicalHeight;
            _GraphicsDeviceManager.IsFullScreen = GameSettingsManager.Default.Fullscreen;

#endif
            //Save new settings out to disk.
            GameSettingsManager.Save();

            //Apply new settings on Graphics Device
            _GraphicsDeviceManager.ApplyChanges();

            //Calculate new aspect ratio
            float aspectRatio = PhysicalWidth / PhysicalHeight;
            VirtualWidth = (int)(VIRTUAL_HEIGHT * aspectRatio);

            //Calculate new Sacling matrix
            float widthScale = (float)PhysicalWidth / VirtualWidth;
            float heightScale = (float)PhysicalHeight / VIRTUAL_HEIGHT;
            Vector3 scalingFactor = new Vector3(widthScale, heightScale, 1);
            ResolutionScale = Matrix.CreateScale(scalingFactor);

            //Update screens
            ScreenManager.ChangeViewSettings(VirtualWidth, ResolutionScale);
        }

        #endregion Public Methods
    }
}
