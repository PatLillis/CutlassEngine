using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cutlass;
using Cutlass.GameComponents;
using Cutlass.Managers;
using Cutlass.Utilities;
using Microsoft.Xna.Framework.Graphics;

namespace PirateyGame.Screens
{
    /// <summary>
    /// Options Menu
    /// </summary>
    class OptionsMenuScreen : ScrollMenuScreen
    {
        #region Properties

        /// <summary></summary>
        private MenuEntry _ResolutionMenuEntry;
        private MenuEntry _FullScreenEntry;
        private MenuEntry _MusicMenuEntry;
        private MenuEntry _SfxMenuEntry;
        private MenuEntry _LanguageMenuEntry;
        private MenuEntry _InsultsMenuEntry;
        private MenuEntry _OceanColorMenuEntry;

        /// <summary>
        /// Available Resolutions
        /// </summary>
        private List<DisplayMode> AvailableResolutions
        {
            get
            {
                if (_AvailableResolutions == null)
                {
                    _AvailableResolutions = new List<DisplayMode>();

                    foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes.Where(mode => mode.Width >= 1024 && mode.Height >= 768))
                    {
                        if (!_AvailableResolutions.Any(r => r.Width == mode.Width && r.Height == mode.Height))
                            _AvailableResolutions.Add(mode);
                    }
                }

                return _AvailableResolutions;
            }
        }
        private List<DisplayMode> _AvailableResolutions = null;

        private int _MusicUpdateTimer = -1;

        private int _SfxUpdateTimer = -1;

        /// <summary>For switching back and forth without selecting.</summary>
        private int _TemporaryResolution = 0;

        /// <summary>Current resolution</summary>
        private int _CurrentResolution = 0;

        /// <summary>Available Locales</summary>
        private static List<string> _AvailableLocales = new List<string>
        {
            "en-US",
            "fr-FR",
            "es-ES",
            "ko-KR"
        };

        /// <summary>Current Language</summary>
        private static string _CurrentLanguage
        {
            get { return CultureInfo.GetCultureInfo(_AvailableLocales[_CurrentLocale]).DisplayName; }
        }

        /// <summary>Current Locale (index)</summary>
        private static int _CurrentLocale = 0;

        /// <summary>Available Ocean Colors</summary>
        private static string[] _AvailableOceanColors = { "Blue", "Mauve", "Red With Blood", "Chartreuse", "Other (specify)" };

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base("Options")
        {
            int resolutionIndex = AvailableResolutions.FindIndex(d => d.Width == GameSettings.Default.ResolutionWidth &&
                                                              d.Height == GameSettings.Default.ResolutionHeight);

            if (resolutionIndex != -1)
            {
                _CurrentResolution = resolutionIndex;
                _TemporaryResolution = resolutionIndex;
            }

            int localeIndex = _AvailableLocales.FindIndex(l => l == GameSettings.Default.Locale);

            if (localeIndex != -1)
            {
                _CurrentLocale = localeIndex;
            }

            // Create our menu entries.
            _ResolutionMenuEntry = new MenuEntry(string.Empty);
            _FullScreenEntry = new MenuEntry(string.Empty);
            _MusicMenuEntry = new MenuEntry(string.Empty);
            _SfxMenuEntry = new MenuEntry(string.Empty);
            _LanguageMenuEntry = new MenuEntry(string.Empty);
            _InsultsMenuEntry = new MenuEntry(string.Empty);
            _OceanColorMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            _ResolutionMenuEntry.Selected += ResolutionMenuEntrySelected;
            _ResolutionMenuEntry.Right += ResolutionMenuEntryRight;
            _ResolutionMenuEntry.Left += ResolutionMenuEntryLeft;

            _FullScreenEntry.Selected += FullscreenMenuEntrySelected;

            _MusicMenuEntry.Right += MusicMenuEntryRight;
            _MusicMenuEntry.StillRight += MusicMenuEntryRight;
            _MusicMenuEntry.RightReleased += MusicMenuEntryReleased;
            _MusicMenuEntry.Left += MusicMenuEntryLeft;
            _MusicMenuEntry.StillLeft += MusicMenuEntryLeft;
            _MusicMenuEntry.LeftReleased += MusicMenuEntryReleased;

            _SfxMenuEntry.Right += SfxMenuEntryRight;
            _SfxMenuEntry.StillRight += SfxMenuEntryRight;
            _SfxMenuEntry.RightReleased += SfxMenuEntryReleased;
            _SfxMenuEntry.Left += SfxMenuEntryLeft;
            _SfxMenuEntry.StillLeft += SfxMenuEntryLeft;
            _SfxMenuEntry.LeftReleased += SfxMenuEntryReleased;

            _LanguageMenuEntry.Selected += LanguageMenuEntrySelected;

            _InsultsMenuEntry.Selected += InsultsMenuEntrySelected;

            _OceanColorMenuEntry.Selected += OceanColorMenuEntrySelected;

            back.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(_ResolutionMenuEntry);
            MenuEntries.Add(_FullScreenEntry);
            MenuEntries.Add(_MusicMenuEntry);
            MenuEntries.Add(_SfxMenuEntry);
            MenuEntries.Add(_LanguageMenuEntry);
            MenuEntries.Add(_InsultsMenuEntry);
            MenuEntries.Add(_OceanColorMenuEntry);
            MenuEntries.Add(back);
        }

        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            _ResolutionMenuEntry.Text = "Screen Resolution: " + AvailableResolutions[_TemporaryResolution].Width + " x " + AvailableResolutions[_TemporaryResolution].Height;
            _FullScreenEntry.Text = "Fullscreen: " + (GameSettings.Default.Fullscreen ? "Yes" : "No");
            _MusicMenuEntry.Text = "Music Level: " + GameSettings.Default.MusicVolume;
            _SfxMenuEntry.Text = "SFX Level: " + GameSettings.Default.SfxVolume;
            _LanguageMenuEntry.Text = "Language: " + _CurrentLanguage;
            _InsultsMenuEntry.Text = "Devastating Insults: " + (GameSettings.Default.Insults ? "On" : "Off");
            _OceanColorMenuEntry.Text = "Ocean Color: " + _AvailableOceanColors[GameSettings.Default.OceanColor];
        }

        #endregion

        #region Handle Input

        void ResolutionMenuEntrySelected(object sender, EventArgs e)
        {
            if (_CurrentResolution != _TemporaryResolution)
            {
                _CurrentResolution = _TemporaryResolution;

                GameSettings.Default.ResolutionWidth = AvailableResolutions[_CurrentResolution].Width;
                GameSettings.Default.ResolutionHeight = AvailableResolutions[_CurrentResolution].Height;

                CutlassEngine.ApplyResolutionChange();

                SetMenuEntryText();
            }
        }

        void ResolutionMenuEntryRight(object sender, EventArgs e)
        {
            _TemporaryResolution = (_TemporaryResolution + 1) % AvailableResolutions.Count;
            SetMenuEntryText();
        }

        void ResolutionMenuEntryLeft(object sender, EventArgs e)
        {
            _TemporaryResolution = (_TemporaryResolution - 1);
            if (_TemporaryResolution < 0)
                _TemporaryResolution = AvailableResolutions.Count - 1;

            SetMenuEntryText();
        }


        void FullscreenMenuEntrySelected(object sender, EventArgs e)
        {
            GameSettings.Default.Fullscreen = !GameSettings.Default.Fullscreen;
            CutlassEngine.ApplyResolutionChange();

            SetMenuEntryText();
        }


        void MusicMenuEntryRight(object sender, EventArgs e)
        {
            _MusicUpdateTimer = (_MusicUpdateTimer + 1) % Input.MenuEntryBuffer;

            if (_MusicUpdateTimer == 0)
            {
                int tempVal = (GameSettings.Default.MusicVolume + 1);
                if (tempVal >= 0 && tempVal <= 100)
                {
                    GameSettings.Default.MusicVolume = tempVal;

                    SetMenuEntryText();
                }
            }
        }

        void MusicMenuEntryLeft(object sender, EventArgs e)
        {
            _MusicUpdateTimer = (_MusicUpdateTimer + 1) % Input.MenuEntryBuffer;

            if (_MusicUpdateTimer == 0)
            {
                int tempVal = (GameSettings.Default.MusicVolume - 1);
                if (tempVal >= 0 && tempVal <= 100)
                {
                    GameSettings.Default.MusicVolume = tempVal;

                    SetMenuEntryText();
                }
            }
        }

        void MusicMenuEntryReleased(object sender, EventArgs e)
        {
            _MusicUpdateTimer = -1;

            GameSettings.Save();
        }

        void SfxMenuEntryRight(object sender, EventArgs e)
        {
            _SfxUpdateTimer = (_SfxUpdateTimer + 1) % Input.MenuEntryBuffer;

            if (_SfxUpdateTimer == 0)
            {
                int tempVal = (GameSettings.Default.SfxVolume + 1);
                if (tempVal >= 0 && tempVal <= 100)
                {
                    GameSettings.Default.SfxVolume = tempVal;

                    SetMenuEntryText();
                }
            }
        }

        void SfxMenuEntryLeft(object sender, EventArgs e)
        {
            _SfxUpdateTimer = (_SfxUpdateTimer + 1) % Input.MenuEntryBuffer;

            if (_SfxUpdateTimer == 0)
            {
                int tempVal = (GameSettings.Default.SfxVolume - 1);
                if (tempVal >= 0 && tempVal <= 100)
                {
                    GameSettings.Default.SfxVolume = tempVal;

                    SetMenuEntryText();
                }
            }
        }

        void SfxMenuEntryReleased(object sender, EventArgs e)
        {
            _SfxUpdateTimer = -1;

            GameSettings.Save();
        }

        void LanguageMenuEntrySelected(object sender, EventArgs e)
        {
            _CurrentLocale = (_CurrentLocale + 1) % _AvailableLocales.Count;
            GameSettings.Default.Locale = _AvailableLocales[_CurrentLocale];
            GameSettings.Save();

            SetMenuEntryText();
        }


        void InsultsMenuEntrySelected(object sender, EventArgs e)
        {
            GameSettings.Default.Insults = !GameSettings.Default.Insults;
            GameSettings.Save();

            SetMenuEntryText();
        }


        void OceanColorMenuEntrySelected(object sender, EventArgs e)
        {
            GameSettings.Default.OceanColor = (GameSettings.Default.OceanColor + 1) % _AvailableOceanColors.Length;
            GameSettings.Save();

            SetMenuEntryText();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Override to make sure we take into account the max width of the entries.
        /// </summary>
        /// <returns></returns>
        public override int MenuEntryWidth()
        {
            SpriteFont entryFont;

            //resolution
            entryFont = FontManager.GetSpriteFontOrDefault(_ResolutionMenuEntry.EntryFontKey);
            string rOption = "";
            float rOptionWidth = -1.0f;
            foreach (DisplayMode r in AvailableResolutions)
            {
                float rWidth = entryFont.MeasureString(r.Width + " x " + r.Height).X;
                if (rWidth > rOptionWidth)
                {
                    rOptionWidth = rWidth;
                    rOption = r.Width + " x " + r.Height;
                }
            }
            int resolutionWidth = (int)entryFont.MeasureString("Screen Resolution: " + rOption).X;

            //FullScreen
            entryFont = FontManager.GetSpriteFontOrDefault(_FullScreenEntry.EntryFontKey);
            int fullscreenWidth = (int)entryFont.MeasureString("Fullscreen: Yes").X;

            //Music
            entryFont = FontManager.GetSpriteFontOrDefault(_MusicMenuEntry.EntryFontKey);
            int musicWidth = (int)entryFont.MeasureString("Music Level: 100").X;

            //FX
            entryFont = FontManager.GetSpriteFontOrDefault(_SfxMenuEntry.EntryFontKey);
            int fxWidth = (int)entryFont.MeasureString("SFX Level: 100").X;

            //Language
            entryFont = FontManager.GetSpriteFontOrDefault(_LanguageMenuEntry.EntryFontKey);
            string lOption = "";
            float lOptionWidth = -1.0f;
            foreach (string l in _AvailableLocales)
            {
                float lWidth = entryFont.MeasureString(CultureInfo.GetCultureInfo(l).DisplayName).X;
                if (lWidth > lOptionWidth)
                {
                    lOptionWidth = lWidth;
                    lOption = CultureInfo.GetCultureInfo(l).DisplayName;
                }
            }
            int languageWidth = (int)entryFont.MeasureString("Language: " + lOption).X;

            //Insults
            entryFont = FontManager.GetSpriteFontOrDefault(_InsultsMenuEntry.EntryFontKey);
            int insultsWidth = (int)entryFont.MeasureString("Devastating Insults: Off").X;

            //Ocean Color
            entryFont = FontManager.GetSpriteFontOrDefault(_OceanColorMenuEntry.EntryFontKey);
            string cOption = "";
            float cOptionWidth = -1.0f;
            foreach (string c in _AvailableOceanColors)
            {
                float cWidth = entryFont.MeasureString(c).X;
                if (cWidth > cOptionWidth)
                {
                    cOptionWidth = cWidth;
                    cOption = c;
                }
            }
            int oceanColorsWidth = (int)entryFont.MeasureString("Ocean Color: " + cOption).X;

            int overallMaxWidth = -1;
            new List<int> { resolutionWidth, fullscreenWidth, musicWidth, fxWidth, languageWidth, insultsWidth, oceanColorsWidth }
                .ForEach(w => overallMaxWidth = Math.Max(overallMaxWidth, w));

            return overallMaxWidth;
        }

        #endregion
    }
}
