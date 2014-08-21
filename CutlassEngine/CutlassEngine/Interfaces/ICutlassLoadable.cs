using System;

namespace Cutlass.Interfaces
{
    public interface ICutlassLoadable : ICutlassSceneObject
    {
        bool IsLoaded { get; }

        void LoadContent();

        void UnloadContent();
    }
}
