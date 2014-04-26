using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cutlass.Interfaces;

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

        public List<ICutlassSceneObject> Objects
        {
            get { return _Objects; }
            set { _Objects = value; }
        }
        private List<ICutlassSceneObject> _Objects = new List<ICutlassSceneObject>();

        #endregion Properties

        #region Initialization

        public SceneObjectManager() { }

        public SceneObjectManager(IEnumerable<ICutlassSceneObject> objects)
        {
            foreach(ICutlassSceneObject o in objects)
            {
                AddObject(o);
            }
        }

        /// <summary>
        /// Load graphics content.
        /// </summary>
        public void LoadContent()
        {
            foreach (ICutlassSceneObject o in Objects)
            {
                if (o is ICutlassLoadable)
                {
                    ICutlassLoadable oLoadable = o as ICutlassLoadable;
                    oLoadable.LoadContent();
                }
            }

            _Initialized = true;
        }

        /// <summary>
        /// Unload graphics content.
        /// </summary>
        public void UnloadContent()
        {
            foreach (ICutlassSceneObject o in Objects)
            {
                ICutlassLoadable oLoadable = o as ICutlassLoadable;

                if (oLoadable != null)
                    oLoadable.UnloadContent();
            }
        }

        #endregion Initialization

        #region Update and Draw

        public void Update(GameTime gameTime)
        {
            foreach (ICutlassSceneObject o in Objects)
            {
                ICutlassUpdateable oUpdateable = o as ICutlassUpdateable;
                if (oUpdateable != null)
                    oUpdateable.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, Matrix offsetTransform)
        {
            List<ICutlassDrawable> objectsToDraw = new List<ICutlassDrawable>();

            foreach (ICutlassSceneObject o in Objects)
            {
                ICutlassDrawable oDrawable = o as ICutlassDrawable;
                if (oDrawable != null && oDrawable.IsVisible)
                {
                    objectsToDraw.Add(oDrawable);
                }
            }

            objectsToDraw.Sort((x, y) => x.DrawOrder.CompareTo(y.DrawOrder));

            foreach (ICutlassDrawable oDrawable in objectsToDraw)
            {
                //If object should move wrt player, pass in screen's offsetTransform.
                ScreenManager.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, oDrawable.ScreenPositionFixed ? Matrix.Identity : offsetTransform);

                oDrawable.Draw(gameTime, ScreenManager.SpriteBatch);

                ScreenManager.SpriteBatch.End();
            }
        }

        #endregion Update and Draw

        #region Public Methods

        public void AddObject(ICutlassSceneObject o)
        {
            Objects.Add(o);

            if (_Initialized)
            {
                ICutlassLoadable oLoadable = o as ICutlassLoadable;
                if (oLoadable != null)
                {
                    oLoadable.LoadContent();
                }
            }
        }

        public void AddObjects(ICutlassSceneObject o, params ICutlassSceneObject[] list)
        {
            AddObject(o);

            for (int i = 0; i < list.Length; i++)
            {
                AddObject(list[i]);
            }
        }

        #endregion Public Methods
    }
}
