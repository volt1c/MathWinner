using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MathWinner.Library.Models
{
    public class PointTransformer
    {
        public Vector2 Position { get; private set; }

        public PointTransformer(float x, float y)
        {
            Position = new Vector2(x, y);
        }

        public void ReflectOverLineXEquals(float x)
        {
            Position = new Vector2(2 * x - Position.X, Position.Y);
        }

        public void ReflectOverLineYEquals(float y)
        {
            Position = new Vector2(Position.X, 2 * y - Position.Y);
        }

        public void Rotate(float degrees)
        {
            float angleRad = MathF.PI * degrees / 180;
            float cosA = MathF.Cos(angleRad);
            float sinA = MathF.Sin(angleRad);
            Position = new Vector2(
                Position.X * cosA - Position.Y * sinA,
                Position.X * sinA + Position.Y * cosA
            );
        }

        public void ReflectOverOrigin()
        {
            Position = new Vector2(-Position.X, -Position.Y);
        }

        public void Scale(float scaleX, float scaleY)
        {
            Position = new Vector2(Position.X * scaleX, Position.Y * scaleY);
        }

        public void Translate(float dx, float dy)
        {
            Position += new Vector2(dx, dy);
        }
    }
}
