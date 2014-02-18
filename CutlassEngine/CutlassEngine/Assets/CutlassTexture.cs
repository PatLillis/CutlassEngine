using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cutlass.Interfaces;
using Cutlass.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace Cutlass.Assets
{
    public class CutlassTexture : ICutlassTexture
    {
        private string _FileName;
        /// <summary>
        /// The file name of the asset.
        /// </summary>
        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }

        private Texture2D _BaseTexture;
        ///<summary>
        ///Gets the underlying Effect.
        ///</summary>
        public Texture2D BaseTexture
        {
            get { return _BaseTexture; }
        }

        private bool _ReadyToRender = false;
        ///<summary>
        ///Is the texture ready to be rendered.
        ///</summary>
        public bool ReadyToRender
        {
            get { return _ReadyToRender; }
        }

        /// <summary>
        /// Construct a new RoeTexture.
        /// </summary>
        public CutlassTexture()
        { }

        /// <summary>
        /// Construct a new RoeTexture.
        /// </summary>
        /// <param name="fileName">The asset file name.</param>
        public CutlassTexture(string fileName)
        {
            _FileName = fileName;
        }

        public void LoadContent()
        {
            if (!String.IsNullOrEmpty(_FileName))
            {
                _BaseTexture = CutlassEngine.ContentManager.Load<Texture2D>(_FileName);
                _ReadyToRender = true;
            }
        }

        public void UnloadContent()
        {
            _BaseTexture.Dispose();
        }
    }
}