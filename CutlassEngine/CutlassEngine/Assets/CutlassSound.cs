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

        SoundEffect _SoundEffect;
        SoundEffectInstance _SoundEffectInstance;

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
                _SoundEffectInstance = _SoundEffect.CreateInstance();
                _IsLoaded = true;
            }

            _IsLoaded = true;
        }

        /// <summary>
        /// Unload texture assets.
        /// </summary>
        public void UnloadContent()
        {
            _SoundEffectInstance.Dispose();
            _SoundEffect.Dispose();
            _IsLoaded = false;
        }

        #endregion Initialization

        #region Public Methods

        public void Play()
        {
            if (_SoundEffectInstance != null)
            {
                _SoundEffectInstance.Play();
            }
        }

        public void PlayFadeIn(float fadeTimeMilliseconds)
        {

        }

        public void Pause()
        {
            if (_SoundEffectInstance != null)
            {
                _SoundEffectInstance.Pause();
            }
        }

        public void Stop()
        {
            if (_SoundEffectInstance != null)
            {
                _SoundEffectInstance.Stop();
            }
        }

        public void StopFadeOut(float fadeTimeMilliseconds)
        {

        }

        #endregion Public Methods
    }
}
