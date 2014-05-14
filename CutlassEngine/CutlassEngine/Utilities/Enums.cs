using System;

namespace Cutlass.Utilities
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

    /// <summary>
    /// Describes the screen transition state.
    /// </summary>
    public enum ScreenState
    {
        /// <summary>Transition On</summary>
        TransitionOn,
        /// <summary>Active</summary>
        Active,
        /// <summary>Transition Off</summary>
        TransitionOff,
        /// <summary>Hidden</summary>
        Hidden,
    }
}