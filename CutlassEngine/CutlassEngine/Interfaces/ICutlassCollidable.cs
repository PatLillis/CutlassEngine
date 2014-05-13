using BoundingRect;
using Cutlass.Utilities;
using System;

namespace Cutlass.Interfaces
{
    [Flags]
    public enum CollisionCategory
    {
        None = 0x0,
        GoodGuy = 0x1,
        BadGuy = 0x2
    }

    public interface ICutlassCollidable
    {
        BoundingRectangle BoundingRect { get; }

        CollisionCategory CategoryMask { get; set; }
        CollisionCategory Category { get; set; }

        event EventHandler<CollisionEventArgs> CollisionDetected;
    }
}
