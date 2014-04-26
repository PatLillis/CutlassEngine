using System;
using Microsoft.Xna.Framework.Graphics;

namespace Cutlass.Interfaces
{
    /// <summary>
    /// Interface for fonts used in the engine.
    /// </summary>
    public interface ICutlassFont
    {
        /// <summary>Filename of asset</summary>
        string FileName { get; set; }

        /// <summary>Underlying font</summary>
        SpriteFont Font { get; }

        /// <summary>Has this asset been loaded</summary>
        bool ReadyToRender { get;}

        /// <summary>
        /// Load the asset
        /// </summary>
        void LoadContent();

        /// <summary>
        /// Unload the asset
        /// </summary>
        void UnloadContent();
    }
}
