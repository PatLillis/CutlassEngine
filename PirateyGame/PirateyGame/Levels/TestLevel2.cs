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
            _PlayerInitialPosition = new Vector2(400, 400);
        }

        public override void LoadContent()
        {
            _Scenery.Add(new Scenery(new Vector2(200, 200), texture: new CutlassTexture("Content/Textures/Sprites/planks-800-80")));
            _Scenery.Add(new Scenery(new Vector2(-100, 500), texture: new CutlassTexture("Content/Textures/Sprites/planks-800-80")));

            base.LoadContent();
        }

        #endregion Initialization
    }
}
