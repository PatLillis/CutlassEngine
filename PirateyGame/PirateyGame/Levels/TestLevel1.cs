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
            _Scenery.Add(new Scenery(new Vector2(200, 200), texture: new CutlassTexture("Content/Sprites/planks-800-80")));
            _Scenery.Add(new Scenery(new Vector2(120, -600), texture: new CutlassTexture("Content/Sprites/planks-80-800")));
            _Scenery.Add(new Scenery(new Vector2(-100, 450), texture: new CutlassTexture("Content/Sprites/planks-800-80")));
            _Scenery.Add(new LevelTransition(new Vector2(800, 100), 200, 200, LevelDirectory.TestLevel2));
            base.LoadContent();
        }

        #endregion Initialization
    }
}
