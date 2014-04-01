using Microsoft.Xna.Framework;

namespace Cutlass.Interfaces
{
    public interface ICutlassDrawable : ICutlassSceneObject
    {
        bool PostUIDraw { get; set; }

        void Draw(GameTime gameTime);
    }
}