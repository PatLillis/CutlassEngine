using System;
using Cutlass.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cutlass.GameComponents
{
    public class FpsCounter : DrawableGameComponent
    {
        private float _UpdateInterval = 0.1f;
        private float _TimeSinceLastUpdate = 0.0f;
        private float _FrameCount = 0;

        /// <summary>The frames per second.</summary>
        public float Fps
        {
            get { return _Fps; }
        }
        private float _Fps = 0;

        /// <summary>
        /// Set up the FPS Counter Game Component
        /// </summary>
        /// <param name="game"></param>
        /// <param name="screenManager"></param>
        public FpsCounter(Game game, ScreenManager screenManager)
            :base (game)
        {
            Enabled = true;
        }

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

            spriteBatch.Begin();
            spriteBatch.DrawString(FontManager.DefaultFont, Fps.ToString(), new Vector2(10, 10), fpsColor);
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

        /// <summary>
        /// FpsCounter Updated Event.
        /// </summary>
        public event EventHandler<EventArgs> Updated;
    }
}
