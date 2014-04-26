using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cutlass.Assets;
using Cutlass.Managers;
using Cutlass.Utilities;

namespace Cutlass.GameComponents
{
    /// <summary>
    /// FPS Counter component
    /// </summary>
    public class FpsCounter : DrawableGameComponent
    {
        #region Fields

        private TexId _Blank_Id;

        #endregion Fields

        #region Properties

        /// <summary>How often to update frame count.</summary>
        private float _UpdateInterval = 0.1f;
        /// <summary>Seconds since last update</summary>
        private float _TimeSinceLastUpdate = 0.0f;
        /// <summary>Number of frames in the current measurement</summary>
        private float _FrameCount = 0;

        /// <summary>The frames per second.</summary>
        public float Fps
        {
            get { return _Fps; }
        }
        private float _Fps = 0;

        /// <summary>FpsCounter Updated Event</summary>
        public event EventHandler<EventArgs> Updated;

        #endregion Properties

        #region Initialization

        /// <summary>
        /// Set up the FPS Counter Game Component
        /// </summary>
        /// <param name="game"></param>
        /// <param name="screenManager"></param>
        public FpsCounter(Game game)
            :base (game)
        {
            Enabled = true;
        }

        /// <summary>
        /// Load Content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            _Blank_Id = TextureManager.AddTexture(new CutlassTexture("Content/Textures/blank"));
        }

        #endregion Initialization

        #region Update and Draw

        /// <summary>
        /// Draw FPS string on screen
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            //Change color, depending on how dire the situation is.
            Color fpsColor = Color.Green;
            if (Fps <= 60)
                fpsColor = Color.White;
            if (Fps <= 30)
                fpsColor = Color.Red;

            Vector2 backroundSize = FontManager.DefaultFont.MeasureString(Fps.ToString("0.00"));
            Rectangle backgroundRect = new Rectangle(10, 10, (int)backroundSize.X + 20, (int)backroundSize.Y + 20);

            spriteBatch.Begin();

            //Draw background
            spriteBatch.Draw(TextureManager.GetTexture2D(_Blank_Id), backgroundRect, Color.Black * 0.5f);
            spriteBatch.Draw(TextureManager.PointTexture, new Rectangle(backgroundRect.X, backgroundRect.Y, 1, backgroundRect.Height + 1), fpsColor);
            spriteBatch.Draw(TextureManager.PointTexture, new Rectangle(backgroundRect.X, backgroundRect.Y, backgroundRect.Width + 1, 1), fpsColor);
            spriteBatch.Draw(TextureManager.PointTexture, new Rectangle(backgroundRect.X + backgroundRect.Width, backgroundRect.Y, 1, backgroundRect.Height + 1), fpsColor);
            spriteBatch.Draw(TextureManager.PointTexture, new Rectangle(backgroundRect.X, backgroundRect.Y + backgroundRect.Height, backgroundRect.Width + 1, 1), fpsColor);

            //Draw text
            spriteBatch.DrawString(FontManager.DefaultFont, Fps.ToString("0.00"), new Vector2(20, 20), fpsColor);

            spriteBatch.End();
        }

        /// <summary>
        /// Update the FPS readings.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _FrameCount++;
            _TimeSinceLastUpdate += elapsed;

            if (_TimeSinceLastUpdate > _UpdateInterval)
            {
                _Fps = _FrameCount / _TimeSinceLastUpdate; //mean _Fps over updateIntrval
                _FrameCount = 0;
                _TimeSinceLastUpdate -= _UpdateInterval;

                if (Updated != null)
                    Updated(this, new EventArgs());
            }
        }

        #endregion Update and Draw
    }
}