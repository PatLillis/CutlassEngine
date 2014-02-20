using System;
using System.Collections.Generic;
using Cutlass;
using Cutlass.GameComponents;
using Cutlass.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PirateyGame.Screens
{
    abstract class MenuScreen : GameScreen
    {
        #region Fields

        int selectedEntry = 0;
        string menuTitle;

        #endregion

        #region Properties

        protected Color _TitleColor = Color.White;
        /// <summary>Title Color</summary>
        public virtual Color TitleColor
        {
            get { return _TitleColor; }
        }

        protected List<MenuEntry> _MenuEntries = new List<MenuEntry>();
        /// <summary>
        /// Gets the list of menu entries, so derived classes can add
        /// or change the menu contents.
        /// </summary>
        protected IList<MenuEntry> MenuEntries
        {
            get { return _MenuEntries; }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public MenuScreen(string menuTitle, string menuFontIndex = null)
        {
            this.menuTitle = menuTitle;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Responds to user input, changing the selected entry and accepting
        /// or cancelling the menu.
        /// </summary>
        public override void HandleInput(Input input)
        {
            // Move to the previous menu entry?
            if (input.MenuUp)
            {
                selectedEntry--;

                if (selectedEntry < 0)
                    selectedEntry = _MenuEntries.Count - 1;
            }

            // Move to the next menu entry?
            if (input.MenuDown)
            {
                selectedEntry++;

                if (selectedEntry >= _MenuEntries.Count)
                    selectedEntry = 0;
            }

            if (input.MenuRight)
            {
                OnRightEntry(selectedEntry);
            }
            
            if (input.MenuLeft)
            {
                OnLeftEntry(selectedEntry);
            }

            if (input.MenuSelect)
            {
                OnSelectEntry(selectedEntry);
            }
            else if (input.MenuCancel)
            {
                OnCancel();
            }
        }

        /// <summary>
        /// Handler for when the user has chosen a menu entry.
        /// </summary>
        protected virtual void OnSelectEntry(int entryIndex)
        {
            _MenuEntries[entryIndex].OnSelectEntry();
        }

        /// <summary>
        /// Handler for when the user has chosen a menu entry.
        /// </summary>
        protected virtual void OnRightEntry(int entryIndex)
        {
            _MenuEntries[entryIndex].OnRightEntry();
        }

        /// <summary>
        /// Handler for when the user has chosen a menu entry.
        /// </summary>
        protected virtual void OnLeftEntry(int entryIndex)
        {
            _MenuEntries[entryIndex].OnLeftEntry();
        }

        /// <summary>
        /// Handler for when the user has cancelled the menu.
        /// </summary>
        protected virtual void OnCancel()
        {
            ExitScreen();
        }

        /// <summary>
        /// Helper overload makes it easy to use OnCancel as a MenuEntry event handler.
        /// </summary>
        protected void OnCancel(object sender, EventArgs e)
        {
            OnCancel();
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Allows the screen the chance to position the menu entries. By default
        /// all menu entries are lined up in a vertical list, centered on the screen.
        /// </summary>
        protected virtual void UpdateMenuEntryLocations()
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            //Center menu entries on bottom half of screen, or at least try to.
            int lineSpacing = FontManager.DefaultFont.LineSpacing;
            float startingHeight = (CutlassEngine.Device.Viewport.Height * 0.75f) - (lineSpacing * _MenuEntries.Count / 2f);

            // Buffer menu entries so they don't hit the bottom of the screen.
            if (startingHeight < CutlassEngine.Device.Viewport.Height / 2)
            {
                startingHeight = CutlassEngine.Device.Viewport.Height - (lineSpacing * _MenuEntries.Count) - lineSpacing / 2;
            }

            // start at Y = 175; each X value is generated per entry
            Vector2 position = new Vector2(0f, startingHeight);

            // update each menu entry's location in turn
            foreach(MenuEntry menuEntry in _MenuEntries)
            {
                // each entry is to be centered horizontally
                //position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - menuEntry.GetWidth(this) / 2;
                position.X = CutlassEngine.Device.Viewport.Width / 2;

                if (ScreenState == ScreenState.TransitionOn)
                    position.X -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                // set the entry's position
                menuEntry.Position = position;

                // move down for the next entry the size of this entry
                position.Y += menuEntry.GetHeight(this);
            }
        }

        /// <summary>
        /// Updates the menu.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // Update each nested MenuEntry object.
            for (int i = 0; i < _MenuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);

                _MenuEntries[i].Update(this, isSelected, gameTime);
            }
        }

        /// <summary>
        /// Draws the menu.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // make sure our entries are in the right place before we draw them
            UpdateMenuEntryLocations();

            GraphicsDevice graphics = CutlassEngine.Device;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = FontManager.DefaultFont;

            spriteBatch.Begin();

            #region Draw Menu Options

            //Rectangle menuOptionsBackground = new Rectangle();

            //The 1.1 is for the "pulsating effect of the menu options.
            //menuOptionsBackground.Width = (int)(MaxEntryWidth() * 1.1) - 64;
            //menuOptionsBackground.Height = -64;
            //for (int i = 0; i < menuEntries.Count; i++)
            //    menuOptionsBackground.Height += menuEntries[i].GetHeight(this);
            //menuOptionsBackground.X = (int)menuEntries[0].Position.X - menuOptionsBackground.Width / 2;
            //menuOptionsBackground.Y = (int)menuEntries[0].Position.Y - ScreenManager.SpriteFonts[Fonts.PirateyMenuMedium].LineSpacing / 2 + 32;

            ////UR corner
            //spriteBatch.Draw(MenuBackgroundCorner,
            //                 new Vector2(menuOptionsBackground.Right, menuOptionsBackground.Top - 64),
            //                 null,
            //                 Color.White,
            //                 0.0f,
            //                 Vector2.Zero,
            //                 1.0f,
            //                 SpriteEffects.None,
            //                 1.0f);
            ////BR corner
            //spriteBatch.Draw(MenuBackgroundCorner,
            //                 new Vector2(menuOptionsBackground.Right, menuOptionsBackground.Bottom),
            //                 null,
            //                 Color.White,
            //                 0.0f,
            //                 Vector2.Zero,
            //                 1.0f,
            //                 SpriteEffects.FlipVertically,
            //                 1.0f);
            ////BL corner
            //spriteBatch.Draw(MenuBackgroundCorner,
            //                 new Vector2(menuOptionsBackground.Left - 64, menuOptionsBackground.Bottom),
            //                 null,
            //                 Color.White,
            //                 0.0f,
            //                 Vector2.Zero,
            //                 1.0f,
            //                 SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally,
            //                 1.0f);
            ////UL corner
            //spriteBatch.Draw(MenuBackgroundCorner,
            //                 new Vector2(menuOptionsBackground.Left - 64, menuOptionsBackground.Top - 64),
            //                 null,
            //                 Color.White,
            //                 0.0f,
            //                 Vector2.Zero,
            //                 1.0f,
            //                 SpriteEffects.FlipHorizontally,
            //                 1.0f);
            ////Top edge
            //spriteBatch.Draw(MenuBackgroundVerticalEdge,
            //                 new Vector2(menuOptionsBackground.Left, menuOptionsBackground.Top - 64),
            //                 null,
            //                 Color.White,
            //                 0.0f,
            //                 Vector2.Zero,
            //                 new Vector2(menuOptionsBackground.Width / 64.0f, 1.0f),
            //                 SpriteEffects.None,
            //                 1.0f);
            ////Right edge
            //spriteBatch.Draw(MenuBackgroundHorizontalEdge,
            //                 new Vector2(menuOptionsBackground.Right, menuOptionsBackground.Top),
            //                 null,
            //                 Color.White,
            //                 0.0f,
            //                 Vector2.Zero,
            //                 new Vector2(1.0f, menuOptionsBackground.Height / 64.0f),
            //                 SpriteEffects.None,
            //                 1.0f);
            ////Bottom edge
            //spriteBatch.Draw(MenuBackgroundVerticalEdge,
            //                 new Vector2(menuOptionsBackground.Left, menuOptionsBackground.Bottom),
            //                 null,
            //                 Color.White,
            //                 0.0f,
            //                 Vector2.Zero,
            //                 new Vector2(menuOptionsBackground.Width / 64.0f, 1.0f),
            //                 SpriteEffects.FlipVertically,
            //                 1.0f);
            ////Left edge
            //spriteBatch.Draw(MenuBackgroundHorizontalEdge,
            //                 new Vector2(menuOptionsBackground.Left - 64, menuOptionsBackground.Top),
            //                 null,
            //                 Color.White,
            //                 0.0f,
            //                 Vector2.Zero,
            //                 new Vector2(1.0f, menuOptionsBackground.Height / 64.0f),
            //                 SpriteEffects.FlipHorizontally,
            //                 1.0f);
            ////Fill
            //spriteBatch.Draw(MenuBackgroundHorizontalEdge,
            //    menuOptionsBackground,
            //    new Rectangle(0, 0, 1, 1),
            //    Color.White);

            // Draw each menu entry in turn.
            for (int i = 0; i < _MenuEntries.Count; i++)
            {
                MenuEntry menuEntry = _MenuEntries[i];

                bool isSelected = IsActive && (i == selectedEntry);

                menuEntry.Draw(this, isSelected, gameTime);
            }

            #endregion

            #region Draw Title

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Draw the menu title centered on the screen
            Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, 80);
            Vector2 titleSize = font.MeasureString(menuTitle);
            Color titleColor = TitleColor * TransitionAlpha;
            float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            //Draw Scroll behind menu title (properly sized)
            //80 = 40px drawable space on each scroll end image * 2
            //int middleScrollCount = ((int)(titleSize.X * 1.25f) - 80) / TitleScrollMiddle.Width;
            ////If there is leftover, add one.
            //if (((int)(titleSize.X * 1.25f) - 80) % TitleScrollMiddle.Width > 0)
            //{
            //    middleScrollCount += 1;
            //}
            //int totalMiddleScrollWidth = middleScrollCount * TitleScrollMiddle.Width;

            //Vector2 leftScrollPosition = new Vector2((graphics.Viewport.Width / 2) - (totalMiddleScrollWidth / 2),
            //    titlePosition.Y);
            //Vector2 leftScrollOrigin = new Vector2(TitleScrollEdge.Width, TitleScrollEdge.Height / 2);

            //Vector2 rightScrollPosition = new Vector2((graphics.Viewport.Width / 2) + (totalMiddleScrollWidth / 2),
            //    titlePosition.Y);
            //Vector2 rightScrollOrigin = new Vector2(0, TitleScrollEdge.Height / 2);

            //Vector2 middleScrollPosition = new Vector2((graphics.Viewport.Width / 2) - (totalMiddleScrollWidth / 2) + (TitleScrollMiddle.Width / 2), titlePosition.Y);
            //Vector2 middleScrollOrigin = new Vector2(TitleScrollMiddle.Width / 2, TitleScrollMiddle.Height / 2);


            #endregion

            //Draw Left Scroll Handle
            //spriteBatch.Draw(TitleScrollEdge, leftScrollPosition, null, Color.White, 0.0f, leftScrollOrigin, 1.0f, SpriteEffects.FlipHorizontally, 1.0f);
            //Draw Right Scroll Handle
            //spriteBatch.Draw(TitleScrollEdge, rightScrollPosition, null, Color.White, 0.0f, rightScrollOrigin, 1.0f, SpriteEffects.None, 1.0f);
            //Draw Middle of Scroll
            //for (int i = 0; i < middleScrollCount; i++)
            //{
            //    spriteBatch.Draw(TitleScrollMiddle, middleScrollPosition, null, Color.White, 0.0f, middleScrollOrigin, 1.0f, SpriteEffects.None, 1.0f);
            //    middleScrollPosition.X += TitleScrollMiddle.Width;
            //}

            //Draw title text
            spriteBatch.DrawString(font, menuTitle, titlePosition, titleColor, 0,
                                    titleSize / 2, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets max width of all menu entries
        /// Specific menus should override to get maximum width including any possible changes (options, etc.)
        /// </summary>
        /// <returns></returns>
        public virtual int MaxEntryWidth()
        {
            int maxWidth = -1;

            foreach (MenuEntry menuEntry in _MenuEntries)
            {
                maxWidth = Math.Max(menuEntry.GetWidth(this), maxWidth);
            }

            return maxWidth;
        }

        /// <summary>
        /// Set Regular Text Color for all Menu Entries.
        /// </summary>
        /// <param name="textColor"></param>
        public void SetMenuEntryTextColor(Color textColor)
        {
            foreach (MenuEntry entry in MenuEntries)
                entry.TextColor = textColor;
        }

        /// <summary>
        /// Set Selected Text Color for all Menu Entries.
        /// </summary>
        /// <param name="selectedTextColor"></param>
        public void SetMenuEntrySelectedTextColor(Color selectedTextColor)
        {
            foreach (MenuEntry entry in MenuEntries)
                entry.SelectedTextColor = selectedTextColor;
        }

        #endregion
    }
}
