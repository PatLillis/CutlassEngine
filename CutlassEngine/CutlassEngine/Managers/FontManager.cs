using System.Collections.Generic;
using Cutlass.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cutlass.Managers
{
    /// <summary>
    /// Handle all fonts for the game.
    /// </summary>
    public class FontManager : GameComponent
    {
        #region Properties

        /// <summary>Where the actual fonts are stored, accessed by a string key</summary>
        private static Dictionary<string, ICutlassFont> _Fonts = new Dictionary<string, ICutlassFont>();

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
        public static int FontsLoaded
        {
            get { return _FontsLoaded; }
        }
        private static int _FontsLoaded = 0;

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
                if (!font.ReadyToRender)
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
        /// <param name="textureName"></param>
        public static void AddFont(ICutlassFont newFont, string fontName)
        {
            if (fontName != null && !_Fonts.ContainsKey(fontName))
            {
                _Fonts.Add(fontName, newFont);
                if (_Initialized)
                {
                    newFont.LoadContent();
                }
            }
        }

        /// <summary>
        /// Remove a font.
        /// </summary>
        /// <param name="fontName"></param>
        public static void RemoveFont(string fontName)
        {
            if (fontName != null && _Fonts.ContainsKey(fontName))
            {
                if (_Initialized)
                {
                    _Fonts[fontName].UnloadContent();
                    _Fonts.Remove(fontName);
                    _FontsLoaded--;
                }
            }
        }

        /// <summary>
        /// Get a font
        /// </summary>
        /// <param name="textureId"></param>
        /// <returns></returns>
        public static ICutlassFont GetFont(string fontName)
        {
            if (fontName != null && _Fonts.ContainsKey(fontName))
            {
                return _Fonts[fontName];
            }
            return null;
        }

        /// <summary>
        /// Get a SpriteFont object
        /// </summary>
        /// <param name="fontName"></param>
        /// <returns></returns>
        public static SpriteFont GetSpriteFont(string fontName)
        {
            ICutlassFont font = GetFont(fontName);
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
        public static SpriteFont GetSpriteFontOrDefault(string fontName)
        {
            return GetSpriteFont(fontName) ?? DefaultFont;
        }

        /// <summary>
        /// Set the default font for the game.
        /// </summary>
        /// <param name="defaultFont"></param>
        public static void SetDefaultFont(ICutlassFont defaultFont)
        {
            if (!defaultFont.ReadyToRender)
            {
                defaultFont.LoadContent();
            }

            _DefaultFont = defaultFont.Font;
        }

        #endregion Public Methods
    }
}
