using BoundingRect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cutlass.Interfaces
{
    public interface ICutlassDrawable : ICutlassSceneObject
    {
        //Allows for ordering drawing elements
        int DrawOrder { get; set; }

        //Whether this object should be fixed with respect to the player, or with respect to the screen
        bool ScreenPositionFixed { get; }

        bool ReadyToRender { get; }

        bool IsVisible { get; set; }

        int Width { get; }

        int Height { get; }

        ICutlassTexture Texture { get; }

        BoundingRectangle BoundingRect { get; }

        Vector2 Position { get; set; }

        Vector2 Scale { get; set; }

        float Rotation { get; set; }

        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}