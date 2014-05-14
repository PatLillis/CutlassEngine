using BoundingRect;
using Cutlass.Utilities;
using System;

namespace Cutlass.Interfaces
{
    public interface ICutlassCollidable
    {
        BoundingRectangle BoundingRect { get; }

        CollisionCategory CategoryMask { get; set; }
        CollisionCategory Category { get; set; }
        CollisionSide Side { get; set; }

        void CollisionDetected(ICutlassCollidable collisionTarget, BoundingRectangle intersection);
    }
}
