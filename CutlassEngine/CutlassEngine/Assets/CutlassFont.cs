using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cutlass.Interfaces;
using Cutlass.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace Cutlass.Assets
{
    public class CutlassFont : ICutlassFont
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

        private SpriteFont _Font;
        ///<summary>
        ///Gets the underlying font.
        ///</summary>
        public SpriteFont Font
        {
            get { return _Font; }
        }

        private bool _ReadyToRender = false;
        ///<summary>
        ///Is the font ready to be rendered.
        ///</summary>
        public bool ReadyToRender
        {
            get { return _ReadyToRender; }
        }

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

        public void LoadContent()
        {
            if (!String.IsNullOrEmpty(_FileName))
            {
                _Font = CutlassEngine.ContentManager.Load<SpriteFont>(_FileName);
                _ReadyToRender = true;
            }
        }

        public void UnloadContent()
        { }
    }
}
