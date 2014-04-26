using System;
using Microsoft.Xna.Framework;

namespace Cutlass.Interfaces
{
    public interface ICutlassUpdateable : ICutlassSceneObject
    {
        void Update(GameTime gameTime);
    }
}
