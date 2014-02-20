using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cutlass;
using Cutlass.Assets;
using Cutlass.GameComponents;
using Cutlass.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PirateyGame.Screens
{
    class MessageBoxScreen : GameScreen
    {
        #region Fields

        string message;

        #endregion

        #region Events

        public event EventHandler<EventArgs> Accepted;
        public event EventHandler<EventArgs> Cancelled;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor automatically includes the standard "A=ok, B=cancel"
        /// usage text prompt.
        /// </summary>
        public MessageBoxScreen(string message)
            : this(message, false)
        {
            TextureManager.AddTexture(new CutlassTexture("Content/Textures/gradient"), "gradient");
        }

        /// <summary>
        /// Constructor lets the caller specify whether to include the standard
        /// "A=ok, B=cancel" usage text prompt.
        /// </summary>
        public MessageBoxScreen(string message, bool includeUsageText)
        {
            string usageText = String.Empty;

            switch (CutlassEngine.CurrentPlatform)
            {
                case PlatformID.MacOSX:
                    goto case PlatformID.Win32Windows;
                case PlatformID.Unix:
                    goto case PlatformID.Win32Windows;
                case PlatformID.Win32NT:
                    goto case PlatformID.Win32Windows;
                case PlatformID.Win32S:
                    goto case PlatformID.Win32Windows;
                case PlatformID.Win32Windows:
                    usageText = "\nSpace, Enter = ok\nEsc = cancel";
                    break;
                case PlatformID.WinCE:
                    goto case PlatformID.Win32Windows;
                case PlatformID.Xbox:
                    usageText = "\nSA button = ok\nB button = cancel";
                    break;
                default:
                    break;
            }

            if (includeUsageText)
                this.message = message + usageText;
            else
                this.message = message;

            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Responds to user input, accepting or cancelling the message box.
        /// </summary>
        public override void HandleInput(Input input)
        {
            // We pass in our ControllingPlayer, which may either be null (to
            // accept input from any player) or a specific index. If we pass a null
            // controlling player, the InputState helper returns to us which player
            // actually provided the input. We pass that through to our Accepted and
            // Cancelled events, so they can tell which player triggered them.
            if (input.MenuSelect)
            {
                // Raise the accepted event, then exit the message box.
                if (Accepted != null)
                    Accepted(this, new EventArgs());

                ExitScreen();
            }
            else if (input.MenuCancel)
            {
                // Raise the cancelled event, then exit the message box.
                if (Cancelled != null)
                    Cancelled(this, new EventArgs());

                ExitScreen();
            }
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draws the message box.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // Darken down any other screens that were drawn beneath the popup.
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            SpriteFont font = FontManager.GetSpriteFontOrDefault(String.Empty);

            // Center the message text in the viewport.
            Vector2 viewportSize = new Vector2(CutlassEngine.Device.Viewport.Width, CutlassEngine.Device.Viewport.Height);
            Vector2 textSize = font.MeasureString(message);
            Vector2 textPosition = (viewportSize - textSize) / 2;

            // The background includes a border somewhat larger than the text itself.
            const int hPad = 32;
            const int vPad = 16;

            Rectangle backgroundRectangle = new Rectangle((int)textPosition.X - hPad,
                                                          (int)textPosition.Y - vPad,
                                                          (int)textSize.X + hPad * 2,
                                                          (int)textSize.Y + vPad * 2);

            // Fade the popup alpha during transitions.
            Color color = Color.White * TransitionAlpha;

            ScreenManager.SpriteBatch.Begin();

            // Draw the background rectangle.
            ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture2D("gradient"), backgroundRectangle, color);

            // Draw the message box text.
            ScreenManager.SpriteBatch.DrawString(font, message, textPosition, color);

            ScreenManager.SpriteBatch.End();
        }

        #endregion
    }
}
