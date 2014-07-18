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
    public class Scenery : ICutlassDrawable, ICutlassUpdateable, ICutlassLoadable, ICutlassCollidable
    {
        #region Fields

        private TexId _SceneryObject_Id;
        private bool _Animated;

        #endregion Fields

        #region Properties

        public SceneObjectId SceneObjectId { get; set; }

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

        public BoundingRectangle CurrentFrameBoundingRect
        {
            get { return new BoundingRectangle(_Position.X, _Position.Y, Width, Height); }
        }

        public BoundingRectangle NextFrameBoundingRect
        {
            get { return CurrentFrameBoundingRect; }
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

        #region ICutlassCollidable

        public CollisionSide Side
        {
            get { return CollisionSide.All; }
        }

        public CollisionCategory Category
        {
            get { return CollisionCategory.Scenery; }
        }

        public CollisionCategory CategoryMask
        {
            get { return CollisionCategory.All; }
        }

        #endregion ICutlassCollidable

        #endregion Properties

        #region Initialization

        public Scenery(Vector2 position, ICutlassTexture texture, bool animated = false)
        {
            _Position = position;
            _SceneryObject_Id = TextureManager.AddTexture(texture);
            _Active = true;
            _IsVisible = true;
            _Animated = animated;
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

        #region Public Methods

        public void CollisionDetected(ICutlassCollidable collisionTarget, BoundingRectangle intersection, Vector2 adjustmentDirection)
        {
            //Console.WriteLine("Collided with Scenery!");
        }

        #endregion Public Methods

        #region Update and Draw

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            ICutlassTexture texture = TextureManager.GetTexture(_SceneryObject_Id);

            spriteBatch.Draw(texture.BaseTexture, Position, texture.AreaToRender, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            if (_Animated)
                ((ICutlassUpdateable)TextureManager.GetTexture(_SceneryObject_Id)).Update(gameTime);
        }

        #endregion Update and Draw
    }
}
