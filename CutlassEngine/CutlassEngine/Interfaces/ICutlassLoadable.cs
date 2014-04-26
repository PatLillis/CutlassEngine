using System;

namespace Cutlass.Interfaces
{
    public interface ICutlassLoadable
    {
        bool IsLoaded { get; }

        void LoadContent();

        void UnloadContent();
    }
}
