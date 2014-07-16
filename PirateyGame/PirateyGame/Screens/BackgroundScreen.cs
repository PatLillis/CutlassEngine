using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cutlass;
using Cutlass.Assets;
using Cutlass.GameComponents;
using Cutlass.Managers;
using Cutlass.Utilities;

namespace PirateyGame.Screens
{
    /// <summary>
    /// Default background screen
    /// </summary>
    class BackgroundScreen : GameScreen
    {
        #region Fields

        TexId _OceanClouds_Id;

        #endregion Fields

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public BackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, wheras if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            _OceanClouds_Id = TextureManager.AddTexture(new CutlassTexture("Content/Textures/oceanClouds"));
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Updates the background screen. Unlike most screens, this should not
        /// transition off even if it has been covered by another screen: it is
        /// supposed to be covered, after all! This overload forces the
        /// coveredByOtherScreen parameter to false in order to stop the base
        /// Update method wanting to transition off.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
        }

        /// <summary>
        /// Draws the background screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = CutlassEngine.SpriteBatch;
            Texture2D texture = TextureManager.GetTexture2D(_OceanClouds_Id);

            spriteBatch.Begin();

            spriteBatch.Draw(texture, ResolutionManager.VirtualFullscreen, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            spriteBatch.End();
        }

        #endregion
    }
}
