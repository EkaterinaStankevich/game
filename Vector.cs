using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsPlatformer
{
    struct Vector
    {
        public double X;
        public double Y;
        public static Vector Zero = new Vector(0, 0);

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Vector operator *(Vector vector, double scalar)
        {
            return new Vector(vector.X * scalar, vector.Y * scalar);
        }

        public static Vector operator /(Vector vector, double scalar)
        {
            return new Vector(vector.X / scalar, vector.Y / scalar);
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.X - v2.X, v1.Y - v2.Y);
        }

    }
}
