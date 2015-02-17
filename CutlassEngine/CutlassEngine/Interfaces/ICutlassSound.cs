using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cutlass.Interfaces
{
    public interface ICutlassSound : ICutlassLoadable
    {
        /// <summary>Filename of asset</summary>
        string Filename { get; set; }

        void Play();

        void PlayFadeIn(float fadeTimeMilliseconds);

        void Pause();

        void Stop();

        void StopFadeOut(float fadeTimeMilliseconds);
    }
}
