using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Cutlass.Interfaces
{
    public interface ICutlassDrawable : ICutlassObject, IDrawable
    {
        bool PostUIDraw { get; set; }
    }
}
