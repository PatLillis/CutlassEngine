using BoundingRect;
using Cutlass.Assets;
using Cutlass.Interfaces;
using Cutlass.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public BoundingRectangle BoundingRect
        {
            get { return _BoundingRect; }
            set { _BoundingRect = value; }
        }
        private BoundingRectangle _BoundingRect;

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

        #region Update and Draw

        public void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            spriteBatch.DrawString(FontManager.GetSpriteFontOrDefault(_PlayerFontKey), _PlayerName, Position, Palette.MediumBlue);
            spriteBatch.End();
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
