using BoundingRect;
using Cutlass.Assets;
using Cutlass.Interfaces;
using Cutlass.Managers;
using Cutlass.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace PirateyGame.SceneObjects
{
    public class Scenery : ICutlassDrawable, ICutlassUpdateable, ICutlassLoadable
    {
        #region Fields

        private TexId _SceneryObject_Id;

        #endregion Fields

        #region Properties

        public bool Active
        {
            get { return _Active; }
            set { _Active = value; }
        }
        private bool _Active;

        public bool IsLoaded
        {
            get { return _IsLoaded; }
        }
        private bool _IsLoaded = false;

        public int DrawOrder
        {
            get { return _DrawOrder; }
            set { _DrawOrder = value; }
        }
        private int _DrawOrder = 0;

        public bool ScreenPositionFixed
        {
            get { return false; }
        }

        public bool ReadyToRender
        {
            get { return _ReadyToRender; }
        }
        private bool _ReadyToRender = false;

        public bool IsVisible
        {
            get { return _IsVisible; }
            set { _IsVisible = value; }
        }
        private bool _IsVisible = true;

        public int Width
        {
            get { return TextureManager.GetTexture(_SceneryObject_Id).Width; }
        }

        public int Height
        {
            get { return TextureManager.GetTexture(_SceneryObject_Id).Height; }
        }

        public BoundingRectangle BoundingRect
        {
            get { return new BoundingRectangle(_Position.X, _Position.Y, Width, Height); }
        }

        public Vector2 Position
        {
            get { return _Position; }
            set { _Position = value; }
        }
        private Vector2 _Position;

        public Vector2 Scale
        {
            get { return _Scale; }
            set { _Scale = value; }
        }
        private Vector2 _Scale;

        public float Rotation
        {
            get { return _Rotation; }
            set { _Rotation = value; }
        }
        private float _Rotation;

        #endregion Properties

        #region Initialization

        public Scenery(Vector2 position, ICutlassTexture texture)
        {
            _Position = position;
            _SceneryObject_Id = TextureManager.AddTexture(texture);
            _Active = true;
            _IsVisible = true;
        }

        public void LoadContent()
        {
            _IsLoaded = true;
        }

        public void UnloadContent()
        {
            _IsLoaded = false;
        }

        #endregion Initialization

        #region Update and Draw

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            CutlassAnimatedTexture texture = (CutlassAnimatedTexture)TextureManager.GetTexture(_SceneryObject_Id);

            spriteBatch.Draw(texture.BaseTexture, Position, texture.AreaToRender, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            ((CutlassAnimatedTexture)TextureManager.GetTexture(_SceneryObject_Id)).Update(gameTime);
        }

        #endregion Update and Draw
    }
}
