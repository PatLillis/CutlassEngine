using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cutlass.Interfaces;

namespace Cutlass.Assets
{
    /// <summary>
    /// "Default" implementation of ICutlassTexture interface.
    /// </summary>
    public class CutlassTexture : ICutlassTexture
    {
        #region Properties

        /// <summary>Once this is set to false, will be removed from Scene.</summary>
        public bool Active
        {
            get { return _Active; }
            set { _Active = value; }
        }
        protected bool _Active;

        /// <summary>The file name of the asset.</summary>
        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }
        protected string _FileName;

        ///<summary>Gets the underlying texture.</summary>
        public Texture2D BaseTexture
        {
            get { return _BaseTexture; }
        }
        protected Texture2D _BaseTexture;

        ///<summary>Is the texture ready to be rendered.</summary>
        public bool ReadyToRender
        {
            get { return _ReadyToRender; }
        }
        protected bool _ReadyToRender = false;

        /// <summary>Width of texture, or -1 if no texture</summary>
        public int Width
        {
            get
            {
                return AreaToRender.Width;
            }
        }

        /// <summary>Height of texture, or -1 if no texture</summary>
        public int Height
        {
            get
            {
                return AreaToRender.Height;
            }
        }

        public virtual Rectangle AreaToRender
        {
            get
            {
                if (_BaseTexture != null)
                    return _BaseTexture.Bounds;
                else
                    return Rectangle.Empty;
            }
        }

        public bool IsLoaded
        {
            get { return _IsLoaded; }
        }
        protected bool _IsLoaded = false;

        #endregion Properties

        #region Initialization

        /// <summary>
        /// Construct a new CutlassTexture.
        /// </summary>
        public CutlassTexture()
        {
            _Active = true;
        }

        /// <summary>
        /// Construct a new CutlassTexture.
        /// </summary>
        /// <param name="fileName">The asset file name.</param>
        public CutlassTexture(string fileName)
            : this()
        {
            _FileName = fileName;
        }

        /// <summary>
        /// Load all texture assets.
        /// </summary>
        public void LoadContent()
        {
            if (!String.IsNullOrEmpty(_FileName))
            {
                _BaseTexture = CutlassEngine.ContentManager.Load<Texture2D>(_FileName);
                _IsLoaded = true;
                _ReadyToRender = true;
            }

            _IsLoaded = true;
        }

        /// <summary>
        /// Unload texture assets.
        /// </summary>
        public void UnloadContent()
        {
            _BaseTexture.Dispose();

            _IsLoaded = false;
        }

        #endregion Initialization
    }
}