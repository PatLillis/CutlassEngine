using System;
using Microsoft.Xna.Framework;
using BoundingRect;
using Cutlass.Interfaces;

namespace Cutlass.Utilities
{
    public class Vector2EventArgs : EventArgs
    {
        public Vector2 Vector;

        public Vector2EventArgs(Vector2 vector)
        {
            Vector = vector;
        }
    }

    public class BoundingRectangleEventArgs : EventArgs
    {
        public BoundingRectangle Rectangle;

        public BoundingRectangleEventArgs(BoundingRectangle rectangle)
        {
            Rectangle = rectangle;
        }
    }

    public class RectangleEventArgs : EventArgs
    {
        public Rectangle Rectangle;

        public RectangleEventArgs(Rectangle rectangle)
        {
            Rectangle = rectangle;
        }
    }
}
