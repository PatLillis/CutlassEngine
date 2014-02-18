using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cutlass.Assets;
using Cutlass.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cutlass.GameComponents
{
    public class FpsCounter : DrawableGameComponent
    {
        private float updateInterval = 0.1f;
        private float timeSinceLastUpdate = 0.0f;
        private float frameCount = 0;

        private float fps = 0;
        /// <summary>
        /// The frames per second.
        /// </summary>
        public float FPS
        {
            get { return fps; }
        }

        public FpsCounter(Game game, ScreenManager screenManager)
            :base (game)
        {
            Enabled = true;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch spriteBatch = new SpriteBatch(CutlassEngine.Device);

            Color fpsColor = Color.Green;
            if (FPS <= 60)
                fpsColor = Color.White;
            if (FPS <= 30)
                fpsColor = Color.Red;

            spriteBatch.Begin();
            spriteBatch.DrawString(FontManager.DefaultFont, FPS.ToString(), new Vector2(10, 10), fpsColor);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            frameCount++;
            timeSinceLastUpdate += elapsed;

            if (timeSinceLastUpdate > updateInterval)
            {
                fps = frameCount / timeSinceLastUpdate; //mean fps over updateIntrval
                frameCount = 0;
                timeSinceLastUpdate -= updateInterval;

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
