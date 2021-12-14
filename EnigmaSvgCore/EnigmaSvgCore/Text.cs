using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EnigmaSvgCore
{
    public class Text
    {
        public Node OpenNode { get; set; } = null;
        public Node ContentNode { get; set; } = null;
        public Node CloseNode { get; set; } = null;
        public int Rotation { get; } = 0;
        public Symbol[] Symbols { get; set; }
        public string Value { get; } = string.Empty;
        public float FontSize { get; } = 0.0f;
        public bool Hidden { get; } = false;
        public RectangleF Bounds { get; private set; } = new RectangleF();
        public Text(Node openNode, Node contentNode, Node closeNode)
        {
            if (contentNode.Type == Node.NodeType.Closing)
            {
                closeNode = contentNode;
                contentNode = null;
            }

            OpenNode = new Node
            {
                Attributes = new Dictionary<string, string>(openNode.Attributes),
                Depth = openNode.Depth,
                Name = openNode.Name,
                Order = openNode.Order,
                Type = openNode.Type,
                Value = openNode.Value
            };

            if (OpenNode.IsAttributeExist("style") && OpenNode.IsAttributeExist("fill-opacity") && OpenNode.Attributes["fill-opacity"] == "1" && !OpenNode.Attributes["style"].ToLower().Contains("fill:#ffffff"))
            {
                Hidden = false;
            }
            else if (OpenNode.IsAttributeExist("style") && OpenNode.IsAttributeExist("fill-opacity") && (OpenNode.Attributes["fill-opacity"] == "0" || OpenNode.Attributes["style"].ToLower().Contains("fill:#ffffff")))
            {
                Hidden = true;
            }

            if (contentNode != null)
            {
                ContentNode = new Node
                {
                    Attributes = new Dictionary<string, string>(contentNode.Attributes),
                    Depth = contentNode.Depth,
                    Name = contentNode.Name,
                    Order = contentNode.Order,
                    Type = contentNode.Type,
                    Value = contentNode.Value
                };
            }
            
            CloseNode = new Node
            {
                Attributes = new Dictionary<string, string>(closeNode.Attributes),
                Depth = closeNode.Depth,
                Name = closeNode.Name,
                Order = closeNode.Order,
                Type = closeNode.Type,
                Value = closeNode.Value
            };

            Value = ContentNode == null ? "" : ContentNode.Value;

            FontSize = float.Parse(OpenNode.Attributes["font-size"]);
            float textHeight = FontSize - (FontSize / 4);

            string[] sxValues = Regex.Replace(OpenNode.Attributes["x"].Trim(' '), @"[ ]+", " ").Replace(" ", ",").Split(',');
            string[] syValues = Regex.Replace(OpenNode.Attributes["y"].Trim(' '), @"[ ]+", " ").Replace(" ", ",").Split(',');
            Symbols = new Symbol[sxValues.Length];

            if (ContentNode == null)
            {
                Symbols = new Symbol[0];
            }

            float textWidth = textHeight;

            if (OpenNode.IsAttributeExist("transform"))
            {
                Matrix matrix = new Matrix(OpenNode);
                Rotation = matrix.GetRotation();

                var widthHeightPositions = matrix.GetWidthAndHeightPositions();
                FontSize *= Math.Abs(matrix.GetMatrixValueByPositionNumber(widthHeightPositions.Item2));
                textHeight = FontSize - (FontSize / 4);
                textWidth = textHeight;

                if (Symbols.Length == 1)
                {
                    for (int s = 0; s < Symbols.Length; s++)
                    {
                        float smX = matrix.E + (float.Parse(sxValues[s]) * Math.Abs(matrix.GetMatrixValueByPositionNumber(widthHeightPositions.Item1)));
                        float smY = matrix.F + (float.Parse(syValues[s]) * Math.Abs(matrix.GetMatrixValueByPositionNumber(widthHeightPositions.Item2)));
                        Symbols[s] = new Symbol(new Point(smX, smY), Value.Substring(s, 1), FontSize);
                    }
                }
                else if (Symbols.Length > 1)
                {
                    float smX;
                    float smY;
                    switch (Rotation)
                    {
                        case 0:
                            smY = matrix.F + (float.Parse(syValues.First()) * Math.Abs(matrix.GetMatrixValueByPositionNumber(widthHeightPositions.Item2)));
                            for (int s = 0; s < Symbols.Length; s++)
                            {
                                smX = matrix.E + (float.Parse(sxValues[s]) * Math.Abs(matrix.GetMatrixValueByPositionNumber(widthHeightPositions.Item1)));
                                Symbols[s] = new Symbol(new Point(smX, smY), Value.Substring(s, 1), FontSize);
                            }
                            break;
                        case 90:
                            smX = matrix.E + (float.Parse(sxValues.First()) * Math.Abs(matrix.GetMatrixValueByPositionNumber(widthHeightPositions.Item1)));
                            for (int s = 0; s < Symbols.Length; s++)
                            {
                                smY = matrix.F + (float.Parse(sxValues[s]) * Math.Abs(matrix.GetMatrixValueByPositionNumber(widthHeightPositions.Item2)));
                                Symbols[s] = new Symbol(new Point(smX, smY), Value.Substring(s, 1), FontSize);
                            }
                            break;
                        case 180:
                            smY = matrix.F + (float.Parse(syValues.First()) * Math.Abs(matrix.GetMatrixValueByPositionNumber(widthHeightPositions.Item2)));
                            for (int s = 0; s < Symbols.Length; s++)
                            {
                                smX = matrix.E - (float.Parse(sxValues[s]) * Math.Abs(matrix.GetMatrixValueByPositionNumber(widthHeightPositions.Item1)));
                                Symbols[s] = new Symbol(new Point(smX, smY), Value.Substring(s, 1), FontSize);
                            }
                            break;
                        case 270:
                            smX = matrix.E + (float.Parse(sxValues.First()) * Math.Abs(matrix.GetMatrixValueByPositionNumber(widthHeightPositions.Item1)));
                            for (int s = 0; s < Symbols.Length; s++)
                            {
                                smY = matrix.F - (float.Parse(sxValues[s]) * Math.Abs(matrix.GetMatrixValueByPositionNumber(widthHeightPositions.Item2)));
                                Symbols[s] = new Symbol(new Point(smX, smY), Value.Substring(s, 1), FontSize);
                            }
                            break;
                    }
                }
                else
                { 
                    // хуясе 0_о
                }
            }
            else
            {
                Rotation = 0;
                
                for (int s = 0; s < Symbols.Length; s++)
                {
                    float smX = float.Parse(sxValues[s]);
                    float smY = float.Parse(syValues[s]);
                    Symbols[s] = new Symbol(new Point(smX, smY), Value.Substring(s, 1), FontSize);
                }
            }

            if (ContentNode != null)
            {
                switch (Rotation)
                {
                    case 0:
                        textWidth += Symbols.Last().Point.X - Symbols.First().Point.X;
                        Bounds = new RectangleF(Symbols.First().Point.X, Symbols.First().Point.Y - textHeight, textWidth, textHeight);
                        break;
                    case 90:
                        textWidth += Symbols.Last().Point.Y - Symbols.First().Point.Y;
                        Bounds = new RectangleF(Symbols.First().Point.X, Symbols.First().Point.Y, textHeight, textWidth);
                        break;
                    case 180:
                        textWidth += Symbols.First().Point.X - Symbols.Last().Point.X;
                        Bounds = new RectangleF(Symbols.First().Point.X - textWidth, Symbols.First().Point.Y, textWidth, textHeight);
                        break;
                    case 270:
                        textWidth += Symbols.First().Point.Y - Symbols.Last().Point.Y;
                        Bounds = new RectangleF(Symbols.First().Point.X - textHeight, Symbols.First().Point.Y - textWidth, textHeight, textWidth);
                        break;
                }
            }
        }
    }
}
