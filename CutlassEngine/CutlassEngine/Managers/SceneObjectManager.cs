using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cutlass.Interfaces;
using Cutlass.Utilities;

namespace Cutlass.Managers
{
    public class SceneObjectManager
    {
        #region Properties

        /// <summary>Is the TextureManagers Initialized, used for test cases and setup of Effects.</summary>
        public bool Initialized
        {
            get { return _Initialized; }
        }
        private bool _Initialized = false;

        #region Object Dictionaries

        public Dictionary<SceneObjectId, ICutlassSceneObject> Objects
        {
            get { return _Objects; }
            set { _Objects = value; }
        }
        private Dictionary<SceneObjectId, ICutlassSceneObject> _Objects = new Dictionary<SceneObjectId, ICutlassSceneObject>();

        public Dictionary<SceneObjectId, ICutlassLoadable> LoadableObjects
        {
            get { return _LoadableObjects; }
            set { _LoadableObjects = value; }
        }
        private Dictionary<SceneObjectId, ICutlassLoadable> _LoadableObjects = new Dictionary<SceneObjectId, ICutlassLoadable>();

        public Dictionary<SceneObjectId, ICutlassMovable> MovableObjects
        {
            get { return _MovableObjects; }
            set { _MovableObjects = value; }
        }
        private Dictionary<SceneObjectId, ICutlassMovable> _MovableObjects = new Dictionary<SceneObjectId, ICutlassMovable>();

        public Dictionary<SceneObjectId, ICutlassCollidable> CollidableObjects
        {
            get { return _CollidableObjects; }
            set { _CollidableObjects = value; }
        }
        private Dictionary<SceneObjectId, ICutlassCollidable> _CollidableObjects = new Dictionary<SceneObjectId, ICutlassCollidable>();

        public Dictionary<SceneObjectId, ICutlassUpdateable> UpdateableObjects
        {
            get { return _UpdateableObjects; }
            set { _UpdateableObjects = value; }
        }
        private Dictionary<SceneObjectId, ICutlassUpdateable> _UpdateableObjects = new Dictionary<SceneObjectId, ICutlassUpdateable>();

        public Dictionary<SceneObjectId, ICutlassDrawable> DrawableObjects
        {
            get { return _DrawableObjects; }
            set { _DrawableObjects = value; }
        }
        private Dictionary<SceneObjectId, ICutlassDrawable> _DrawableObjects = new Dictionary<SceneObjectId, ICutlassDrawable>();

        #endregion

        private int _NextSceneObjectId = 0;
        private Object _SceneObjectIdLock = new Object();

        private CollisionManager _CollisionManager;
        private MovementManager _MovementManager;

        #endregion Properties

        #region Initialization

        public SceneObjectManager(CollisionManager collisionManager, MovementManager movementManager)
        {
            _CollisionManager = collisionManager;
            _MovementManager = movementManager;
        }

        public SceneObjectManager(IEnumerable<ICutlassSceneObject> objects, CollisionManager collisionManager, MovementManager movementManager)
            : this(collisionManager, movementManager)
        {
            foreach(ICutlassSceneObject o in objects)
            {
                if (o != null)
                {
                    AddObject(o);
                }
            }
        }

        /// <summary>
        /// Load graphics content.
        /// </summary>
        public void LoadContent()
        {
            foreach (ICutlassLoadable loadable in LoadableObjects.Values)
            {
                if (loadable != null)
                {
                    loadable.LoadContent();
                }
            }

            _Initialized = true;
        }

        /// <summary>
        /// Unload graphics content.
        /// </summary>
        public void UnloadContent()
        {
            foreach (ICutlassLoadable loadable in LoadableObjects.Values)
            {
                if (loadable != null)
                {
                    loadable.UnloadContent();
                }
            }
        }

        #endregion Initialization

        #region Update and Draw

        public void Update(GameTime gameTime)
        {
            RemoveInactiveObjects();

            //Update objects
            foreach (ICutlassUpdateable updateable in UpdateableObjects.Values)
            {
                updateable.Update(gameTime);
            }

            //Add objects to CollisionManager
            foreach (ICutlassCollidable collidable in CollidableObjects.Values)
            {
                _CollisionManager.AddCollidableObject(collidable);
            }

            //Check Collisions
            _CollisionManager.CheckCollisions(gameTime);

            //Apply Movement
            //_MovementManager.ApplyGravity(gameTime, MovableObjects.Values);
            //_MovementManager.ApplyFriction(gameTime, objectsToMove);
            _MovementManager.ApplyMovement(MovableObjects.Values);
        }

        public void Draw(GameTime gameTime, Matrix offsetTransform)
        {
            RemoveInactiveObjects();

            foreach (ICutlassDrawable drawable in DrawableObjects.Values.OrderBy(pair => pair.DrawOrder))
            {
                if (drawable.IsVisible)
                {
                    //If object should move wrt player, pass in screen's offsetTransform.
                    ScreenManager.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, drawable.ScreenPositionFixed ? Matrix.Identity : offsetTransform);

                    drawable.Draw(gameTime, ScreenManager.SpriteBatch);

                    ScreenManager.SpriteBatch.End();
                }
            }
        }

        #endregion Update and Draw

        #region Public Methods

        public void AddObject(ICutlassSceneObject o)
        {
            SceneObjectId sceneObjectId;

            lock (_SceneObjectIdLock)
            {
                sceneObjectId = _NextSceneObjectId++;
            }

            Objects.Add(sceneObjectId, o);

            //Add object to necessary lists.
            ICutlassLoadable loadable = o as ICutlassLoadable;
            if (loadable != null)
                LoadableObjects.Add(sceneObjectId, loadable);

            ICutlassMovable movable = o as ICutlassMovable;
            if (movable != null)
                MovableObjects.Add(sceneObjectId, movable);

            ICutlassCollidable collidable = o as ICutlassCollidable;
            if (collidable != null)
                CollidableObjects.Add(sceneObjectId, collidable);

            ICutlassUpdateable updateable = o as ICutlassUpdateable;
            if (updateable != null)
                UpdateableObjects.Add(sceneObjectId, updateable);

            ICutlassDrawable drawable = o as ICutlassDrawable;
            if (drawable != null)
                DrawableObjects.Add(sceneObjectId, drawable);

            //If this scene has already been initialized, initialize this object now.
            if (_Initialized && loadable != null)
            {
                loadable.LoadContent();
            }
        }

        public void AddObjects(params ICutlassSceneObject[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                AddObject(list[i]);
            }
        }

        public void RemoveObject(SceneObjectId id)
        {
            Objects.Remove(id);
            LoadableObjects.Remove(id);
            MovableObjects.Remove(id);
            CollidableObjects.Remove(id);
            UpdateableObjects.Remove(id);
            DrawableObjects.Remove(id);
        }

        public void RemoveObjects(params SceneObjectId[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                RemoveObject(list[i]);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void RemoveInactiveObjects()
        {
            List<SceneObjectId> objectsToRemove = new List<SceneObjectId>();

            //Remove outdated objects
            foreach (ICutlassSceneObject o in Objects.Values)
            {
                if (!o.Active)
                {
                    objectsToRemove.Add(o.SceneObjectId);
                }
            }

            RemoveObjects(objectsToRemove.ToArray());
        }

        #endregion
    }
}
