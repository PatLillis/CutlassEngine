using Microsoft.Xna.Framework.Graphics;

namespace Cutlass.Interfaces
{
    public interface ICutlassTexture
    {
        string FileName
        {
            get;
            set;
        }

        Texture2D BaseTexture
        {
            get;
        }

        bool ReadyToRender
        {
            get;
        }

        void LoadContent();

        void UnloadContent();
    }
}
