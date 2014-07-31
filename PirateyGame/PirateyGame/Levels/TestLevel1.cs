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
            _PlayerInitialPosition = new Vector2(400, 100);
        }

        public override void LoadContent()
        {
            _Scenery.Add(new Scenery(new Vector2(200, 200), texture: new CutlassTexture("Content/Textures/Sprites/planks-800-80")));
            _Scenery.Add(new Scenery(new Vector2(120, -600), texture: new CutlassTexture("Content/Textures/Sprites/planks-80-800")));
            _Scenery.Add(new Scenery(new Vector2(0, 200), texture: new CutlassTexture("Content/Textures/Sprites/topOnlyPlatform-200-100"), side: CollisionSide.Top));
            _Scenery.Add(new Scenery(new Vector2(-100, 500), texture: new CutlassTexture("Content/Textures/Sprites/planks-800-80")));
            _Scenery.Add(new LevelTransition(new Vector2(600, 200), 200, 200, LevelDirectory.TestLevel2));

            base.LoadContent();
        }

        #endregion Initialization
    }
}
