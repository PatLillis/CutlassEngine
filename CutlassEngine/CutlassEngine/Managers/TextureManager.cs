using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cutlass.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cutlass.Managers
{
    public class TextureManager : GameComponent
    {
        private static Dictionary<string, ICutlassTexture> _Textures = new Dictionary<string, ICutlassTexture>();

        private static bool _Initialized = false;
        /// <summary>Is the TextureManagers Initialized, used for test cases and setup of Effects.</summary>
        public static bool Initialized
        {
            get { return _Initialized; }
        }

        private static int _TexturesLoaded = 0;
        /// <summary>
        /// The number of textures that are currently loaded.
        /// Use this for user loading bar feedback.
        /// </summary>
        public static int TexturesLoaded
        {
            get { return _TexturesLoaded; }
        }

        /// <summary>
        /// Create the texture Manager.
        /// </summary>
        /// <param name="game"></param>
        public TextureManager(Game game)
            : base(game)
        { }

        /// <summary>
        /// Add a texture of type CutlassTexture.
        /// </summary>
        /// <param name="newTexture"></param>
        /// <param name="textureName"></param>
        public static void AddTexture(ICutlassTexture newTexture, string textureName)
        {
            if (textureName != null && !_Textures.ContainsKey(textureName))
            {
                _Textures.Add(textureName, newTexture);
                if (_Initialized)
                {
                    newTexture.LoadContent();
                }
            }
        }

        public static void RemoveTexture(string textureName)
        {
            if (textureName != null && _Textures.ContainsKey(textureName))
            {
                if (_Initialized)
                {
                    _Textures[textureName].UnloadContent();
                    _Textures.Remove(textureName);
                    _TexturesLoaded--;
                }
            }
        }

        /// <summary>
        /// Get a texture
        /// </summary>
        /// <param name="textureId"></param>
        /// <returns></returns>
        public static ICutlassTexture GetTexture(string textureName)
        {
            if (textureName != null && _Textures.ContainsKey(textureName))
            {
                return _Textures[textureName];
            }
            return null;
        }

        public static Texture2D GetTexture2D(string textureName)
        {
            ICutlassTexture texture = GetTexture(textureName);
            if (texture != null)
                return texture.BaseTexture;
            else
                return null;
        }

        /// <summary>
        /// Create the shaders.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            foreach (ICutlassTexture texture in _Textures.Values)
            {
                if (!texture.ReadyToRender)
                {
                    texture.LoadContent();
                }
            }

            _Initialized = true;
        }
    }
}
