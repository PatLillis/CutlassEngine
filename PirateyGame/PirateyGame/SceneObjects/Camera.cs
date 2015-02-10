using System;
using Microsoft.Xna.Framework;
using BoundingRect;
using Cutlass.GameComponents;
using Cutlass.Interfaces;
using Cutlass.Utilities;

namespace PirateyGame.SceneObjects
{
    public class Camera : ICutlassUpdateable
    {
        #region Fields

        const int VERTICAL_EDGE_BUFFER = 100;
        const int HORIZONTAL_EDGE_BUFFER = 200;

        #endregion Fields

        #region Properties

        public SceneObjectId SceneObjectId { get; set; }

        public GameScreen ViewScreen
        {
            get { return _ViewScreen; }
            set { _ViewScreen = value; }
        }
        private GameScreen _ViewScreen;

        public bool Active
        {
            get { return _Active; }
            set { _Active = value; }
        }
        private bool _Active;

        #endregion Properties

        #region Initialization

        public Camera(GameScreen viewScreen, float screenWidth, float screenHeight, Vector2 startingCenter)
        {
            _Active = true;
            _ViewScreen = viewScreen;
            Vector2 startingTopLeft = startingCenter - new Vector2(screenWidth / 2, screenHeight / 2);
            _ViewScreen.VisibleArea = new BoundingRectangle(startingTopLeft.X, startingTopLeft.Y, screenWidth, screenHeight);
        }

        #endregion Initialization

        #region Public Methods

        /// <summary>
        /// Update camera position so that the passed in Rectangle is on screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void UpdateCameraPosition(object sender, BoundingRectangleEventArgs args)
        {
            BoundingRectangle rectangle = args.Rectangle;
            Vector2 currentOffset = _ViewScreen.VisibleArea.Min;
            Vector2 newOffset = Vector2.Zero;

            //Calculate offset
            newOffset.X = Math.Max(0, HORIZONTAL_EDGE_BUFFER - (rectangle.Left - _ViewScreen.VisibleArea.Left));
            newOffset.X = Math.Min(newOffset.X, -HORIZONTAL_EDGE_BUFFER - (rectangle.Right - _ViewScreen.VisibleArea.Right));
            newOffset.Y = Math.Max(0, VERTICAL_EDGE_BUFFER - (rectangle.Top - _ViewScreen.VisibleArea.Top));
            newOffset.Y = Math.Min(newOffset.Y, -VERTICAL_EDGE_BUFFER - (rectangle.Bottom - _ViewScreen.VisibleArea.Bottom));

            //Apply offset
            _ViewScreen.VisibleArea = BoundingRectangle.Translate(_ViewScreen.VisibleArea, -newOffset);
            _ViewScreen.OffsetTransform = Matrix.CreateTranslation(new Vector3(newOffset - currentOffset, 0.0f));
        }

        #endregion Public Methods

        #region Update

        /// <summary>
        /// Do any necessary updating to the Camera object
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            //Update
        }

        #endregion Update
    }
}
