using System;
using BoundingRect;
using Cutlass.Utilities;
using Microsoft.Xna.Framework;

namespace Cutlass.Interfaces
{
    public interface ICutlassCollidable
    {
        BoundingRectangle CurrentFrameBoundingRect { get; }
        BoundingRectangle NextFrameBoundingRect { get; }

        bool Stationary { get; }

        Vector2 Position { get; }
        Vector2 Velocity { get; }
        Vector2 PositionCorrection { get; }

        CollisionCategory CategoryMask { get; }
        CollisionCategory Category { get; }
        CollisionSide Side { get; }

        void CollisionDetected(ICutlassCollidable collisionTarget);
        void CollisionDetectedWithCorrection(ICutlassCollidable collisionTarget, Vector2 normal, float distance);
    }
}
