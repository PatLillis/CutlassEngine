using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BoundingRect;
using Cutlass.Assets;
using Cutlass.GameComponents;
using Cutlass.Interfaces;
using Cutlass.Managers;
using Cutlass.Utilities;

namespace PirateyGame.SceneObjects
{
    public class Player : ICutlassDrawable,
                          ICutlassUpdateable,
                          ICutlassLoadable,
                          ICutlassCollidable,
                          ICutlassMovable
    {
        #region Fields

        private TexId _PlayerTest_Id;

        const float MAX_PLAYER_VERTICAL_SPEED = 1f;
        const float MAX_PLAYER_HORIZONTAL_SPEED = 1f;

        #endregion Fields

        #region Properties

        public SceneObjectId SceneObjectId { get; set; }

        public bool Active
        {
            get { return _Active; }
            set { _Active = value; }
        }
        private bool _Active;

        public int DrawOrder
        {
            get { return _DrawOrder; }
            set { _DrawOrder = value; }
        }
        private int _DrawOrder = 0;

        public bool ScreenPositionFixed
        {
            get { return _ScreenPositionFixed; }
        }
        private bool _ScreenPositionFixed = false;

        public bool ReadyToRender
        {
            get { return _ReadyToRender; }
        }
        private bool _ReadyToRender = false;

        public bool IsLoaded
        {
            get { return _IsLoaded; }
        }
        private bool _IsLoaded = false;

        public bool IsVisible
        {
            get { return _IsVisible; }
            set { _IsVisible = value; }
        }
        private bool _IsVisible = false;

        public int Width
        {
            get { return TextureManager.GetTexture(_PlayerTest_Id).Width; }
        }

        public int Height
        {
            get { return TextureManager.GetTexture(_PlayerTest_Id).Height; }
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

        #region ICutlassMovable

        public Vector2 Velocity
        {
            get { return _Velocity; }
            set { _Velocity = value; }
        }
        private Vector2 _Velocity = Vector2.Zero;

        public float GravityCoefficient { get { return 1; } }

        public float FrictionCoefficient { get { return 1; } }

        #endregion ICutlassMovable

        #region ICutlassCollidable

        public CollisionSide Side
        {
            get { return CollisionSide.All; }
        }

        public CollisionCategory Category
        {
            get { return CollisionCategory.Good; }
        }

        public CollisionCategory CategoryMask
        {
            get { return CollisionCategory.Bad | CollisionCategory.Scenery;  }
        }

        #endregion ICutlassCollidable

        #endregion Properties

        #region Events

        public event EventHandler<BoundingRectangleEventArgs> PlayerMoved;

        #endregion Events

        #region Initialization

        public Player(ICutlassTexture texture, Vector2 position, string fontKey = "")
        {
            _PlayerTest_Id = TextureManager.AddTexture(texture);
            _Active = true;

            _Position = position;

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

        #region Public Methods

        public void HandleInput(Input input)
        {
            KeyboardState keyboardState = input.CurrentKeyboardState;
            GamePadState gamePadState = input.CurrentGamePadState;

            // Otherwise move the player vector.
            Vector2 movement = Vector2.Zero;

            if (keyboardState.IsKeyDown(Keys.Left))
                _Velocity.X = Math.Max(_Velocity.X - 1.0f, -MAX_PLAYER_HORIZONTAL_SPEED);

            if (keyboardState.IsKeyDown(Keys.Right))
                _Velocity.X = Math.Min(_Velocity.X + 1.0f, MAX_PLAYER_HORIZONTAL_SPEED);

            if (keyboardState.IsKeyDown(Keys.Up))
                _Velocity.Y = Math.Max(_Velocity.Y - 1.0f, -MAX_PLAYER_VERTICAL_SPEED);

            if (keyboardState.IsKeyDown(Keys.Down))
                _Velocity.Y = Math.Min(_Velocity.Y + 1.0f, MAX_PLAYER_VERTICAL_SPEED);

            Console.WriteLine(_Velocity);
            //Vector2 thumbstick = gamePadState.ThumbSticks.Left;

            //movement.X += thumbstick.X;
            //movement.Y -= thumbstick.Y;

            //if (movement.Length() > 1)
            //    movement.Normalize();

            //movement.X *= PLAYER_VERTICAL_SPEED;
            //movement.Y *= PLAYER_HORIZONTAL_SPEED;

            //_Position += movement;

            //OnPlayerMoved();
        }

        protected internal void OnPlayerMoved()
        {
            if (PlayerMoved != null)
                PlayerMoved(this, new BoundingRectangleEventArgs(BoundingRect));
        }

        public void CollisionDetected(ICutlassCollidable collisionTarget, BoundingRectangle intersection, Vector2 offset)
        {
            switch(collisionTarget.Category)
            {
                case CollisionCategory.Scenery:
                    _Position += offset;
                    break;
            }
        }

        #endregion

        #region Update and Draw

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            CutlassAnimatedTexture texture = (CutlassAnimatedTexture)TextureManager.GetTexture(_PlayerTest_Id);

            spriteBatch.Draw(texture.BaseTexture, Position, texture.AreaToRender, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            ((CutlassAnimatedTexture)TextureManager.GetTexture(_PlayerTest_Id)).Update(gameTime);
        }

        #endregion Update and Draw
    }
}
