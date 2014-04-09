using Cutlass.Assets;
using Cutlass.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                    oLoadable.IsLoaded = true;
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

        public void Draw(GameTime gameTime)
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
                oDrawable.Draw(gameTime);
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
                    oLoadable.IsLoaded = true;
                }
            }
        }

        #endregion Public Methods
    }
}
