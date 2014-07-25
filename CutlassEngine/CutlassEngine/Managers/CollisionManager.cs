using System;
using System.Collections.Generic;
using Cutlass.Interfaces;
using Microsoft.Xna.Framework;
using BoundingRect;
using Cutlass.Utilities;
using Cutlass.GameComponents;

namespace Cutlass.Managers
{
    /// <summary>
    /// Collision Manager for a Screen.
    /// Uses collision based on Speculative Contacts, as outlined in:
    /// http://www.wildbunny.co.uk/blog/2011/12/14/how-to-make-a-2d-platform-game-part-2-collision-detection/
    /// </summary>
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
                    CheckCollisionsInGroup(gameTime, _CurrentCollidableObjects[i, j]);

            //Reset objects for next frame
            _CurrentCollidableObjects = new List<ICutlassCollidable>[_HorizontalGroups, _VerticalGroups];
        }

        #endregion Public Methods

        #region Private Methods

        private void AddCollidableObject(ICutlassCollidable collidableObject)
        {
            //Want to extend collisions slightly outside visible screen.
            BoundingRectangle collisionSpace = BoundingRectangle.Scale(_ViewScreen.VisibleArea, 1.1f, _ViewScreen.VisibleArea.Center);

            BoundingRectangle intersection = BoundingRectangle.Intersection(collidableObject.CurrentFrameBoundingRect, collisionSpace);

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

        private void CheckCollisionsInGroup(GameTime gameTime, List<ICutlassCollidable> collidableObjects)
        {
            if (collidableObjects == null || collidableObjects.Count <= 1)
                return;

            for (int i = 0; i < collidableObjects.Count; i++)
            {
                ICutlassCollidable first = collidableObjects[i];

                for (int j = i + 1; j < collidableObjects.Count; j++)
                {
                    ICutlassCollidable second = collidableObjects[j];

                    CollisionContact collisionContact = new CollisionContact() { A = first, B = second };

                    if (CalculateCollision(gameTime, ref collisionContact) && !IsInternalEdge(collisionContact))
                    {
                        first.CollisionDetected(collisionContact);
                        second.CollisionDetected(new CollisionContact(collisionContact.B, collisionContact.A, -collisionContact.Normal, collisionContact.Distance));
                    }
                }
            }
        }

        private bool CalculateCollision(GameTime gameTime, ref CollisionContact contact)
        {
            ICutlassCollidable first = contact.A;
            ICutlassCollidable second = contact.B;

            //No collision if their catgories don't overlap.
            if ((first.Category & second.CategoryMask) == 0 ||
                (second.Category & first.CategoryMask) == 0)
                return false;

            Vector2 halfExtents = new Vector2(second.CurrentFrameBoundingRect.Width / 2, second.CurrentFrameBoundingRect.Height / 2);
            BoundingRectangle firstPlusHalfExtents = new BoundingRectangle(first.CurrentFrameBoundingRect.Left - halfExtents.X,
                                                                    first.CurrentFrameBoundingRect.Top - halfExtents.Y,
                                                                    first.CurrentFrameBoundingRect.Width + 2 * halfExtents.X,
                                                                    first.CurrentFrameBoundingRect.Height + 2 * halfExtents.Y);

            //Get closest point
            Vector2 closestPoint = second.CurrentFrameBoundingRect.Center;

            //X Axis
            float x = second.CurrentFrameBoundingRect.Center.X;
            if (x < firstPlusHalfExtents.Left) x = firstPlusHalfExtents.Left;
            if (x > firstPlusHalfExtents.Right) x = firstPlusHalfExtents.Right;
            closestPoint.X = x;

            //Y Axis
            float y = second.CurrentFrameBoundingRect.Center.Y;
            if (y < firstPlusHalfExtents.Top) y = firstPlusHalfExtents.Top;
            if (y > firstPlusHalfExtents.Bottom) y = firstPlusHalfExtents.Bottom;
            closestPoint.Y = y;

            //Check if second's center is inside fisrtPlusHalfExtents. If so, find closest edge for negative distance.
            if (closestPoint == second.CurrentFrameBoundingRect.Center)
            {
                Vector2 distanceToClosestEdge;
                //X Axis
                if (Math.Abs(closestPoint.X - firstPlusHalfExtents.Right) > Math.Abs(closestPoint.X - firstPlusHalfExtents.Left))
                    distanceToClosestEdge.X = closestPoint.X - firstPlusHalfExtents.Left;
                else
                    distanceToClosestEdge.X = closestPoint.X - firstPlusHalfExtents.Right;

                //Y Axis
                if (Math.Abs(closestPoint.Y - firstPlusHalfExtents.Bottom) > Math.Abs(closestPoint.Y - firstPlusHalfExtents.Top))
                    distanceToClosestEdge.Y = closestPoint.Y - firstPlusHalfExtents.Top;
                else
                    distanceToClosestEdge.Y = closestPoint.Y - firstPlusHalfExtents.Bottom;

                //Minimum
                Vector2 axisMinor = VectorUtilities.MinorAxis(distanceToClosestEdge);
                closestPoint = closestPoint + axisMinor;
            }

            //Calculate distance and Normal
            Vector2 distance = closestPoint - second.CurrentFrameBoundingRect.Center;

            contact.Distance = VectorUtilities.MaximumComponent(distance);

            if (contact.Distance == 0.0f)//right up against each other, will get invalid normal.
            {
                //On Top
                if (first.CurrentFrameBoundingRect.Bottom == second.CurrentFrameBoundingRect.Top)
                {
                    contact.Normal = new Vector2(0.0f, -1.0f);
                    if ((first.Side & CollisionSide.Bottom) == 0 ||
                        (second.Side & CollisionSide.Top) == 0)
                        return false;
                }
                //To the Right
                else if (first.CurrentFrameBoundingRect.Left == second.CurrentFrameBoundingRect.Right)
                {
                    contact.Normal = new Vector2(1.0f, 0.0f);
                    if ((first.Side & CollisionSide.Left) == 0 ||
                        (second.Side & CollisionSide.Right) == 0)
                        return false;
                }
                //On Bottom
                else if (first.CurrentFrameBoundingRect.Top == second.CurrentFrameBoundingRect.Bottom)
                {
                    contact.Normal = new Vector2(0.0f, 1.0f);
                    if ((first.Side & CollisionSide.Top) == 0 ||
                        (second.Side & CollisionSide.Bottom) == 0)
                        return false;
                }
                //To the Left
                else if (first.CurrentFrameBoundingRect.Right == second.CurrentFrameBoundingRect.Left)
                {
                    contact.Normal = new Vector2(-1.0f, 0.0f);
                    if ((first.Side & CollisionSide.Right) == 0 ||
                        (second.Side & CollisionSide.Left) == 0)
                        return false;
                }
            }
            else
            {
                contact.Normal = VectorUtilities.NormalizedMajorAxis(distance);
            }

            //On top
            if (contact.Normal.Y == -1.0f &&
                ((first.Side & CollisionSide.Bottom) == 0 ||
                 (second.Side & CollisionSide.Top) == 0))
            {
                return false;
            }
            //To the Right
            else if (contact.Normal.X == 1.0f &&
                ((first.Side & CollisionSide.Left) == 0 ||
                 (second.Side & CollisionSide.Right) == 0))
            {
                return false;
            }
            //On Bottom
            else if (contact.Normal.Y == 1.0f &&
                ((first.Side & CollisionSide.Top) == 0 ||
                 (second.Side & CollisionSide.Bottom) == 0))
            {
                return false;
            }
            //To the Left
            else if (contact.Normal.X == -1.0f &&
                ((first.Side & CollisionSide.Right) == 0 ||
                        (second.Side & CollisionSide.Left) == 0))
            {
                return false;
            }

            Boolean returnValue = false;

            //Collision Direction is X
            if (Math.Abs(contact.Normal.X) >= Math.Abs(contact.Normal.Y) &&
                (((first.Velocity.X * gameTime.ElapsedGameTime.TotalMilliseconds) + contact.Distance) * contact.Normal.X < 0))
                returnValue = true;

            //Collision Direction is Y
            if (Math.Abs(contact.Normal.X) <= Math.Abs(contact.Normal.Y) &&
                (((first.Velocity.Y * gameTime.ElapsedGameTime.TotalMilliseconds) + contact.Distance) * contact.Normal.Y < 0))
                returnValue = true;

            return returnValue;
        }

        private bool IsInternalEdge(CollisionContact contact)
        {
            return false;
        }

        #endregion Private Methods
    }
}