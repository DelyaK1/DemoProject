using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace EnigmaSvgCore
{
    public class Content
    {
        public Word[] Words { get; set; }
        public string Value { get; set; } = string.Empty;
        public bool Hidden { get; set; } = false;
        public RectangleF Bounds { get; private set; } = new RectangleF();
        public Content(Word[] words)
        {
            Words = new Word[words.Length];
            
            Value = "";
            for (int w = 0; w < words.Length; w++)
            {
                Words[w] = words[w];
                for (int t = 0; t < words[w].Texts.Length; t++)
                {
                    for (int s = 0; s < words[w].Texts[t].Symbols.Length; s++)
                    {
                        if (words[w].Texts[t].Symbols[s].LeadCharacter != null)
                        {
                            Value += words[w].Texts[t].Symbols[s].LeadCharacter.ToString();
                        }
                        Value += words[w].Texts[t].Symbols[s].Value;
                    }
                }
            }

            try
            {
                Hidden = Words.First().Hidden;
            }
            catch { }

            Bounds = FindBounds();
        }

        private RectangleF FindBounds()
        {
            if (Words.Length > 0)
            {
                var left = Words.OrderBy(w => w.Bounds.Left).First().Bounds.Left;
                var right = Words.OrderByDescending(w => w.Bounds.Right).First().Bounds.Right;
                var top = Words.OrderBy(w => w.Bounds.Top).First().Bounds.Top;
                var bottom = Words.OrderBy(w => w.Bounds.Bottom).Last().Bounds.Bottom;

                return new RectangleF(left, top, right - left, bottom - top);
            }
            else
            {
                return new RectangleF();
            }
        }
    }
}