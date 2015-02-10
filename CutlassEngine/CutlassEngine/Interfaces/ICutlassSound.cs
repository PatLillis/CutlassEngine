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

        /// <summary>Underlying sound effect, essentially "global"</summary>
        SoundEffectInstance Instance { get; }
    }
}
