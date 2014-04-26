using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cutlass.Interfaces
{
    /// <summary>
    /// Interface for textures used in the engine.
    /// </summary>
    public interface ICutlassTexture : ICutlassLoadable
    {
        /// <summary>Filename of asset</summary>
        string FileName { get; set; }

        /// <summary>Underlying texture</summary>
        Texture2D BaseTexture { get; }

        /// <summary>Width of texture, or -1 if no texture</summary>
        int Width { get; }

        /// <summary>Height of texture, or -1 if no texture</summary>
        int Height { get; }

        Rectangle AreaToRender { get; }

        /// <summary>Has this asset been loaded</summary>
        bool ReadyToRender { get; }
    }
}
