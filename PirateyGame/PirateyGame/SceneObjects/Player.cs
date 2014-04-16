﻿using BoundingRect;
using Cutlass.Assets;
using Cutlass.GameComponents;
using Cutlass.Interfaces;
using Cutlass.Managers;
using Cutlass.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace PirateyGame.SceneObjects
{
    public class Player : ICutlassDrawable, ICutlassUpdateable, ICutlassLoadable
    {
        #region Properties

        public string PlayerName
        {
            get { return _PlayerName; }
        }
        private string _PlayerName = String.Empty;

        private string _PlayerFontKey = String.Empty;

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
            set { _ReadyToRender = value; }
        }
        private bool _ReadyToRender = false;

        public bool IsVisible
        {
            get { return _IsVisible; }
            set { _IsVisible = value; }
        }
        private bool _IsVisible = false;

        public float Width
        {
            get { return FontManager.GetSpriteFontOrDefault(_PlayerFontKey).MeasureString(_PlayerName).X; }
        }

        public float Height
        {
            get { return FontManager.GetSpriteFontOrDefault(_PlayerFontKey).MeasureString(_PlayerName).Y; }
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

        public bool IsLoaded
        {
            get { return _IsLoaded; }
            set { _IsLoaded = value; }
        }
        private bool _IsLoaded = false;

        #endregion Properties

        #region Events

        public event EventHandler<BoundingRectangleEventArgs> PlayerMoved;

        #endregion Events

        #region Initialization

        public Player(string playerName, string fontKey = "")
        {
            _PlayerName = playerName +"(|)";
            _PlayerFontKey = fontKey;

            Position = new Vector2(100, 100);

            IsVisible = true;
        }

        public void LoadContent()
        {
            _IsLoaded = true;
        }

        public void UnloadContent()
        {
            //Unload Content
        }

        #endregion Initialization

        #region Public Methods

        public void HandleInput(Input input)
        {
            KeyboardState keyboardState = input.CurrentKeyboardState;
            GamePadState gamePadState = input.CurrentGamePadState;

            // Otherwise move the player position.
            Vector2 movement = Vector2.Zero;

            if (keyboardState.IsKeyDown(Keys.Left))
                movement.X--;

            if (keyboardState.IsKeyDown(Keys.Right))
                movement.X++;

            if (keyboardState.IsKeyDown(Keys.Up))
                movement.Y--;

            if (keyboardState.IsKeyDown(Keys.Down))
                movement.Y++;

            Vector2 thumbstick = gamePadState.ThumbSticks.Left;

            movement.X += thumbstick.X;
            movement.Y -= thumbstick.Y;

            if (movement.Length() > 1)
                movement.Normalize();

            _Position += movement;

            OnPlayerMoved();
        }

        protected internal void OnPlayerMoved()
        {
            if (PlayerMoved != null)
                PlayerMoved(this, new BoundingRectangleEventArgs(BoundingRect));
        }

        #endregion

        #region Update and Draw

        public void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.DrawString(FontManager.GetSpriteFontOrDefault(_PlayerFontKey), _PlayerName, Position, Palette.MediumBlue);
        }

        public void Update(GameTime gameTime)
        {
            string statusChar = _PlayerName.Substring(_PlayerName.Length - 2, 1);
            string statusGroup = String.Empty;

            switch(statusChar)
            {
                case @"\":
                    statusGroup = "(|)";
                    break;
                case @"|":
                    statusGroup = "(/)";
                    break;
                case @"/":
                    statusGroup = "(-)";
                    break;
                case @"-":
                    statusGroup = @"(\)";
                    break;
                default:
                    break;
            }

            _PlayerName = _PlayerName.Substring(0, _PlayerName.Length - 3) + statusGroup;
        }

        #endregion Update and Draw
    }
}
