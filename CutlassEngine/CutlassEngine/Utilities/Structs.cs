using System;
using Microsoft.Xna.Framework;
using Cutlass.Interfaces;

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

    public struct SceneObjectId
    {
        private int Id;

        public SceneObjectId(int id) { Id = id; }

        public static implicit operator SceneObjectId(int id) { return new SceneObjectId(id); }

        public static implicit operator int(SceneObjectId id) { return id.Id; }
    }

    public struct CollisionContact
    {
        public ICutlassCollidable A, B;

        public Vector2 Normal;

        public float Distance;

        public CollisionContact(CollisionContact contact)
            : this(contact.A, contact.B, contact.Normal, contact.Distance)
        { }

        public CollisionContact(ICutlassCollidable a, ICutlassCollidable b, Vector2 normal, float distance)
        {
            A = a;
            B = b;
            Normal = normal;
            Distance = distance;
        }
    }
}
