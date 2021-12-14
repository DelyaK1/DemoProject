using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EnigmaSvgCore
{
    public class Word
    {
        public Text[] Texts { get; set; }
        public string Value { get; set; } = string.Empty;
        public int Rotation { get; } = 0;
        public RectangleF Bounds { get; private set; } = new RectangleF();
        public Point CenterPoint { get; } = null;
        public bool Hidden { get; } = false;
        public Word(Text[] texts)
        {
            Texts = new Text[texts.Length];
            Array.Copy(texts, Texts, texts.Length);
            
            if (Texts.Length > 0)
            {
                Value = "";
            }
            for (int i = 0; i < texts.Length; i++)
            {
                Texts[i] = texts[i];
                Value += texts[i].Value;
            }
            if (texts.Length > 0)
            {
                Rotation = texts[0].Rotation;
                Bounds = GetBounds();
                CenterPoint = GetCenterPoint();
                Hidden = texts[0].Hidden;
            }
            else
            {
                Hidden = false;
            }
        }

        private RectangleF GetBounds()
        {
            Text textFirst = Texts.First();
            Text textLast = Texts.Last();
            float left = textFirst.Bounds.Left < textLast.Bounds.Right ? textFirst.Bounds.Left : textLast.Bounds.Left;
            float right = textLast.Bounds.Right > textFirst.Bounds.Left ? textLast.Bounds.Right : textFirst.Bounds.Right;
            float top = textFirst.Bounds.Top < textLast.Bounds.Top ? textFirst.Bounds.Top : textLast.Bounds.Top;
            float bottom = textFirst.Bounds.Bottom > textLast.Bounds.Bottom ? textFirst.Bounds.Bottom : textLast.Bounds.Bottom;
            return new RectangleF(left, top, right - left, bottom - top);
        }

        private Point GetCenterPoint()
        {
            var bounds = GetBounds();
            return new Point(bounds.Left + ((bounds.Right - bounds.Left) / 2), bounds.Top + ((bounds.Bottom - bounds.Top) / 2));
        }

        public Symbol[] GetSymbolsByValueIndexAndLength(int index, int length)
        {
            int currentIndex = 0;
            bool indexFound = false;
            int tIndex = 0;
            int sIndex = 0;
            Symbol[] symbols = new Symbol[length];
            for (int i = 0; i < Texts.Length; i++)
            {
                for (int j = 0; j < Texts[i].Symbols.Length; j++)
                {
                    if (currentIndex == index)
                    {
                        indexFound = true;
                        tIndex = i;
                        sIndex = j;
                        break;
                    }
                    currentIndex++;
                }
                if (indexFound)
                {
                    break;
                }
            }

            bool valuesFound = false;
            currentIndex = 0;
            for (int t = tIndex; t < Texts.Length; t++)
            {
                for (int s = sIndex; s < Texts[t].Symbols.Length; s++)
                {
                    if (currentIndex < length)
                    {
                        symbols[currentIndex++] = new Symbol(Texts[t].Symbols[s].Point, Texts[t].Symbols[s].Value, Texts[t].FontSize);
                    }
                    else
                    {
                        valuesFound = true;
                        break;
                    }
                }
                if (valuesFound)
                {
                    break;
                }
                else
                {
                    sIndex = 0;
                }
            }

            return symbols;
        }

        public Symbol[] GetSymbols()
        {
            int totalSymbols = 0;
            for (int t = 0; t < Texts.Length; t++)
            {
                totalSymbols += Texts[t].Symbols.Length;
            }
            Symbol[] allSymbols = new Symbol[totalSymbols];

            int symbolCounter = 0;
            for (int t = 0; t < Texts.Length; t++)
            {
                for (int s = 0; s < Texts[t].Symbols.Length; s++)
                {
                    allSymbols[symbolCounter++] = Texts[t].Symbols[s];
                }
            }

            return allSymbols;
        }
    }
}
