using Cutlass.Interfaces;
using Cutlass.Utilities;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Cutlass.Assets
{
    public class CutlassSong : ICutlassSound
    {
        #region Properties

        Song _Song;

        bool _IsPlaying = false;

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
        public CutlassSong(string fileName)
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
                _Song = CutlassEngine.ContentManager.Load<Song>(_Filename);
                _IsLoaded = true;
            }

            _IsLoaded = true;
        }

        /// <summary>
        /// Unload texture assets.
        /// </summary>
        public void UnloadContent()
        {
            _Song.Dispose();
            _IsLoaded = false;
        }

        #endregion Initialization

        #region Public Methods

        public void Play()
        {
            if (_Song != null)
            {
                MediaPlayer.Play(_Song);
                _IsPlaying = true;
            }
        }

        public void PlayFadeIn(float fadeTimeMilliseconds)
        {
            MediaPlayer.Volume = 0;
            Play();

            float deltaVolume = 1f/100;

            Timer timer = new Timer(fadeTimeMilliseconds / 100);

            timer.Elapsed += new ElapsedEventHandler((object o, ElapsedEventArgs args) =>
            {
                MediaPlayer.Volume += deltaVolume;
                
                if (MediaPlayer.Volume == 1.0)
                {
                    timer.Stop();
                }
            });

            timer.Start();
        }

        public void Pause()
        {
            if (_Song != null && _IsPlaying)
            {
                MediaPlayer.Pause();
                _IsPlaying = false;
            }
        }

        public void Stop()
        {
            if (_Song != null && _IsPlaying)
            {
                MediaPlayer.Stop();
                _IsPlaying = false;
            }
        }

        public void StopFadeOut(float fadeTimeMilliseconds)
        {

        }

        #endregion Public Methods
    }
}
