using System;
using Microsoft.Xna.Framework;

namespace Cutlass.Interfaces
{
    public interface ICutlassMovable
    {
        Vector2 Position { get; set; }

        Vector2 Velocity { get; set; }

        float GravityCoefficient { get; }

        float FrictionCoefficient { get; }

        //void Move(GameTime gameTime);
    }
}
