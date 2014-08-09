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
using PirateyGame.Items;
using System.Collections.Generic;
using PirateyGame.Screens;

namespace PirateyGame.SceneObjects
{
    public class Player : ICutlassDrawable,
                          ICutlassUpdateable,
                          ICutlassLoadable,
                          ICutlassCollidable,
                          ICutlassMovable
    {
        #region Fields

        private TexId _Player_TexId_Walking_R;
        private TexId _Player_TexId_Walking_L;
        private TexId _Player_TexId_Walking_Reverse_R;
        private TexId _Player_TexId_Walking_Reverse_L;
        private TexId _Player_TexId_Standing_R;
        private TexId _Player_TexId_Standing_L;
        private TexId _Player_TexId_Jumping_R;
        private TexId _Player_TexId_Jumping_L;
        private TexId _CurrentTexture;

        const float MAX_PLAYER_VERTICAL_SPEED = 0.3f;
        const float MAX_PLAYER_HORIZONTAL_SPEED = 0.3f;

        private bool _WasOnGround = false;

        private Vector2 _LookDirection = Vector2.UnitX;
        /// <summary>-1: L; 0: none; +1: R</summary>
        private int _WalkDirection = 0;

        #endregion Fields

        #region Properties

        public GameplayScreen ParentScreen
        {
            get { return _ParentScreen; }
        }
        private GameplayScreen _ParentScreen;

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
        private int _DrawOrder = 1;

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

        public float Width
        {
            get { return TextureManager.GetTexture(_CurrentTexture).Width; }
        }

        public float Height
        {
            get { return TextureManager.GetTexture(_CurrentTexture).Height; }
        }

        public BoundingRectangle CurrentFrameBoundingRect
        {
            get { return new BoundingRectangle(_Position.X - Width / 2, _Position.Y - Height / 2, Width, Height); }
        }

        public BoundingRectangle NextFrameBoundingRect
        {
            get { return new BoundingRectangle(_Position.X - Width / 2 + _Velocity.X, _Position.Y - Height / 2 + _Velocity.Y, Width, Height); }
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

        public bool IsOnGround
        {
            get { return _IsOnGround; }
            set { _IsOnGround = value; }
        }
        private bool _IsOnGround = false;

        public bool IsJumpingDown
        {
            get { return _IsJumpingDown; }
            set { _IsJumpingDown = value; }
        }
        private bool _IsJumpingDown = false;

        #region ICutlassMovable

        public Vector2 Velocity
        {
            get { return _Velocity; }
            set { _Velocity = value; }
        }
        private Vector2 _Velocity = Vector2.Zero;

        public float GravityCoefficient { get { return 1f; } }

        public float FrictionCoefficient { get { return 1f; } }

        #endregion ICutlassMovable

        #region ICutlassCollidable

        public bool Stationary
        {
            get { return false; }
        }

        public Vector2 PositionCorrection
        {
            get { return _PositionCorrection; }
        }
        private Vector2 _PositionCorrection;

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

        public event EventHandler<Vector2EventArgs> Moved;

        #endregion Events

        #region Initialization

        public Player(GameplayScreen parentScreen, Vector2 position)
        {
            _Player_TexId_Standing_R = TextureManager.AddTexture(new CutlassTexture("Content/Sprites/pirate-standing-48-120-R"));
            _Player_TexId_Standing_L = TextureManager.AddTexture(new CutlassTexture("Content/Sprites/pirate-standing-48-120-L"));
            _Player_TexId_Jumping_R = TextureManager.AddTexture(new CutlassTexture("Content/Sprites/pirate-jumping-48-120-R"));
            _Player_TexId_Jumping_L = TextureManager.AddTexture(new CutlassTexture("Content/Sprites/pirate-jumping-48-120-L"));
            _Player_TexId_Walking_R = TextureManager.AddTexture(new CutlassAnimatedTexture("Content/Sprites/pirate-walking-560-120-R", 10, 5));
            _Player_TexId_Walking_L = TextureManager.AddTexture(new CutlassAnimatedTexture("Content/Sprites/pirate-walking-560-120-L", 10, 5));
            _Player_TexId_Walking_Reverse_R = TextureManager.AddTexture(new CutlassAnimatedTexture("Content/Sprites/pirate-walking-reverse-560-120-R", 10, 5));
            _Player_TexId_Walking_Reverse_L = TextureManager.AddTexture(new CutlassAnimatedTexture("Content/Sprites/pirate-walking-reverse-560-120-L", 10, 5));
            _CurrentTexture = _Player_TexId_Walking_R;

            _ParentScreen = parentScreen;
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

        public void HandleInput(GameTime gameTime, Input input, Vector2 playerScreenPosition)
        {
            KeyboardState keyboardState = input.CurrentKeyboardState;
            MouseState mouseState = input.CurrentMouseState;
            GamePadState gamePadState = input.CurrentGamePadState;

            //Keyboard Input
            _IsJumpingDown = false;
            _WalkDirection = 0;

            if (keyboardState.IsKeyDown(GameSettingsManager.Default.LeftKey))
            {
                _WalkDirection = _WalkDirection - 1;
                _Velocity.X = Math.Max(_Velocity.X - (0.1f * (float)gameTime.ElapsedGameTime.TotalMilliseconds), -MAX_PLAYER_HORIZONTAL_SPEED);
            }

            if (keyboardState.IsKeyDown(GameSettingsManager.Default.RightKey))
            {
                _WalkDirection = _WalkDirection + 1;
                _Velocity.X = Math.Min(_Velocity.X + (0.1f * (float)gameTime.ElapsedGameTime.TotalMilliseconds), MAX_PLAYER_HORIZONTAL_SPEED);
            }

            if (keyboardState.IsKeyDown(GameSettingsManager.Default.JumpKey) && _IsOnGround)
            {
                _IsOnGround = false;

                if (keyboardState.IsKeyDown(GameSettingsManager.Default.DownKey))
                {
                    _IsJumpingDown = true;
                    _Velocity.Y += 1.0f;
                }
                else
                {
                    _Velocity.Y = _Velocity.Y - (6.0f);
                }
            }

            //Mouse Input
            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
            _LookDirection = mousePosition - (playerScreenPosition);// + new Vector2(Width / 2, Height / 2));
            _LookDirection.Normalize();

            //Looking Right
            if (_LookDirection.X >= 0)
            {
                //On the ground
                if (_WasOnGround)
                {
                    //Walking forward
                    if (_WalkDirection > 0)
                    {
                        _CurrentTexture = _Player_TexId_Walking_R;
                    }
                    //Walking backward
                    else if (_WalkDirection < 0)
                    {
                        _CurrentTexture = _Player_TexId_Walking_Reverse_R;
                    }
                    //Standing
                    else
                    {
                        _CurrentTexture = _Player_TexId_Standing_R;
                    }
                }
                //Jumping
                else
                {
                    _CurrentTexture = _Player_TexId_Jumping_R;
                }
            }
            //Looking Left
            else
            {
                //On the ground
                if (_WasOnGround)
                {
                    //Walking forward
                    if (_WalkDirection < 0)
                    {
                        _CurrentTexture = _Player_TexId_Walking_L;
                    }
                    //Walking backward
                    else if (_WalkDirection > 0)
                    {
                        _CurrentTexture = _Player_TexId_Walking_Reverse_L;
                    }
                    //Standing
                    else
                    {
                        _CurrentTexture = _Player_TexId_Standing_L;
                    }
                }
                //Jumping
                else
                {
                    _CurrentTexture = _Player_TexId_Jumping_L;
                }
            }
        }

        public virtual void BeforeMove(GameTime gameTime)
        {
            if (Velocity.Y == 0.0f &&
                _WasOnGround)
            {
                _IsOnGround = true;
            }
            else
            {
                _IsOnGround = false;
                //StandingOn.Clear();
            }
            //_Position += _PositionCorrection;
            //_PositionCorrection = Vector2.Zero;
        }

        public virtual void AfterMove(GameTime gameTime)
        {
            if (Moved != null)
                Moved(this, new Vector2EventArgs(Position));

            if (PlayerMoved != null)
                PlayerMoved(this, new BoundingRectangleEventArgs(CurrentFrameBoundingRect));
        }

        public void CollisionDetected(ICutlassCollidable collisionTarget)
        { }

        public void CollisionDetectedWithCorrection(ICutlassCollidable collisionTarget, Vector2 normal, float distance)
        {
            //Check for jumping through top-only collisions, or level-transition zones.
            if ((collisionTarget.Side == CollisionSide.Top && normal.Y == -1 && _IsJumpingDown) ||
                collisionTarget is LevelTransition)
                return;

            //get the separation and penetration separately, this is to stop penetration
            //from causing the obejcts to ping apart
            float separation = Math.Max(distance, 0.0f);
            float penetration = Math.Min(distance, 0.0f);

            //get relative normal velocity so object will stop exactly on surface.
            float relativeNormalVelocity = 0.0f;

            relativeNormalVelocity = VectorUtilities.DotProduct(Velocity, normal);

            if (relativeNormalVelocity < 0)
            {
                //remove normal velocity
                Velocity -= normal * relativeNormalVelocity;
            }

            //is this ground?
            if (normal.Y < 0.0f)
            {
                _WasOnGround = true;
                //StandingOn.Add(contact.B);
            }
        }

        #endregion

        #region Update and Draw

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            ICutlassTexture texture = (ICutlassTexture)TextureManager.GetTexture(_CurrentTexture);

            spriteBatch.Draw(texture.BaseTexture, CurrentFrameBoundingRect.Min, texture.AreaToRender, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            _WasOnGround = _IsOnGround;

            ICutlassUpdateable texture = TextureManager.GetTexture(_CurrentTexture) as ICutlassUpdateable;

            if (texture != null)
                texture.Update(gameTime);
        }

        #endregion Update and Draw
    }
}
