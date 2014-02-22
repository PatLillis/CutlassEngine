using System;
using Cutlass;
using Cutlass.Managers;
using PirateyGame.Screens;

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
            ScreenManager.AddScreen(new BackgroundScreen());
            ScreenManager.AddScreen(new MainMenuScreen());
        }
    }
}
