using System;
using System.Collections.Generic;
using Cutlass.Interfaces;
using Microsoft.Xna.Framework;
using BoundingRect;
using Cutlass.Utilities;
using Cutlass.GameComponents;

namespace Cutlass.Managers
{
    public class CollisionManager
    {
        #region Fields

        private static readonly int DEFAULT_HORIZONTAL_GROUPS = 3;
        private static readonly int DEFAULT_VERTICAL_GROUPS = 3;

        private int _HorizontalGroups = DEFAULT_HORIZONTAL_GROUPS;
        private int _VerticalGroups = DEFAULT_VERTICAL_GROUPS;

        #endregion Fields

        #region Properties

        public GameScreen ViewScreen
        {
            get { return _ViewScreen; }
            set { _ViewScreen = value; }
        }
        private GameScreen _ViewScreen;

        /// <summary>
        /// Reset every frame
        /// </summary>
        List<ICutlassCollidable>[,] CurrentCollidableObjects
        {
            get { return _CurrentCollidableObjects; }
            set { _CurrentCollidableObjects = value; }
        }
        private List<ICutlassCollidable>[,] _CurrentCollidableObjects;

        #endregion Properties

        #region Initialization

        public CollisionManager(GameScreen viewScreen, int horizontalGroups = 0, int verticalGroups = 0)
        {
            _ViewScreen = viewScreen;

            if (horizontalGroups > 0)
                _HorizontalGroups = horizontalGroups;
            if (verticalGroups > 0)
                _VerticalGroups = verticalGroups;

            _CurrentCollidableObjects = new List<ICutlassCollidable>[_HorizontalGroups, _VerticalGroups];
        }

        #endregion Initialization

        #region Public Methods

        public void CheckCollisions(GameTime gameTime, IEnumerable<ICutlassCollidable> collidableObjects)
        {
            //Add objects to collision list
            foreach (ICutlassCollidable collidable in collidableObjects)
            {
                AddCollidableObject(collidable);
            }

            //Check for collisions
            for (int i = 0; i < _HorizontalGroups; i++)
                for (int j = 0; j < _VerticalGroups; j++)
                    CheckCollisionsInGroup(_CurrentCollidableObjects[i, j]);

            //Reset objects for next frame
            _CurrentCollidableObjects = new List<ICutlassCollidable>[_HorizontalGroups, _VerticalGroups];
        }

        #endregion Public Methods

        #region Private Methods

        private void AddCollidableObject(ICutlassCollidable collidableObject)
        {
            //Want to extend collisions slightly outside visible screen.
            BoundingRectangle collisionSpace = BoundingRectangle.Scale(_ViewScreen.VisibleArea, 1.1f, _ViewScreen.VisibleArea.Center);

            BoundingRectangle intersection = BoundingRectangle.Intersection(collidableObject.NextFrameBoundingRect, collisionSpace);

            int minHorizontalGroup = -1,
                  maxHorizontalGroup = -1,
                  minVerticalGroup = -1,
                  maxVerticalGroup = -1;

            if (!intersection.IsZero)
            {
                float groupWidth = collisionSpace.Width / _HorizontalGroups;
                float groupHeight = collisionSpace.Height / _VerticalGroups;

                minHorizontalGroup = Math.Max(0, (int)((intersection.Left - collisionSpace.Left) / groupWidth));
                maxHorizontalGroup = Math.Min(_HorizontalGroups - 1, (int)((intersection.Right - collisionSpace.Left) / groupWidth));
                minVerticalGroup = Math.Max(0, (int)((intersection.Top - collisionSpace.Top) / groupHeight));
                maxVerticalGroup = Math.Min(_VerticalGroups - 1, (int)((intersection.Bottom - collisionSpace.Top) / groupHeight));

                for (int i = minHorizontalGroup; i <= maxHorizontalGroup; i++)
                    for (int j = minVerticalGroup; j <= maxVerticalGroup; j++)
                    {
                        if (_CurrentCollidableObjects[i, j] == null)
                            _CurrentCollidableObjects[i, j] = new List<ICutlassCollidable>();
                        _CurrentCollidableObjects[i, j].Add(collidableObject);
                    }
            }
        }

        private void CheckCollisionsInGroup(List<ICutlassCollidable> collidableObjects)
        {
            if (collidableObjects == null || collidableObjects.Count <= 1)
                return;

            for (int i = 0; i < collidableObjects.Count; i++)
            {
                ICutlassCollidable first = collidableObjects[i];

                for (int j = i + 1; j < collidableObjects.Count; j++)
                {
                    ICutlassCollidable second = collidableObjects[j];

                    if ((first.Category & second.CategoryMask) != 0 &&
                        (second.Category & first.CategoryMask) != 0 &&
                        first.NextFrameBoundingRect.Intersects(second.NextFrameBoundingRect))
                    {
                        CollisionDetected(first, second, BoundingRectangle.Intersection(first.NextFrameBoundingRect, second.NextFrameBoundingRect));
                    }
                }
            }
        }

        private void CollisionDetected(ICutlassCollidable first, ICutlassCollidable second, BoundingRectangle intersection)
        {
            Vector2 directionToFirst = first.NextFrameBoundingRect.Center - intersection.Center;

            if (intersection.Height > intersection.Width)
                directionToFirst.Y = 0;
            else
                directionToFirst.X = 0;

            directionToFirst.Normalize();

            first.CollisionDetected(second, intersection, directionToFirst);
            second.CollisionDetected(first, intersection, -directionToFirst);
        }

        #endregion Private Methods
    }
}
