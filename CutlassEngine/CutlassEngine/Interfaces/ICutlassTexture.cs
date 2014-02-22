using Microsoft.Xna.Framework.Graphics;

namespace Cutlass.Interfaces
{
    /// <summary>
    /// Interface for textures used in the engine.
    /// </summary>
    public interface ICutlassTexture
    {
        /// <summary>Filename of asset</summary>
        string FileName { get; set; }

        /// <summary>Underlying texture</summary>
        Texture2D BaseTexture { get; }

        /// <summary>Has this asset been loaded</summary>
        bool ReadyToRender { get; }

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
