using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cutlass;
using Cutlass.Managers;
using PirateyGame.Screens;
//using Cutlass.Managers;

namespace PirateyGame
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
#if !XBOX360
        [STAThread]
#endif
        static void Main(string[] args)
        {
            StartGame();
        }

        private static void StartGame()
        {
            using (CutlassEngine game = new CutlassEngine("Piratey Game"))
            {
                SetupScene();
                game.Run();
            }
        }

        private static void SetupScene()
        {
            ScreenManager.AddScreen(new ScrollMenuScreen("Main Menu"));
            //ScreenManager.AddScreen(new BackgroundScreen());
            //ScreenManager.AddScreen(new MainMenuScreen());
        }
    }
}
