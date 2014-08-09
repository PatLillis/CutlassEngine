using System;
using Microsoft.Xna.Framework;
using Cutlass.Assets;
using Cutlass.Utilities;
using PirateyGame.Screens;
using PirateyGame.SceneObjects;

namespace PirateyGame.Levels
{
    public class TestLevel1 : GameplayScreen
    {
        #region Initialization

        public TestLevel1()
            : base()
        {
            _PlayerInitialPosition = new Vector2(400, 80);
            _CameraInitialPositionCenter = _PlayerInitialPosition;
        }

        public override void LoadContent()
        {
            _Scenery.Add(new Scenery(new Vector2(600, 240), texture: new CutlassTexture("Content/Sprites/planks-800-80")));
            //_Scenery.Add(new Scenery(new Vector2(120, -600), texture: new CutlassAnimatedTexture("Content/Sprites/planks-animated-240-800", 3), animated: true));
            _Scenery.Add(new Scenery(new Vector2(300, 480), texture: new CutlassTexture("Content/Sprites/planks-800-80")));
            _Scenery.Add(new LevelTransition(new Vector2(900, 200), 200, 200, LevelDirectory.TestLevel2));
            base.LoadContent();
        }

        #endregion Initialization
    }
}
