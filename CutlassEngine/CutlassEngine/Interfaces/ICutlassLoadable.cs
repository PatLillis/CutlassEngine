using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cutlass.Interfaces
{
    public interface ICutlassLoadable
    {
        bool IsLoaded { get; set; }

        void LoadContent();

        void UnloadContent();
    }
}
