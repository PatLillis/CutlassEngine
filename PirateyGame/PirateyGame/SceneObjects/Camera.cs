using BoundingRect;
using Cutlass.GameComponents;
using Cutlass.Interfaces;
using Cutlass.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PirateyGame.SceneObjects
{
    public class Camera : ICutlassUpdateable
    {
        #region Fields

        const int PLAYER_VERTICAL_BUFFER = 50;
        const int PLAYER_HORIZONTAL_BUFFER = 50;

        #endregion Fields

        #region Properties

        GameScreen ViewScreen
        {
            get { return _ViewScreen; }
            set { _ViewScreen = value; }
        }
        private GameScreen _ViewScreen;

        public Rectangle VisibleArea
        {
            get { return _VisibleArea; }
            set { _VisibleArea = value; }
        }
        private Rectangle _VisibleArea;

        public Vector2 CurrentOffset
        {
            get { return _CurrentOffset; }
            set { _CurrentOffset = value; }
        }
        private Vector2 _CurrentOffset = Vector2.Zero;

        #endregion Properties

        #region Initialization

        public Camera(GameScreen viewScreen, int screenWidth, int screenHeight)
        {
            _ViewScreen = viewScreen;
            _VisibleArea = new Rectangle() { Width = screenWidth, Height = screenHeight };
        }

        #endregion Initialization

        #region Public Methods

        /// <summary>
        /// Make sure camera follow player around.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void UpdateCameraPosition(object sender, BoundingRectangleEventArgs args)
        {
            BoundingRectangle playerPosition = args.Rectangle;
            Vector2 offset = Vector2.Zero;

            if (playerPosition.Top < _VisibleArea.Top + PLAYER_VERTICAL_BUFFER)
                offset.Y = -playerPosition.Top + _VisibleArea.Top + PLAYER_VERTICAL_BUFFER;
            else if (playerPosition.Bottom > _VisibleArea.Bottom - PLAYER_VERTICAL_BUFFER)
                offset.Y = -playerPosition.Bottom + _VisibleArea.Bottom - PLAYER_VERTICAL_BUFFER;

            if (playerPosition.Left < _VisibleArea.Left + PLAYER_HORIZONTAL_BUFFER)
                offset.X = -playerPosition.Left + _VisibleArea.Left + PLAYER_HORIZONTAL_BUFFER;
            else if (playerPosition.Right > _VisibleArea.Right - PLAYER_HORIZONTAL_BUFFER)
                offset.X = -playerPosition.Right + _VisibleArea.Right - PLAYER_HORIZONTAL_BUFFER;

            _ViewScreen.OffsetTransform = Matrix.CreateTranslation(new Vector3(offset, 0.0f));
        }

        /// <summary>
        /// Change screen size
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void UpdateVisibleArea(object sender, RectangleEventArgs args)
        {
            _VisibleArea = args.Rectangle;
        }

        #endregion Public Methods

        /// <summary>
        /// Do any necessary updating to the Camera object
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            //Update
        }
    }
}
