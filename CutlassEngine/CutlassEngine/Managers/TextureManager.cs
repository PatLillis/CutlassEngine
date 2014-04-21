using System.Collections.Generic;
using System.Linq;
using Cutlass.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cutlass.Utilities;

namespace Cutlass.Managers
{
    /// <summary>
    /// Handle all textures for the game.
    /// </summary>
    public class TextureManager : GameComponent
    {
        /// <summary>Where the actual textures are stored, accessed by a string key</summary>
        private static Dictionary<int, ICutlassTexture> _Textures = new Dictionary<int, ICutlassTexture>();

        /// <summary>The default texture to use</summary>
        public static Texture2D PointTexture
        {
            get { return _PointTexture; }
        }
        private static Texture2D _PointTexture = null;

        /// <summary>Is the TextureManagers Initialized, used for test cases and setup of Effects.</summary>
        public static bool Initialized
        {
            get { return _Initialized; }
        }
        private static bool _Initialized = false;

        /// <summary>The number of textures that are currently loaded.</summary>
        private static int _NextId = 0;

        /// <summary>
        /// Create the texture Manager.
        /// </summary>
        /// <param name="game"></param>
        public TextureManager(Game game)
            : base(game)
        { }

        /// <summary>
        /// Add a texture of type CutlassTexture. Returns the id of the new texture
        /// </summary>
        /// <param name="newTexture"></param>
        /// <param name="textureName"></param>
        public static TexId AddTexture(ICutlassTexture newTexture)
        {
            _Textures.Add(_NextId, newTexture);

            if (_Initialized)
                newTexture.LoadContent();

            return _NextId++;
        }

        /// <summary>
        /// Remove a texture from the dictionary.
        /// </summary>
        /// <param name="textureName"></param>
        public static void RemoveTexture(TexId textureId)
        {
            ICutlassTexture textureToRemove;
            _Textures.TryGetValue(textureId, out textureToRemove);

            if (textureToRemove != null)
            {
                if (_Initialized)
                    _Textures[textureId].UnloadContent();

                _Textures.Remove(textureId);
            }
        }

        /// <summary>
        /// Get a texture
        /// </summary>
        /// <param name="textureId"></param>
        /// <returns></returns>
        public static ICutlassTexture GetTexture(TexId textureId)
        {
            return _Textures.ElementAtOrDefault(textureId).Value;
        }

        /// <summary>
        /// Get a Texture2D
        /// </summary>
        /// <param name="textureName"></param>
        /// <returns></returns>
        public static Texture2D GetTexture2D(TexId textureId)
        {
            ICutlassTexture texture = GetTexture(textureId);

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

            //Set PointTexture (if it hasn't already been set)
            if (_PointTexture == null)
            {
                _PointTexture = new Texture2D(CutlassEngine.Device, 1, 1);
                _PointTexture.SetData<Color>(new Color[] { Color.White });
            }

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