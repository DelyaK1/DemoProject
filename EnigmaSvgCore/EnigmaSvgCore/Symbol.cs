using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnigmaSvgCore
{
    public class Symbol
    {
        public Point Point { get; set; }
        public string Value { get; set; } = string.Empty;
        public float FontSize { get; set; }
        public char? LeadCharacter { get; set; } = null;
        public Symbol(Point point, string value, float fontSize)
        {
            Point = new Point(point.X, point.Y);
            Value = value;
            FontSize = fontSize;
        }

        public static string GetValueFromSymbols(Symbol[] symbols)
        {
            string value = "";
            for (int i = 0; i < symbols.Length; i++)
            {
                value += string.IsNullOrEmpty(symbols[i].Value) ? "" : symbols[i].Value;
            }
            return value;
        }
    }
}
