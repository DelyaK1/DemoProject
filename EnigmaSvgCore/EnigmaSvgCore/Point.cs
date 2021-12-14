using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EnigmaSvgCore
{
    public class Point
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Point(string x, string y)
        {
            X = float.Parse(x);
            Y = float.Parse(y);
        }

        public static float GetDistance(Point point1, Point point2)
        {
            return (float)Math.Sqrt(((point2.X - point1.X) * (point2.X - point1.X)) + ((point2.Y - point1.Y) * (point2.Y - point1.Y)));
        }
    }
}
