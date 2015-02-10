using Cutlass.Interfaces;
using Cutlass.Utilities;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cutlass.Assets
{
    /// <summary>
    /// A sound effect, things like jumps, beeps, music, etc.
    /// </summary>
    public class CutlassSound : ICutlassSound
    {
        #region Properties

        /// <summary>Once this is set to false, will be removed from Scene.</summary>
        public bool Active
        {
            get { return _Active; }
            set { _Active = value; }
        }
        protected bool _Active;

        public SceneObjectId SceneObjectId { get; set; }

        /// <summary>Filename of asset</summary>
        public string Filename
        {
            get { return _Filename; }
            set { _Filename = value; }
        }
        protected string _Filename;

        /// <summary>Underlying sound effect, essentially "global"</summary>
        public SoundEffectInstance Instance
        {
            get
            {
                if (_SoundEffect != null)
                    return _SoundEffect.CreateInstance();
                else
                    return null;
            }
        }
        protected SoundEffect _SoundEffect;

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
        /// <param name="fileName">The asset file name.</param>
        public CutlassSound(string fileName)
        {
            _Filename = fileName;
            _Active = true;
        }

        /// <summary>
        /// Load all texture assets.
        /// </summary>
        public void LoadContent()
        {
            if (!String.IsNullOrEmpty(_Filename))
            {
                _SoundEffect = CutlassEngine.ContentManager.Load<SoundEffect>(_Filename);
                _IsLoaded = true;
            }

            _IsLoaded = true;
        }

        /// <summary>
        /// Unload texture assets.
        /// </summary>
        public void UnloadContent()
        {
            _SoundEffect.Dispose();
            _IsLoaded = false;
        }

        #endregion Initialization
    }
}
