using System;
using System.Collections.Generic;
using System.Linq;
using Cutlass;
using Cutlass.GameComponents;
using Cutlass.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cutlass.Utilities;

namespace PirateyGame.Screens
{
    /// <summary>
    /// Menu Screen
    /// </summary>
    abstract class MenuScreen : GameScreen
    {
        #region Properties

        /// <summary>Which menu entry is selected</summary>
        private int _SelectedEntry = 0;

        /// <summary>Title of this menu</summary>
        public string MenuTitle
        {
            get { return _MenuTitle; }
        }
        protected string _MenuTitle = String.Empty;

        /// <summary>Which font the title should use</summary>
        protected FontId _TitleFont_Id;

        /// <summary>Title Color</summary>
        public Color TitleColor
        {
            get { return _TitleColor; }
        }
        protected Color _TitleColor = Color.White;

        /// <summary>
        /// Gets the list of menu entries, so derived classes can add
        /// or change the menu contents.
        /// </summary>
        protected IList<MenuEntry> MenuEntries
        {
            get { return _MenuEntries; }
        }
        protected List<MenuEntry> _MenuEntries = new List<MenuEntry>();

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public MenuScreen(string menuTitle)
        {
            this._MenuTitle = menuTitle;

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
                _SelectedEntry--;

                if (_SelectedEntry < 0)
                    _SelectedEntry = _MenuEntries.Count - 1;
            }

            // Move to the next menu entry?
            if (input.MenuDown)
            {
                _SelectedEntry++;

                if (_SelectedEntry >= _MenuEntries.Count)
                    _SelectedEntry = 0;
            }

            #region Right

            // Move the current entry right?
            if (input.MenuRight)
            {
                OnRightEntry(_SelectedEntry);
            }
            //Keep moving the current entry right?
            else if (input.MenuStillRight)
            {
                OnStillRight(_SelectedEntry);
            }
            //Right released
            else if (input.MenuRightReleased)
            {
                OnRightReleased(_SelectedEntry);
            }

            #endregion Right

            #region Left

            // Move the current entry left?
            if (input.MenuLeft)
            {
                OnLeftEntry(_SelectedEntry);
            }
            //Keep moving the current entry let?
            else if (input.MenuStillLeft)
            {
                OnStillLeft(_SelectedEntry);
            }
            //Right released
            else if (input.MenuLeftReleased)
            {
                OnLeftReleased(_SelectedEntry);
            }

    #endregion Left

            // Select the current entry?
            if (input.MenuSelect)
            {
                OnSelectEntry(_SelectedEntry);
            }
            // Cancel?
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
        /// Handler for when the user inputs a "right" menu action.
        /// </summary>
        protected virtual void OnRightEntry(int entryIndex)
        {
            _MenuEntries[entryIndex].OnRightEntry();
        }

        /// <summary>
        /// Handler for when the user is still inputting to the right.
        /// </summary>
        protected virtual void OnStillRight(int entryIndex)
        {
            _MenuEntries[entryIndex].OnStillRight();
        }

        /// <summary>
        /// Handler for right key released
        /// </summary>
        protected virtual void OnRightReleased(int entryIndex)
        {
            _MenuEntries[entryIndex].OnRightReleased();
        }

        /// <summary>
        /// Handler for when the user inputs a "left" menu action.
        /// </summary>
        protected virtual void OnLeftEntry(int entryIndex)
        {
            _MenuEntries[entryIndex].OnLeftEntry();
        }

        /// <summary>
        /// Handler for when the user is still inputting to the left.
        /// </summary>
        protected virtual void OnStillLeft(int entryIndex)
        {
            _MenuEntries[entryIndex].OnStillLeft();
        }

        /// <summary>
        /// Handler for left key released
        /// </summary>
        protected virtual void OnLeftReleased(int entryIndex)
        {
            _MenuEntries[entryIndex].OnLeftReleased();
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

            int totalMenuEntryHeights = MenuEntryHeight();

            //Center menu entries on bottom half of screen, or at least try to.
            float startingHeight = (CutlassEngine.Device.Viewport.Height * 0.75f) - (totalMenuEntryHeights / 2f);

            // Buffer menu entries so they don't hit the bottom of the screen.
            if (startingHeight < CutlassEngine.Device.Viewport.Height / 2)
            {
                startingHeight = CutlassEngine.Device.Viewport.Height - (totalMenuEntryHeights) - 10;
            }

            Vector2 position = new Vector2(0f, startingHeight);

            // update each menu entry's location in turn
            foreach(MenuEntry menuEntry in _MenuEntries)
            {
                // each entry is to be centered horizontally
                position.X = CutlassEngine.Device.Viewport.Width / 2;

                if (ScreenState == ScreenState.TransitionOn)
                    position.X -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                // set the entry's position
                menuEntry.Position = position;

                // move down for the next entry the size of this entry
                position.Y += menuEntry.GetHeight();
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
                bool isSelected = IsActive && (i == _SelectedEntry);

                _MenuEntries[i].Update(isSelected, gameTime);
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
            SpriteFont font = FontManager.GetSpriteFontOrDefault(_TitleFont_Id);

            spriteBatch.Begin();

            #region Draw Menu Options

            // Draw each menu entry in turn.
            for (int i = 0; i < _MenuEntries.Count; i++)
            {
                MenuEntry menuEntry = _MenuEntries[i];

                bool isSelected = IsActive && (i == _SelectedEntry);

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
            Vector2 titleSize = font.MeasureString(_MenuTitle);
            Color titleColor = TitleColor * TransitionAlpha;
            float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            //Draw title _Text
            spriteBatch.DrawString(font, _MenuTitle, titlePosition, titleColor, 0,
                                    titleSize / 2, titleScale, SpriteEffects.None, 0);

            #endregion

            spriteBatch.End();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets max width of all menu entries
        /// Specific menus should override to get maximum width including any possible changes (options, etc.)
        /// </summary>
        /// <returns></returns>
        public virtual int MenuEntryWidth()
        {
            int maxWidth = -1;

            foreach (MenuEntry menuEntry in _MenuEntries)
            {
                maxWidth = Math.Max(menuEntry.GetWidth(), maxWidth);
            }

            return maxWidth;
        }

        public virtual int MenuEntryHeight()
        {
            return _MenuEntries.Sum(m => m.GetHeight());
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

        /// <summary>
        /// Set font for all menu entries
        /// </summary>
        /// <param name="fontKey"></param>
        public void SetMenuEntryFont(FontId fontId)
        {
            foreach (MenuEntry entry in MenuEntries)
                entry.Entry_Id = fontId;
        }

        #endregion
    }
}
