using System;
using Microsoft.Xna.Framework;

namespace Cutlass.Utilities
{
    public class VectorUtilities
    {
        public static Vector2 NormalizedMajorAxis(Vector2 vector)
        {
            Vector2 returnVector = MajorAxis(vector);

            if (returnVector.Length() > 0.0f)
                returnVector.Normalize();

            return returnVector;
        }

        public static Vector2 NormalizedMinorAxis(Vector2 vector)
        {
            Vector2 returnVector = MinorAxis(vector);

            if (returnVector.Length() > 0.0f)
                returnVector.Normalize();

            return returnVector;
        }

        public static Vector2 MajorAxis(Vector2 vector)
        {
            Vector2 returnVector;
            if (Math.Abs(vector.X) > Math.Abs(vector.Y))
                returnVector = new Vector2(vector.X, 0.0f);
            else
                returnVector = new Vector2(0.0f, vector.Y);

            return returnVector;
        }

        public static Vector2 MinorAxis(Vector2 vector)
        {
            Vector2 returnVector;
            if (Math.Abs(vector.X) < Math.Abs(vector.Y))
                returnVector = new Vector2(vector.X, 0.0f);
            else
                returnVector = new Vector2(0.0f, vector.Y);

            return returnVector;
        }

        public static Vector2 XAxis(Vector2 vector)
        {
            return new Vector2(vector.X, 0.0f);
        }

        public static Vector2 YAxis(Vector2 vector)
        {
            return new Vector2(0.0f, vector.Y);
        }

        public static float MaximumComponent(Vector2 vector)
        {
            if (Math.Abs(vector.X) > Math.Abs(vector.Y))
                return vector.X;
            else
                return vector.Y;
        }

        public static float MinimumComponent(Vector2 vector)
        {
            if (Math.Abs(vector.X) < Math.Abs(vector.Y))
                return vector.X;
            else
                return vector.Y;
        }

        public static Vector2 AbsoluteValue(Vector2 vector)
        {
            return new Vector2(Math.Abs(vector.X), Math.Abs(vector.Y));
        }

        public static float DotProduct(Vector2 a, Vector2 b)
        {
            return (a.X * b.X) + (a.Y * b.Y);
        }

        public static Vector2 PerpendicularVector(Vector2 vector)
        {
            return new Vector2(vector.Y, -vector.X);
        }

        public static Vector2 FromPoint(Point point)
        {
            return new Vector2(point.X, point.Y);
        }
    }
}
