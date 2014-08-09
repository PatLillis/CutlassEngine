using System;
using Microsoft.Xna.Framework;
using Cutlass.Assets;
using Cutlass.Utilities;
using PirateyGame.Screens;
using PirateyGame.SceneObjects;

namespace PirateyGame.Levels
{
    public class TestLevel2 : GameplayScreen
    {
        #region Initialization

        public TestLevel2()
            : base()
        {
            _PlayerInitialPosition = new Vector2(400, 330);
            _CameraInitialPositionCenter = _PlayerInitialPosition;
        }

        public override void LoadContent()
        {
            _Scenery.Add(new Scenery(new Vector2(600, 240), texture: new CutlassTexture("Content/Sprites/planks-800-80")));
            _Scenery.Add(new Scenery(new Vector2(300, 490), texture: new CutlassTexture("Content/Sprites/planks-800-80")));
            _Scenery.Add(new Scenery(new Vector2(-50, 250), texture: new CutlassTexture("Content/Sprites/topOnlyPlatform-200-100"), side: CollisionSide.Top));
            _Scenery.Add(new LevelTransition(new Vector2(-500, 200), 200, 200, LevelDirectory.TestLevel1));

            base.LoadContent();
        }

        #endregion Initialization
    }
}
