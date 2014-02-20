using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cutlass;
using Cutlass.Assets;
using Cutlass.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PirateyGame.Screens
{
    class ScrollMenuScreen : MenuScreen
    {
        public ScrollMenuScreen(string menuTitle)
            : base(menuTitle)
        {
            _TitleColor = Palette.MediumBrown;
            _TitleFontKey = "bilbo";
        }

        public override void LoadContent()
        {
            base.LoadContent();

            TextureManager.AddTexture(new CutlassTexture("Content/Textures/titleScrollMiddle"), "titleScrollMiddle");
            TextureManager.AddTexture(new CutlassTexture("Content/Textures/titleScrollEdge"), "titleScrollEdge");
            TextureManager.AddTexture(new CutlassTexture("Content/Textures/backgroundMenuCorner"), "menuBackgroundCorner");
            TextureManager.AddTexture(new CutlassTexture("Content/Textures/backgroundMenuVerticalEdge"), "menuBackgroundVerticalEdge");
            TextureManager.AddTexture(new CutlassTexture("Content/Textures/backgroundMenuHorizontalEdge"), "menuBackgroundHorizontalEdge");

            FontManager.AddFont(new CutlassFont("Content/Fonts/bilboSwashCaps"), "bilbo");
            FontManager.AddFont(new CutlassFont("Content/Fonts/frederickaTheGreat"), "fredericka");

            SetMenuEntryFont("fredericka");
            SetMenuEntryTextColor(Palette.CharcoalGrey);
            SetMenuEntrySelectedTextColor(Palette.LightBlue);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice graphics = CutlassEngine.Device;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont titleFont = FontManager.GetSpriteFontOrDefault(_TitleFontKey);

            #region Draw Title Scroll

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Draw the menu title centered on the screen
            Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, 80);
            Vector2 titleSize = titleFont.MeasureString(_MenuTitle);
            Color titleColor = TitleColor * TransitionAlpha;
            float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            Texture2D titleScrollMiddle = TextureManager.GetTexture2D("titleScrollMiddle");
            Texture2D titleScrollEdge = TextureManager.GetTexture2D("titleScrollEdge");

            //Draw Scroll behind menu title (properly sized)
            //80 = 40px drawable space on each scroll end image * 2
            int middleScrollCount = ((int)(titleSize.X * titleScale) - 80) / titleScrollMiddle.Width;
            //If there is leftover, add one.
            if (((int)(titleSize.X * 1.25f) - 80) % titleScrollMiddle.Width > 0)
            {
                middleScrollCount += 1;
            }
            int totalMiddleScrollWidth = middleScrollCount * titleScrollMiddle.Width;

            Vector2 leftScrollPosition = new Vector2((graphics.Viewport.Width / 2) - (totalMiddleScrollWidth / 2),
                titlePosition.Y);
            Vector2 leftScrollOrigin = new Vector2(titleScrollEdge.Width, titleScrollEdge.Height / 2);

            Vector2 rightScrollPosition = new Vector2((graphics.Viewport.Width / 2) + (totalMiddleScrollWidth / 2),
                titlePosition.Y);
            Vector2 rightScrollOrigin = new Vector2(0, titleScrollEdge.Height / 2);

            Vector2 middleScrollPosition = new Vector2((graphics.Viewport.Width / 2) - (totalMiddleScrollWidth / 2) + (titleScrollMiddle.Width / 2), titlePosition.Y);
            Vector2 middleScrollOrigin = new Vector2(titleScrollEdge.Width / 2, titleScrollEdge.Height / 2);

            spriteBatch.Begin();

            //Draw Left Scroll Handle
            spriteBatch.Draw(titleScrollEdge, leftScrollPosition, null, Color.White, 0.0f, leftScrollOrigin, 1.0f, SpriteEffects.FlipHorizontally, 1.0f);
            //Draw Right Scroll Handle
            spriteBatch.Draw(titleScrollEdge, rightScrollPosition, null, Color.White, 0.0f, rightScrollOrigin, 1.0f, SpriteEffects.None, 1.0f);
            //Draw Middle of Scroll
            for (int i = 0; i < middleScrollCount; i++)
            {
                spriteBatch.Draw(titleScrollMiddle, middleScrollPosition, null, Color.White, 0.0f, middleScrollOrigin, 1.0f, SpriteEffects.None, 1.0f);
                middleScrollPosition.X += titleScrollMiddle.Width;
            }

            spriteBatch.End();

            #endregion

            #region Draw Menu Entry Background

            Rectangle menuOptionsBackground = new Rectangle();

            //The 1.1 is for the "pulsating effect of the menu options.
            menuOptionsBackground.Width = (int)(MaxEntryWidth() * 1.1) - 64;
            menuOptionsBackground.Height = -64;
            for (int i = 0; i < _MenuEntries.Count; i++)
                menuOptionsBackground.Height += _MenuEntries[i].GetHeight(this);
            menuOptionsBackground.X = (int)_MenuEntries[0].Position.X - menuOptionsBackground.Width / 2;
            menuOptionsBackground.Y = (int)_MenuEntries[0].Position.Y - FontManager.GetSpriteFontOrDefault(_MenuEntries[0].EntryFontKey).LineSpacing / 2 + 32;

            spriteBatch.Begin();

            //UR corner
            spriteBatch.Draw(TextureManager.GetTexture2D("menuBackgroundCorner"),
                             new Vector2(menuOptionsBackground.Right, menuOptionsBackground.Top - 64),
                             null,
                             Color.White,
                             0.0f,
                             Vector2.Zero,
                             1.0f,
                             SpriteEffects.None,
                             1.0f);
            //BR corner
            spriteBatch.Draw(TextureManager.GetTexture2D("menuBackgroundCorner"),
                             new Vector2(menuOptionsBackground.Right, menuOptionsBackground.Bottom),
                             null,
                             Color.White,
                             0.0f,
                             Vector2.Zero,
                             1.0f,
                             SpriteEffects.FlipVertically,
                             1.0f);
            //BL corner
            spriteBatch.Draw(TextureManager.GetTexture2D("menuBackgroundCorner"),
                             new Vector2(menuOptionsBackground.Left - 64, menuOptionsBackground.Bottom),
                             null,
                             Color.White,
                             0.0f,
                             Vector2.Zero,
                             1.0f,
                             SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally,
                             1.0f);
            //UL corner
            spriteBatch.Draw(TextureManager.GetTexture2D("menuBackgroundCorner"),
                             new Vector2(menuOptionsBackground.Left - 64, menuOptionsBackground.Top - 64),
                             null,
                             Color.White,
                             0.0f,
                             Vector2.Zero,
                             1.0f,
                             SpriteEffects.FlipHorizontally,
                             1.0f);
            //Top edge
            spriteBatch.Draw(TextureManager.GetTexture2D("menuBackgroundVerticalEdge"),
                             new Vector2(menuOptionsBackground.Left, menuOptionsBackground.Top - 64),
                             null,
                             Color.White,
                             0.0f,
                             Vector2.Zero,
                             new Vector2(menuOptionsBackground.Width / 64.0f, 1.0f),
                             SpriteEffects.None,
                             1.0f);
            //Right edge
            spriteBatch.Draw(TextureManager.GetTexture2D("menuBackgroundHorizontalEdge"),
                             new Vector2(menuOptionsBackground.Right, menuOptionsBackground.Top),
                             null,
                             Color.White,
                             0.0f,
                             Vector2.Zero,
                             new Vector2(1.0f, menuOptionsBackground.Height / 64.0f),
                             SpriteEffects.None,
                             1.0f);
            //Bottom edge
            spriteBatch.Draw(TextureManager.GetTexture2D("menuBackgroundVerticalEdge"),
                             new Vector2(menuOptionsBackground.Left, menuOptionsBackground.Bottom),
                             null,
                             Color.White,
                             0.0f,
                             Vector2.Zero,
                             new Vector2(menuOptionsBackground.Width / 64.0f, 1.0f),
                             SpriteEffects.FlipVertically,
                             1.0f);
            //Left edge
            spriteBatch.Draw(TextureManager.GetTexture2D("menuBackgroundHorizontalEdge"),
                             new Vector2(menuOptionsBackground.Left - 64, menuOptionsBackground.Top),
                             null,
                             Color.White,
                             0.0f,
                             Vector2.Zero,
                             new Vector2(1.0f, menuOptionsBackground.Height / 64.0f),
                             SpriteEffects.FlipHorizontally,
                             1.0f);
            //Fill
            spriteBatch.Draw(TextureManager.GetTexture2D("menuBackgroundHorizontalEdge"),
                menuOptionsBackground,
                new Rectangle(0, 0, 1, 1),
                Color.White);

            spriteBatch.End();

            #endregion

            base.Draw(gameTime);
        }
    }
}
