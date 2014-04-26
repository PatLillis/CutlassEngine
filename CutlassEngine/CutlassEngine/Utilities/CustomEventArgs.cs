using System;
using Microsoft.Xna.Framework;
using BoundingRect;

namespace Cutlass.Utilities
{
    public class PositionEventArgs : EventArgs
    {
        public Vector2 Position;

        public PositionEventArgs(Vector2 position)
        {
            Position = position;
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

    public class RectangleEventArgs: EventArgs
    {
        public Rectangle Rectangle;

        public RectangleEventArgs(Rectangle rectangle)
        {
            Rectangle = rectangle;
        }
    }
}
