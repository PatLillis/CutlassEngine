using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Cutlass;
using Cutlass.Managers;
using Cutlass.Utilities;
using Microsoft.Xna.Framework.Graphics;

namespace PirateyGame.Screens
{
    class OptionsMenuScreen : ScrollMenuScreen
    {
        #region Fields

        MenuEntry _ResolutionMenuEntry;
        MenuEntry _FullScreenEntry;
        //MenuEntry musicMenuEntry;
        //MenuEntry fxMenuEntry;
        MenuEntry _LanguageMenuEntry;
        MenuEntry _InsultsMenuEntry;
        MenuEntry _OceanColorMenuEntry;

        List<DisplayMode> _Resolutions = null;
        List<DisplayMode> Resolutions
        {
            get
            {
                if (_Resolutions == null)
                {
                    _Resolutions = new List<DisplayMode>();

                    foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes.Where(mode => mode.Width >= 1024 && mode.Height >= 768))
                    {
                        if (!_Resolutions.Any(r => r.Width == mode.Width && r.Height == mode.Height))
                            _Resolutions.Add(mode);
                    }
                }

                return _Resolutions;
            }
        }

        //for switching back and forth without selecting.
        int temporaryResolution = 0;
        int currentResolution = 0;

        //static int musicLevel = 100;

        //static int fxLevel = 100;

        static List<string> locales = new List<string>
        {
            "en-US",
            "fr-FR",
            "es-ES",
            "ko-KR"
        };

        static int currentLocale = 0;
        static string currentLanguage
        {
            get
            {
                return CultureInfo.GetCultureInfo(locales[currentLocale]).DisplayName;
            }
        }

        static bool insults = true;

        static string[] oceanColors = { "Blue", "Mauve", "Red With Blood", "Chartreuse", "Other (specify)" };
        static int currentOceanColor = 0;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base("Options")
        {
            int resolutionIndex = Resolutions.FindIndex(d => d.Width == GameSettings.Default.ResolutionWidth &&
                                                              d.Height == GameSettings.Default.ResolutionHeight);

            if (resolutionIndex != -1)
            {
                currentResolution = resolutionIndex;
                temporaryResolution = resolutionIndex;
            }

            int localeIndex = locales.FindIndex(l => l == GameSettings.Default.Locale);

            if (localeIndex != -1)
            {
                currentLocale = localeIndex;
            }

            // Create our menu entries.
            _ResolutionMenuEntry = new MenuEntry(string.Empty);
            _FullScreenEntry = new MenuEntry(string.Empty);
            //musicMenuEntry = new MenuEntry(string.Empty);
            //fxMenuEntry = new MenuEntry(string.Empty);
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
            //musicMenuEntry.Selected += MusicMenuEntrySelected;
            //fxMenuEntry.Selected += FxMenuEntrySelected;
            _LanguageMenuEntry.Selected += LanguageMenuEntrySelected;
            _InsultsMenuEntry.Selected += InsultsMenuEntrySelected;
            _OceanColorMenuEntry.Selected += OceanColorMenuEntrySelected;
            back.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(_ResolutionMenuEntry);
            MenuEntries.Add(_FullScreenEntry);
            //MenuEntries.Add(musicMenuEntry);
            //MenuEntries.Add(fxMenuEntry);
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
            _ResolutionMenuEntry.Text = "Screen Resolution: " + Resolutions[temporaryResolution].Width + " x " + Resolutions[temporaryResolution].Height;
            _FullScreenEntry.Text = "Fullscreen: " + (GameSettings.Default.Fullscreen ? "Yes" : "No");
            //musicMenuEntry.Text = "Music Level: " + musicLevel;
            //fxMenuEntry.Text = "SFX Level: " + fxLevel;
            _LanguageMenuEntry.Text = "Language: " + currentLanguage;
            _InsultsMenuEntry.Text = "Devastating Insults: " + (GameSettings.Default.Insults ? "On" : "Off");
            _OceanColorMenuEntry.Text = "Ocean Color: " + oceanColors[GameSettings.Default.OceanColor];
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the Resolution menu entry is selected.
        /// </summary>
        void ResolutionMenuEntrySelected(object sender, EventArgs e)
        {
            if (currentResolution != temporaryResolution)
            {
                currentResolution = temporaryResolution;

                GameSettings.Default.ResolutionWidth = Resolutions[currentResolution].Width;
                GameSettings.Default.ResolutionHeight = Resolutions[currentResolution].Height;

                CutlassEngine.ApplyResolutionChange();

                SetMenuEntryText();
            }
        }

        /// <summary>
        /// Event handler for when the Resolution menu entry is selected.
        /// </summary>
        void ResolutionMenuEntryRight(object sender, EventArgs e)
        {
            temporaryResolution = (temporaryResolution + 1) % Resolutions.Count;
            SetMenuEntryText();
        }
        /// <summary>
        /// Event handler for when the Resolution menu entry is selected.
        /// </summary>
        void ResolutionMenuEntryLeft(object sender, EventArgs e)
        {
            temporaryResolution = (temporaryResolution - 1);
            if (temporaryResolution < 0)
                temporaryResolution = Resolutions.Count - 1;

            SetMenuEntryText();
        }

        /// <summary>
        /// Event handler for when the Fullscreen menu entry is selected.
        /// </summary>
        void FullscreenMenuEntrySelected(object sender, EventArgs e)
        {
            GameSettings.Default.Fullscreen = !GameSettings.Default.Fullscreen;
            CutlassEngine.ApplyResolutionChange();

            SetMenuEntryText();
        }

        /// <summary>
        /// Event handler for when the Fullscreen menu entry is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //void MusicMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        //{
        //    musicLevel = (musicLevel - 89) % 11 + 90;

        //    SetMenuEntryText();
        //}

        /// <summary>
        /// Event handler for when the Fullscreen menu entry is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //void FxMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        //{
        //    fxLevel = (fxLevel - 89) % 11 + 90;

        //    SetMenuEntryText();
        //}

        /// <summary>
        /// Event handler for when the Language menu entry is selected.
        /// </summary>
        void LanguageMenuEntrySelected(object sender, EventArgs e)
        {
            currentLocale = (currentLocale + 1) % locales.Count;
            GameSettings.Default.Locale = locales[currentLocale];
            GameSettings.Save();

            SetMenuEntryText();
        }

        void InsultsMenuEntrySelected(object sender, EventArgs e)
        {
            GameSettings.Default.Insults = !GameSettings.Default.Insults;
            GameSettings.Save();

            SetMenuEntryText();
        }

        /// <summary>
        /// Event handler for when the Ocean Color menu entry is selected.
        /// </summary>
        void OceanColorMenuEntrySelected(object sender, EventArgs e)
        {
            GameSettings.Default.OceanColor = (GameSettings.Default.OceanColor + 1) % oceanColors.Length;
            GameSettings.Save();

            SetMenuEntryText();
        }

        #endregion

        #region Public Methods

        public override int MaxEntryWidth()
        {
            SpriteFont entryFont;

            //resolution
            entryFont = FontManager.GetSpriteFontOrDefault(_ResolutionMenuEntry.EntryFontKey);
            string rOption = "";
            float rOptionWidth = -1.0f;
            foreach (DisplayMode r in Resolutions)
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
            //entryFont = ScreenManager.SpriteFonts[musicMenuEntry.Font];
            //int musicWidth = (int)entryFont.MeasureString("Music Level: " + "100").X;

            ////FX
            //entryFont = ScreenManager.SpriteFonts[fxMenuEntry.Font];
            //int fxWidth = (int)entryFont.MeasureString("SFX Level: " + "100").X;

            //Language
            entryFont = FontManager.GetSpriteFontOrDefault(_LanguageMenuEntry.EntryFontKey);
            string lOption = "";
            float lOptionWidth = -1.0f;
            foreach (string l in locales)
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
            foreach (string c in oceanColors)
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
            new List<int> { resolutionWidth, fullscreenWidth, /*musicWidth, fxWidth, */languageWidth, insultsWidth, oceanColorsWidth }
                .ForEach(w => overallMaxWidth = Math.Max(overallMaxWidth, w));

            return overallMaxWidth;
        }

        #endregion
    }
}
