using System.Collections.Generic;
using Cutlass.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cutlass.Managers
{
    public class FontManager : GameComponent
    {
        private static Dictionary<string, ICutlassFont> _Fonts = new Dictionary<string, ICutlassFont>();

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

        /// <summary>
        /// The number of fonts that are currently loaded.
        /// Use this for user loading bar feedback.
        /// </summary>
        public static int FontsLoaded
        {
            get { return _FontsLoaded; }
        }
        private static int _FontsLoaded = 0;

        /// <summary>
        /// Create the font Manager.
        /// </summary>
        /// <param name="game"></param>
        public FontManager(Game game)
            : base(game)
        { }

        /// <summary>
        /// Add a font of type CutlassTexture.
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

        public static void SetDefaultFont(ICutlassFont defaultFont)
        {
            if (!defaultFont.ReadyToRender)
            {
                defaultFont.LoadContent();
            }

            _DefaultFont = defaultFont.Font;
        }

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
    }
}
