using BoundingRect;
using Microsoft.Xna.Framework;

namespace Cutlass.Interfaces
{
    public interface ICutlassDrawable : ICutlassSceneObject
    {
        int DrawOrder { get; set; }

        bool ReadyToRender { get; set; }

        bool IsVisible { get; set; }

        BoundingRectangle BoundingRect { get; set; }

        Vector2 Position { get; set; }

        Vector2 Scale { get; set; }

        float Rotation { get; set; }

        void Draw(GameTime gameTime);
    }
}