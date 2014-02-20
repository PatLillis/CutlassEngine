using Microsoft.Xna.Framework.Graphics;

namespace Cutlass.Interfaces
{
    public interface ICutlassFont
    {
        string FileName
        {
            get;
            set;
        }

        SpriteFont Font
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
