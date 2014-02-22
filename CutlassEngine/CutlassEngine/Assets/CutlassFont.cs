﻿using System;
using Cutlass.Interfaces;
using Microsoft.Xna.Framework.Graphics;

namespace Cutlass.Assets
{
    /// <summary>
    /// "Default" implementation of ICutlassFont interface.
    /// </summary>
    public class CutlassFont : ICutlassFont
    {
        #region Properties

        /// <summary>The file name of the asset.</summary>
        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }
        private string _FileName;

        ///<summary>Gets the underlying font.</summary>
        public SpriteFont Font
        {
            get { return _Font; }
        }
        private SpriteFont _Font;

        ///<summary>Is the font ready to be rendered.</summary>
        public bool ReadyToRender
        {
            get { return _ReadyToRender; }
        }
        private bool _ReadyToRender = false;

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
            _FileName = fileName;
        }

        /// <summary>
        /// Load all font assets.
        /// </summary>
        public void LoadContent()
        {
            if (!String.IsNullOrEmpty(_FileName))
            {
                _Font = CutlassEngine.ContentManager.Load<SpriteFont>(_FileName);
                _ReadyToRender = true;
            }
        }

        /// <summary>
        /// Unload font assets.
        /// </summary>
        public void UnloadContent()
        { }

        #endregion Initialization
    }
}