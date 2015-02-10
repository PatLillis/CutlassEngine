using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cutlass.Interfaces;
using Cutlass.Utilities;

namespace Cutlass.Managers
{
    /// <summary>
    /// Handle all fonts for the game.
    /// </summary>
    public class FontManager : GameComponent
    {
        #region Properties

        /// <summary>Where the actual fonts are stored, accessed by a string key</summary>
        private static Dictionary<FontId, ICutlassFont> _Fonts = new Dictionary<FontId, ICutlassFont>();

        /// <summary>The default font to use</summary>
        public static SpriteFont DefaultFont
        {
            get { return _DefaultFont; }
        }
        private static SpriteFont _DefaultFont = null;

        /// <summary>Is the FontManager Initialized.</summary>
        public static bool Initialized
        {
            get { return _Initialized; }
        }
        private static bool _Initialized = false;

        /// <summary>The number of fonts that are currently loaded.</summary>
        private static int _NextFontId = 0;

        private static Object _FontIdLock = new Object();

        #endregion Properties

        #region Initialization

        /// <summary>
        /// Create the font Manager.
        /// </summary>
        /// <param name="game"></param>
        public FontManager(Game game)
            : base(game)
        { }

        /// <summary>
        /// Create the font manager.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            //Set DefaultFont (if it hasn't already been set)
            if (_DefaultFont == null)
            {
                _DefaultFont = CutlassEngine.ContentManager.Load<SpriteFont>("Content/Fonts/defaultFont");
            }

            foreach (ICutlassFont font in _Fonts.Values)
            {
                if (!font.IsLoaded)
                {
                    font.LoadContent();
                }
            }

            _Initialized = true;
        }

        #endregion Initialization

        #region Public Methods

        /// <summary>
        /// Add a font of type ICutlassFont.
        /// </summary>
        /// <param name="newTexture"></param>
        public static FontId AddFont(ICutlassFont newFont)
        {
            FontId fontId;

            lock (_FontIdLock)
            {
                fontId = _NextFontId++;
            }

            _Fonts.Add(fontId, newFont);

            if (_Initialized)
                newFont.LoadContent();

            return fontId;
        }

        /// <summary>
        /// Remove a font.
        /// </summary>
        /// <param name="fontId"></param>
        public static void RemoveFont(FontId fontId)
        {
            ICutlassFont fontToRemove;
            _Fonts.TryGetValue(fontId, out fontToRemove);

            if (fontToRemove != null)
            {
                if (_Initialized)
                    fontToRemove.UnloadContent();

                _Fonts.Remove(fontId);
            }
        }

        /// <summary>
        /// Get a font
        /// </summary>
        /// <param name="textureId"></param>
        /// <returns></returns>
        public static ICutlassFont GetFont(FontId fontId)
        {
            return _Fonts.ElementAtOrDefault(fontId).Value;
        }

        /// <summary>
        /// Get a SpriteFont object
        /// </summary>
        /// <param name="fontName"></param>
        /// <returns></returns>
        public static SpriteFont GetSpriteFont(FontId fontId)
        {
            ICutlassFont font = GetFont(fontId);

            if (font != null)
                return font.Font;
            else
                return null;
        }

        /// <summary>
        /// Get a SpriteFont, or return the default font if not found.
        /// </summary>
        /// <param name="fontName"></param>
        /// <returns></returns>
        public static SpriteFont GetSpriteFontOrDefault(FontId fontId)
        {
            return GetSpriteFont(fontId) ?? DefaultFont;
        }

        /// <summary>
        /// Set the default font for the game.
        /// </summary>
        /// <param name="defaultFont"></param>
        public static void SetDefaultFont(ICutlassFont defaultFont)
        {
            if (!defaultFont.IsLoaded)
                defaultFont.LoadContent();

            _DefaultFont = defaultFont.Font;
        }

        #endregion Public Methods
    }
}
