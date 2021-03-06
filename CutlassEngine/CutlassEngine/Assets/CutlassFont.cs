﻿using System;
using Microsoft.Xna.Framework.Graphics;
using Cutlass.Interfaces;
using Cutlass.Utilities;

namespace Cutlass.Assets
{
    /// <summary>
    /// "Default" implementation of ICutlassFont interface.
    /// </summary>
    public class CutlassFont : ICutlassFont
    {
        #region Properties

        /// <summary>ID</summary>
        public SceneObjectId SceneObjectId { get; set; }

        /// <summary>Once this is set to false, will be removed from Scene.</summary>
        public bool Active
        {
            get { return _Active; }
            set { _Active = value; }
        }
        protected bool _Active;

        /// <summary>The file name of the asset.</summary>
        public string Filename
        {
            get { return _Filename; }
            set { _Filename = value; }
        }
        protected string _Filename;

        ///<summary>Gets the underlying font.</summary>
        public SpriteFont Font
        {
            get { return _Font; }
        }
        protected SpriteFont _Font;

        ///<summary>Is the font ready to be rendered.</summary>
        public bool IsLoaded
        {
            get { return _IsLoaded; }
        }
        protected bool _IsLoaded = false;

        #endregion Properties

        #region Initialization

        /// <summary>
        /// Construct a new CutlassFont.
        /// </summary>
        public CutlassFont()
        { }

        /// <summary>
        /// Construct a new CutlassFont.
        /// </summary>
        /// <param name="fileName">The asset file name.</param>
        public CutlassFont(string fileName)
        {
            _Filename = fileName;
        }

        /// <summary>
        /// Load all font assets.
        /// </summary>
        public void LoadContent()
        {
            if (!String.IsNullOrEmpty(_Filename))
            {
                _Font = CutlassEngine.ContentManager.Load<SpriteFont>(_Filename);
                _IsLoaded = true;
            }
        }

        /// <summary>
        /// Unload font assets.
        /// </summary>
        public void UnloadContent()
        {
            _IsLoaded = false;
        }

        #endregion Initialization
    }
}