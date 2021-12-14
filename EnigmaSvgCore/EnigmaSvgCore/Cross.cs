using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnigmaSvgCore
{
    public class Cross
    {
        public Line HorizontalLine { get; set; }
        public Line VerticalLine { get; set; }
        public Point Point { get; set; }

        public Cross()
        { 
            
        }

        public Cross(Line hLine, Line vLine)
        {
            HorizontalLine = new Line(hLine.Point1, hLine.Point2, hLine.Paths, hLine.Id);
            VerticalLine = new Line(vLine.Point1, vLine.Point2, vLine.Paths, vLine.Id);
            Point = new Point(vLine.Point1.X, hLine.Point1.Y);
        }
    }
}
