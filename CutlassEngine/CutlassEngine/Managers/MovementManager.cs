using System;
using System.Collections.Generic;
using Cutlass.Interfaces;
using Microsoft.Xna.Framework;
using Cutlass.Utilities;

namespace Cutlass.Managers
{
    public class MovementManager
    {
        public float Gravity;
        public float Friction;

        public MovementManager(float gravity = 0.05f, float friction = 0.05f)
        {
            Gravity = gravity;
            Friction = friction;
        }

        public void ApplyGravity(GameTime gameTime, IEnumerable<ICutlassMovable> objectsToMove)
        {
            foreach(ICutlassMovable movableObject in objectsToMove)
            {
                movableObject.Velocity = new Vector2(movableObject.Velocity.X,
                    movableObject.Velocity.Y + Gravity * gameTime.ElapsedGameTime.Milliseconds);
            }
        }

        public void ApplyFriction(GameTime gameTime, List<ICutlassMovable> objectsToMove)
        {
            foreach (ICutlassMovable movableObject in objectsToMove)
            {
                movableObject.Velocity = new Vector2(
                    movableObject.Velocity.Y * (1 - (Friction * gameTime.ElapsedGameTime.Milliseconds)),
                    movableObject.Velocity.X * (1 - (2 * Friction * gameTime.ElapsedGameTime.Milliseconds)));
            }
        }

        public void ApplyMovement(IEnumerable<ICutlassMovable> objectsToMove)
        {
            foreach (ICutlassMovable movableObject in objectsToMove)
            {
                movableObject.Position += movableObject.Velocity;
                movableObject.OnMoved();
            }
        }
    }
}
