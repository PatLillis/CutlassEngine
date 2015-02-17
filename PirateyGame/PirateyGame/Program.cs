using System;
using Cutlass;
using Cutlass.Managers;
using PirateyGame.Screens;
using Cutlass.Utilities;
using Cutlass.Assets;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace PirateyGame
{
    /// <summary>
    /// Main Program/Entry Point
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
#if !XBOX360
        [STAThread]
#endif
        private static void Main(string[] args)
        {
            StartGame();
        }

        /// <summary>
        /// Start up the game/engine
        /// </summary>
        private static void StartGame()
        {
            using (CutlassEngine engine = new CutlassEngine())
            {
                CutlassEngine.Game = engine;
                SetupScene();
                engine.Run();
            }
        }

        /// <summary>
        /// Add initial screens
        /// </summary>
        private static void SetupScene()
        {
            CutlassEngine.BackgroundColor = Palette.Black;
            ScreenManager.AddScreen(new BackgroundScreen());
            ScreenManager.AddScreen(new MainMenuScreen());

        }
    }
}
