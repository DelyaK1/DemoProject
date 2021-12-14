using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnigmaSvgCore
{
    public class Cell
    {
        public int Id { get; set; } = -1;
        public Svg Svg { get; }
        public RectangleF Bounds { get; }
        public Cross TopLeftCross { get; }
        public Cross BottomLeftCross { get; }
        public Cross TopRightCross { get; }
        public Cross BottomRightCross { get; }

        public Cell(Svg svg, Cross topLeftCross, Cross topRightCross, Cross bottomLeftCross, Cross bottomRightCross)
        {
            Svg = svg;
            TopLeftCross = new Cross(topLeftCross.HorizontalLine, topLeftCross.VerticalLine);
            TopRightCross = new Cross(topRightCross.HorizontalLine, topRightCross.VerticalLine);
            BottomLeftCross = new Cross(bottomLeftCross.HorizontalLine, bottomLeftCross.VerticalLine);
            BottomRightCross = new Cross(bottomRightCross.HorizontalLine, bottomRightCross.VerticalLine);
            Bounds = new RectangleF(topLeftCross.Point.X, topLeftCross.Point.Y, topRightCross.Point.X - topLeftCross.Point.X, bottomRightCross.Point.Y - topRightCross.Point.Y);
        }

        public Content GetContent(Margins margins = null, bool hidden = false, float symbolsDistortion = 0.2f)
        {
            RectangleF bounds = Bounds;
            if (margins != null)
            {
                bounds = new RectangleF(bounds.Left + margins.Left, bounds.Top + margins.Top, bounds.Right - bounds.Left - margins.Left - margins.Right, bounds.Bottom - bounds.Top - margins.Top - margins.Bottom);
            }
            return new Content(Svg.GetWords(hidden, symbolsDistortion, bounds));
        }
    }
}
