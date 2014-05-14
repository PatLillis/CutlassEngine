using BoundingRect;
using Cutlass.Utilities;
using System;

namespace Cutlass.Interfaces
{
    [Flags]
    public enum CollisionCategory
    {
        None = 0x0,
        Good = 0x1,
        Bad = 0x2,
        AttackHigh = 0x4,
        AttackMid = 0x8,
        AttackLow = 0x16,
        Scenery = 0x32
    }

    [Flags]
    public enum CollisionSide
    {
        None = 0x0,
        Top = 0x1,
        Right = 0x2,
        Bottom = 0x4,
        Left = 0x8,
        All = Top | Right | Bottom | Left
    }

    public interface ICutlassCollidable
    {
        BoundingRectangle BoundingRect { get; }

        CollisionCategory CategoryMask { get; set; }
        CollisionCategory Category { get; set; }
        CollisionSide Side { get; set; }

        void CollisionDetected(ICutlassCollidable collisionTarget, BoundingRectangle intersection);
    }
}
