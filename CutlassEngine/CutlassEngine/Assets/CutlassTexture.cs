using System;
using Cutlass.Interfaces;
using Microsoft.Xna.Framework.Graphics;

namespace Cutlass.Assets
{
    public class CutlassTexture : ICutlassTexture
    {
        /// <summary>The file name of the asset.</summary>
        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }
        private string _FileName;

        ///<summary>Gets the underlying texture.</summary>
        public Texture2D BaseTexture
        {
            get { return _BaseTexture; }
        }
        private Texture2D _BaseTexture;

        ///<summary>Is the texture ready to be rendered.</summary>
        public bool ReadyToRender
        {
            get { return _ReadyToRender; }
        }
        private bool _ReadyToRender = false;

        /// <summary>
        /// Construct a new CutlassTexture.
        /// </summary>
        public CutlassTexture()
        { }

        /// <summary>
        /// Construct a new CutlassTexture.
        /// </summary>
        /// <param name="fileName">The asset file name.</param>
        public CutlassTexture(string fileName)
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
                _ReadyToRender = true;
            }
        }

        /// <summary>
        /// Unload texture assets.
        /// </summary>
        public void UnloadContent()
        {
            _BaseTexture.Dispose();
        }
    }
}