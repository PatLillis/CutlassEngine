using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cutlass.Interfaces;
using Microsoft.Xna.Framework;
using BoundingRect;

namespace Cutlass.Assets
{
    public class CutlassSceneObject : ICutlassSceneObject
    {
        public bool PostUIDraw
        {
            get { return _PostUIDraw; }
            set { _PostUIDraw = value; }
        }
        private bool _PostUIDraw = false;

        public bool ReadyToRender
        {
            get { return _ReadyToRender; }
            set { _ReadyToRender = value; }
        }
        private bool _ReadyToRender = false;

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
        private Vector2 _Position = Vector2.Zero;

        public Vector2 Scale
        {
            get { return _Scale; }
            set { _Scale = value; }
        }
        private Vector2 _Scale = Vector2.One;

        public float Rotation
        {
            get { return _Rotation; }
            set { _Rotation = value; }
        }
        private float _Rotation = 0.0f;

        public void Draw(GameTime gameTime)
        {
            if (this is ICutlassDrawable)
            {
                //Draw object
            }
        }

        public void Update(GameTime gameTime)
        {
            if (this is ICutlassUpdateable)
            {
                //Update object
            }
        }
    }
}