using BoundingRect;
using Cutlass.Utilities;
using System;

namespace Cutlass.Interfaces
{
    public interface ICutlassCollidable
    {
        BoundingRectangle BoundingRect { get; }

        CollisionCategory CategoryMask { get; }
        CollisionCategory Category { get; }
        CollisionSide Side { get; }

        void CollisionDetected(ICutlassCollidable collisionTarget, BoundingRectangle intersection);
    }
}
