using Cutlass.Interfaces;
using Cutlass.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cutlass.Managers
{
    /// <summary>
    /// Manages access to all sound effects used in the game.
    /// </summary>
    public class SoundManager : GameComponent
    {
        #region Properties

        /// <summary>Where the actual sounds are stored, accessed by a string key</summary>
        private static Dictionary<SoundId, ICutlassSound> _Sounds = new Dictionary<SoundId, ICutlassSound>();

        /// <summary>Is the SoundManager Initialized.</summary>
        public static bool Initialized
        {
            get { return _Initialized; }
        }
        private static bool _Initialized = false;

        /// <summary>The number of sound effects that are currently loaded.</summary>
        private static int _NextSoundId = 0;

        private static Object _SoundIdLock = new Object();

        #endregion Properties
        
        #region Initialization

        /// <summary>
        /// Create the sound Manager.
        /// </summary>
        /// <param name="game"></param>
        public SoundManager(Game game)
            : base(game)
        { }

        /// <summary>
        /// Create the sound manager.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            foreach (ICutlassSound sound in _Sounds.Values)
            {
                if (!sound.IsLoaded)
                {
                    sound.LoadContent();
                }
            }

            _Initialized = true;
        }

        #endregion Initialization

        #region Public Methods

        /// <summary>
        /// Add a soundof type ICutlassSound
        /// </summary>
        /// <param name="newSound"></param>
        public static SoundId AddSound(ICutlassSound newSound)
        {
            SoundId soundId;

            lock (_SoundIdLock)
            {
                soundId = _NextSoundId++;
            }

            _Sounds.Add(soundId, newSound);

            if (_Initialized)
                newSound.LoadContent();

            return soundId;
        }

        /// <summary>
        /// Remove a sound.
        /// </summary>
        /// <param name="soundId"></param>
        public static void RemoveSound(SoundId soundId)
        {
            ICutlassSound soundToRemove;
            _Sounds.TryGetValue(soundId, out soundToRemove);

            if (soundToRemove != null)
            {
                if (_Initialized)
                    soundToRemove.UnloadContent();

                _Sounds.Remove(soundId);
            }
        }

        /// <summary>
        /// Get a sound
        /// </summary>
        /// <param name="soundId"></param>
        /// <returns></returns>
        public static ICutlassSound GetSound(SoundId soundId)
        {
            return _Sounds.ElementAtOrDefault(soundId).Value;
        }

        #endregion Public Methods
    }
}
