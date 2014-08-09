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

        protected TexId _SceneryObject_Id;
        protected bool _Animated;

        #endregion Fields

        #region Properties

        public SceneObjectId SceneObjectId { get; set; }

        public bool Active
        {
            get { return _Active; }
            set { _Active = value; }
        }
        protected bool _Active;

        public bool IsLoaded
        {
            get { return _IsLoaded; }
        }
        protected bool _IsLoaded = false;

        public int DrawOrder
        {
            get { return _DrawOrder; }
            set { _DrawOrder = value; }
        }
        protected int _DrawOrder = 0;

        public bool ScreenPositionFixed
        {
            get { return false; }
        }

        public bool ReadyToRender
        {
            get { return _ReadyToRender; }
        }
        protected bool _ReadyToRender = false;

        public bool IsVisible
        {
            get { return _IsVisible; }
            set { _IsVisible = value; }
        }
        protected bool _IsVisible = true;

        public virtual float Width
        {
            get { return TextureManager.GetTexture(_SceneryObject_Id).Width; }
        }

        public virtual float Height
        {
            get { return TextureManager.GetTexture(_SceneryObject_Id).Height; }
        }

        public virtual BoundingRectangle CurrentFrameBoundingRect
        {
            get { return new BoundingRectangle(_Position.X - Width / 2, _Position.Y - Height / 2, Width, Height); }
        }

        public virtual BoundingRectangle NextFrameBoundingRect
        {
            get { return CurrentFrameBoundingRect; }
        }

        public Vector2 Position
        {
            get { return _Position; }
            set { _Position = value; }
        }
        protected Vector2 _Position;

        public Vector2 Scale
        {
            get { return _Scale; }
            set { _Scale = value; }
        }
        protected Vector2 _Scale;

        public float Rotation
        {
            get { return _Rotation; }
            set { _Rotation = value; }
        }
        protected float _Rotation;

        #region ICutlassCollidable

        public virtual bool Stationary
        {
            get { return true; }
        }

        public virtual Vector2 Velocity
        {
            get { return Vector2.Zero; }
        }

        public virtual Vector2 PositionCorrection
        {
            get { return Vector2.Zero; }
        }

        public virtual CollisionSide Side
        {
            get { return _Side; }
        }
        private CollisionSide _Side;

        public virtual CollisionCategory Category
        {
            get { return CollisionCategory.Scenery; }
        }

        public virtual CollisionCategory CategoryMask
        {
            get { return CollisionCategory.Good | CollisionCategory.Bad; }
        }

        #endregion ICutlassCollidable

        #endregion Properties

        #region Initialization

        public Scenery(Vector2 position, bool isVisible = true, ICutlassTexture texture = null, bool animated = false, CollisionSide side = CollisionSide.All)
        {
            _Position = position;
            _IsVisible = isVisible;
            _Active = true;

            if (isVisible && texture != null)
            {
                _SceneryObject_Id = TextureManager.AddTexture(texture);
                _Animated = animated;
                _Side = side;
            }
            else
            {
                _Side = CollisionSide.All;
            }
        }

        public virtual void LoadContent()
        {
            _IsLoaded = true;
        }

        public virtual void UnloadContent()
        {
            _IsLoaded = false;
        }

        #endregion Initialization

        #region Public Methods

        public virtual void CollisionDetected(ICutlassCollidable collisionTarget)
        { }

        public virtual void CollisionDetectedWithCorrection(ICutlassCollidable collisionTarget, Vector2 normal, float distance)
        { }

        #endregion Public Methods

        #region Update and Draw

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_IsVisible)
            {
                ICutlassTexture texture = TextureManager.GetTexture(_SceneryObject_Id);

                spriteBatch.Draw(texture.BaseTexture, CurrentFrameBoundingRect.Min, texture.AreaToRender, Color.White);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            if (_Animated && _IsVisible)
                ((ICutlassUpdateable)TextureManager.GetTexture(_SceneryObject_Id)).Update(gameTime);
        }

        #endregion Update and Draw
    }
}
