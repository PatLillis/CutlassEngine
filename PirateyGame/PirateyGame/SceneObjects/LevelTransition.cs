using System;
using Microsoft.Xna.Framework;
using Cutlass.Utilities;
using PirateyGame.Screens;
using Cutlass.Interfaces;
using Cutlass.Managers;
using PirateyGame.Levels;

namespace PirateyGame.SceneObjects
{
    public class LevelTransition : Scenery
    {
        #region Properties

        public override float Width
        {
            get { return _Width; }
        }
        private float _Width;

        public override float Height
        {
            get { return _Height; }
        }
        private float _Height;

        public override CollisionCategory CategoryMask
        {
            get { return CollisionCategory.Good; }
        }

        private LevelDirectory _NextLevelId;

        #endregion Properties

        #region Initialization

        public LevelTransition(Vector2 position, float width, float height, LevelDirectory nextLevelId)
#if DEBUG
            : base(position, true)
#else
            : base(position, false)
#endif
        {
            Type screenType = typeof(GameplayScreen);

            _Width = width;
            _Height = height;

            _NextLevelId = nextLevelId;
        }

        #endregion Initialization

        #region Private Methods

        void ConfirmCongratsMessageBoxAccepted(object sender, EventArgs e)
        {
            switch(_NextLevelId)
            {
                case LevelDirectory.TestLevel1:
                    LoadingScreen.Load(true, new TestLevel1());
                    break;
                case LevelDirectory.TestLevel2:
                    LoadingScreen.Load(true, new TestLevel2());
                    break;
                default:
                    break;
            }
        }

        #endregion Private Methods

        #region Public Methods

        public override void CollisionDetected(ICutlassCollidable collisionTarget)
        {
            base.CollisionDetected(collisionTarget);

            MessageBoxScreen congratsMessageBox = new MessageBoxScreen("Congratulations!");
            congratsMessageBox.Accepted += ConfirmCongratsMessageBoxAccepted;

            ScreenManager.AddScreen(congratsMessageBox);
        }

#if DEBUG
        public override void Draw(GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.PointTexture, new Rectangle((int)CurrentFrameBoundingRect.Min.X, (int)CurrentFrameBoundingRect.Min.Y, 1, (int)_Height + 1), Color.Red);
            spriteBatch.Draw(TextureManager.PointTexture, new Rectangle((int)CurrentFrameBoundingRect.Min.X, (int)CurrentFrameBoundingRect.Min.Y, (int)_Width + 1, 1), Color.Red);
            spriteBatch.Draw(TextureManager.PointTexture, new Rectangle((int)CurrentFrameBoundingRect.Min.X + (int)_Width, (int)CurrentFrameBoundingRect.Min.Y, 1, (int)_Height + 1), Color.Red);
            spriteBatch.Draw(TextureManager.PointTexture, new Rectangle((int)CurrentFrameBoundingRect.Min.X, (int)CurrentFrameBoundingRect.Min.Y + (int)_Height, (int)_Width + 1, 1), Color.Red);
        }
#endif
        #endregion Public Methods
    }
}
