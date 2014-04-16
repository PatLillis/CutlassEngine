using BoundingRect;
using Microsoft.Xna.Framework;

namespace Cutlass.Interfaces
{
    public interface ICutlassDrawable : ICutlassSceneObject
    {
        //Allows for ordering drawing elements
        int DrawOrder { get; set; }

        //Whether this object should be fixed with respect to the player, or with respect to the screen
        bool ScreenPositionFixed { get; }

        bool ReadyToRender { get; set; }

        bool IsVisible { get; set; }

        float Width { get; }

        float Height { get; }

        BoundingRectangle BoundingRect { get; }

        Vector2 Position { get; set; }

        Vector2 Scale { get; set; }

        float Rotation { get; set; }

        void Draw(GameTime gameTime);
    }
}