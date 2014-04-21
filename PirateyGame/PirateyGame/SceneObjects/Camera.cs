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
        const int PLAYER_HORIZONTAL_BUFFER = 200;

        #endregion Fields

        #region Properties

        GameScreen ViewScreen
        {
            get { return _ViewScreen; }
            set { _ViewScreen = value; }
        }
        private GameScreen _ViewScreen;

        public BoundingRectangle VisibleArea
        {
            get { return _VisibleArea; }
            set { _VisibleArea = value; }
        }
        private BoundingRectangle _VisibleArea;

        #endregion Properties

        #region Initialization

        public Camera(GameScreen viewScreen, float screenWidth, float screenHeight, float startingX = 0.0f, float startingY = 0.0f)
        {
            _ViewScreen = viewScreen;
            _VisibleArea = new BoundingRectangle(startingX, startingY, screenWidth, screenHeight);
        }

        #endregion Initialization

        #region Public Methods

        /// <summary>
        /// Make sure camera follows player around.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void UpdateCameraPosition(object sender, BoundingRectangleEventArgs args)
        {
            BoundingRectangle playerPosition = args.Rectangle;
            Vector2 currentOffset = _VisibleArea.Min;
            Vector2 newOffset = Vector2.Zero;

            //Calculate offset
            newOffset.X = Math.Max(0, PLAYER_HORIZONTAL_BUFFER - (playerPosition.Left - _VisibleArea.Left));
            newOffset.X = Math.Min(newOffset.X, -PLAYER_HORIZONTAL_BUFFER - (playerPosition.Right - _VisibleArea.Right));
            newOffset.Y = Math.Max(0, PLAYER_VERTICAL_BUFFER - (playerPosition.Top - _VisibleArea.Top));
            newOffset.Y = Math.Min(newOffset.Y, -PLAYER_VERTICAL_BUFFER - (playerPosition.Bottom - _VisibleArea.Bottom));

            //Apply offset
            _VisibleArea.Translate(-newOffset);
            _ViewScreen.OffsetTransform = Matrix.CreateTranslation(new Vector3(newOffset - currentOffset, 0.0f));
        }

        /// <summary>
        /// Change screen size
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void UpdateVisibleArea(object sender, RectangleEventArgs args)
        {
            _VisibleArea = new BoundingRectangle(args.Rectangle);
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
