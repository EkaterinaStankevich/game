using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsPlatformer
{
    struct Rect
    {
        public Vector Center;
        public Size Size;

        public double Width
        {
            get { return Size.Width; }
            set { Size.Width = value; }
        }

        public double Height
        {
            get { return Size.Height; }
            set { Size.Height = value; }
        }

        public double X
        {
            get { return Center.X; }
            set { Center.X = value; }
        }

        public double Y
        {
            get { return Center.Y; }
            set { Center.Y = value; }
        }

        public Rect(Vector center, Size size)
        {
            Center = center;
            Size = size;
        }

        public Rect(double x, double y, double width, double height)
        {
            Center = new Vector(x, y);
            Size = new Size(width, height);
        }
    }
}
