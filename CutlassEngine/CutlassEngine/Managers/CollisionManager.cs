using System;
using System.Collections.Generic;
using Cutlass.Interfaces;
using Microsoft.Xna.Framework;
using BoundingRect;
using Cutlass.Utilities;


namespace Cutlass.Managers
{
    public class CollisionManager
    {
        #region Fields

        public const int BUCKETS_PER_SIDE = 3;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Reset every frame
        /// </summary>
        public Dictionary<int, List<ICutlassCollidable>> CurrentCollidableObjects
        {
            get { return _CurrentCollidableObjects; }
        }
        private Dictionary<int, List<ICutlassCollidable>> _CurrentCollidableObjects;

        #endregion Properties

        #region Public Methods

        public void AddCollidableObject(ICutlassCollidable collidableObject)
        {

        }

        public void CheckCollisions(GameTime gameTime)
        {

        }

        #endregion Public Methods

        #region Private Methods

        private void CollisionDetected(ICutlassCollidable first, ICutlassCollidable second, BoundingRectangle intersection)
        {
            first.CollisionDetected(second, intersection);
            second.CollisionDetected(first, intersection);
        }

        #endregion Private Methods
    }
}
