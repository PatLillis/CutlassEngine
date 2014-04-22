using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cutlass.Utilities
{
    public struct TexId
    {
        private int Id;

        public TexId(int id) { Id = id; }

        public static implicit operator TexId(int id) { return new TexId(id); }

        public static implicit operator int(TexId id) { return id.Id; }
    }

    public struct FontId
    {
        private int Id;

        public static readonly FontId Default = new FontId(-1);

        public FontId(int id) { Id = id; }

        public static implicit operator FontId(int id) { return new FontId(id); }

        public static implicit operator int(FontId id) { return id.Id; }
    }
}
