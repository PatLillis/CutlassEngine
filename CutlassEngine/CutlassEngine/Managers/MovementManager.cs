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

        public MovementManager(float gravity = 0.015f, float friction = 0.015f)
        {
            Gravity = gravity;
            Friction = friction;
        }

        public void ApplyGravity(GameTime gameTime, IEnumerable<ICutlassMovable> objectsToMove)
        {
            float timeSteppedGravity = Gravity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            foreach(ICutlassMovable movableObject in objectsToMove)
            {
                movableObject.Velocity = new Vector2(movableObject.Velocity.X,
                    movableObject.Velocity.Y + (timeSteppedGravity * movableObject.GravityCoefficient));
            }
        }

        public void ApplyFriction(GameTime gameTime, IEnumerable<ICutlassMovable> objectsToMove)
        {
            float timeSteppedFriction = Friction * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            foreach (ICutlassMovable movableObject in objectsToMove)
            {
                movableObject.Velocity = new Vector2(
                    movableObject.Velocity.X * (1 - (2 * timeSteppedFriction * movableObject.FrictionCoefficient)),
                    movableObject.Velocity.Y * (1 - (timeSteppedFriction * movableObject.FrictionCoefficient)));
            }
        }

        public void ApplyMovement(GameTime gameTime, IEnumerable<ICutlassMovable> objectsToMove)
        {
            foreach (ICutlassMovable movableObject in objectsToMove)
            {
                movableObject.BeforeMove(gameTime);
                movableObject.Position += movableObject.Velocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                movableObject.AfterMove(gameTime);
            }
        }
    }
}
