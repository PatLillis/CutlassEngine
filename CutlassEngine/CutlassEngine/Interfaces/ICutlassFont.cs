using System;
using Microsoft.Xna.Framework.Graphics;

namespace Cutlass.Interfaces
{
    /// <summary>
    /// Interface for fonts used in the engine.
    /// </summary>
    public interface ICutlassFont : ICutlassLoadable
    {
        /// <summary>Filename of asset</summary>
        string Filename { get; set; }

        /// <summary>Underlying font</summary>
        SpriteFont Font { get; }
    }
}
