using System;
using Microsoft.Xna.Framework;
using Cutlass.Utilities;

namespace Cutlass.Interfaces
{
    public interface ICutlassMovable
    {
        Vector2 Position { get; set; }

        Vector2 Velocity { get; set; }

        float GravityCoefficient { get; }

        float FrictionCoefficient { get; }

        //Throws event with object's new position 
        event EventHandler<Vector2EventArgs> Moved;

        void BeforeMove(GameTime gameTime);

        void AfterMove(GameTime gameTime);
    }
}
