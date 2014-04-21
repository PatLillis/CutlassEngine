using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cutlass.Utilities
{
    public struct TexId
    {
        private int Id;

        public TexId(int id)
        {
            Id = id;
        }

        public static implicit operator TexId(int id)
        {
            return new TexId(id);
        }

        public static implicit operator int(TexId id)
        {
            return id.Id;
        }
    }
}
