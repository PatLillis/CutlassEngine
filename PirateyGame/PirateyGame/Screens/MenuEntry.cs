using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cutlass.Managers;
using Cutlass.Utilities;

namespace PirateyGame.Screens
{
    /// <summary>
    /// Helper class represents a single entry in a MenuScreen. By default this
    /// just draws the entry text string, but it can be customized to display menu
    /// entries in different ways. This also provides an event that will be raised
    /// when the menu entry is selected.
    /// </summary>
    class MenuEntry
    {
        #region Properties

        /// <summary>Tracks a fading selection effect on the entry.</summary>
        private float _SelectionFade;

        /// <summary>Tracks the pulsating effect on the entry.</summary>
        private float _Pulsate = 0.0f;

        /// <summary>
        /// The text rendered for this entry.
        /// </summary>
        public string Text
        {
            get { return _Text; }
            set { _Text = value; }
        }
        private string _Text;

        /// <summary>Normal Text Color</summary>
        public Color TextColor = Color.LightSlateGray;
        /// <summary>Selected Text Color</summary>
        public Color SelectedTextColor = Color.Yellow;

        /// <summary>Which font this menu entry should use</summary>
        public FontId Entry_Id;

        /// <summary>The position at which the entry is drawn</summary>
        public Vector2 Position
        {
            get { return _Position; }
            set { _Position = value; }
        }
        private Vector2 _Position;

        #endregion

        #region Events

        /// <summary>Event raised when the menu entry is selected</summary>
        public event EventHandler<EventArgs> Selected;

        /// <summary>Event raised when the menu entry should go to the right</summary>
        public event EventHandler<EventArgs> Right;

        /// <summary>Event raised when the right action is still being fired</summary>
        public event EventHandler<EventArgs> StillRight;

        /// <summary>Event raised when the right action is released.</summary>
        public event EventHandler<EventArgs> RightReleased;

        /// <summary>Event raised when the menu entry should go to the left</summary>
        public event EventHandler<EventArgs> Left;

        /// <summary>Event raised when the left action is still being fired</summary>
        public event EventHandler<EventArgs> StillLeft;

        /// <summary>Event raised when the left action is released.</summary>
        public event EventHandler<EventArgs> LeftReleased;

        /// <summary>
        /// Method for raising the Selected event.
        /// </summary>
        protected internal virtual void OnSelectEntry()
        {
            if (Selected != null)
                Selected(this, new EventArgs());
        }

        /// <summary>
        /// Method for raising the Right event.
        /// </summary>
        protected internal virtual void OnRightEntry()
        {
            if (Right != null)
                Right(this, new EventArgs());
        }

        /// <summary>
        /// Method for raising the StillRight event
        /// </summary>
        protected internal virtual void OnStillRight()
        {
            if (StillRight != null)
                StillRight(this, new EventArgs());
        }

        /// <summary>
        /// Method for raising the RightReleased event
        /// </summary>
        protected internal virtual void OnRightReleased()
        {
            if (RightReleased != null)
                RightReleased(this, new EventArgs());
        }

        /// <summary>
        /// Method for raising the Left event.
        /// </summary>
        protected internal virtual void OnLeftEntry()
        {
            if (Left != null)
                Left(this, new EventArgs());
        }

        /// <summary>
        /// Method for raising the StillLeft event
        /// </summary>
        protected internal virtual void OnStillLeft()
        {
            if (StillLeft != null)
                StillLeft(this, new EventArgs());
        }

        /// <summary>
        /// Method for raising the LeftReleased event
        /// </summary>
        protected internal virtual void OnLeftReleased()
        {
            if (LeftReleased != null)
                LeftReleased(this, new EventArgs());
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Constructs a new menu entry with the specified _Text.
        /// </summary>
        public MenuEntry(string text)
        {
            this._Text = text;
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Updates the menu entry.
        /// </summary>
        public virtual void Update(bool isSelected, GameTime gameTime)
        {
            // there is no such thing as a selected item on Windows Phone, so we always
            // force isSelected to be false
#if WINDOWS_PHONE
            isSelected = false;
#endif

            // When the menu selection changes, entries gradually fade between
            // their selected and deselected appearance, rather than instantly
            // popping to the new state.
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (isSelected)
                _SelectionFade = Math.Min(_SelectionFade + (fadeSpeed * 4f), 1);
            else
                _SelectionFade = Math.Max(_SelectionFade - (fadeSpeed * 4f), 0);

            if (isSelected && _Pulsate - 0.001 < Math.PI)
                _Pulsate += fadeSpeed;
            else
                _Pulsate = 0.0f;
        }

        /// <summary>
        /// Draws the menu entry. This can be overridden to customize the appearance.
        /// </summary>
        public virtual void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            // Draw _Text, centered on the middle of each line.
            SpriteFont font = FontManager.GetSpriteFontOrDefault(Entry_Id);

            // Draw the selected entry
            Color color = isSelected ? SelectedTextColor : TextColor;

            // Pulsate the size of the selected menu entry.
            double time = gameTime.TotalGameTime.TotalSeconds;

            float pulsate = (float)Math.Sin(_Pulsate * 6) + 1;

            float scale = 1 + pulsate * 0.05f * _SelectionFade;

            // Modify the alpha to fade _Text out during transitions.
            color *= screen.TransitionAlpha;

            Vector2 origin = new Vector2(font.MeasureString(_Text).X / 2, font.LineSpacing / 2);

            spriteBatch.DrawString(font, _Text, _Position, color, 0,
                                   origin, scale, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Queries how much space this menu entry requires.
        /// </summary>
        public virtual int GetHeight()
        {
            return FontManager.GetSpriteFontOrDefault(Entry_Id).LineSpacing;
        }

        /// <summary>
        /// Queries how wide the entry is, used for centering on the screen.
        /// </summary>
        public virtual int GetWidth()
        {
            return (int)FontManager.GetSpriteFontOrDefault(Entry_Id).MeasureString(Text).X;
        }

        #endregion
    }
}
