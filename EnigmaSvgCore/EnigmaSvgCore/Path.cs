using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnigmaSvgCore
{
    public class Path
    {
        public Node Node { get; set; } = null;
        public Command[] Commands { get; set; } = null;
        public Path(Node node)
        {
            Node = new Node(node);
            Commands = Command.ParseCommandsFromD(Node.Attributes["d"]);
        }

        public void ApplyTransformMatrix()
        {
            if (Node.Attributes.ContainsKey("transform"))
            {
                Matrix matrix = new Matrix(Node);

                string newDAttributeValue = "";
                for (int c = 0; c < Commands.Length; c++)
                {
                    newDAttributeValue += Commands[c].Name.ToString();
                    if (Commands[c].Name == Command.CommandName.C ||
                        Commands[c].Name == Command.CommandName.c ||
                        Commands[c].Name == Command.CommandName.L ||
                        Commands[c].Name == Command.CommandName.l ||
                        Commands[c].Name == Command.CommandName.M ||
                        Commands[c].Name == Command.CommandName.m)
                    {
                        for (int i = 0; i < Commands[c].Values.Length; i += 2)
                        {
                            Commands[c].Values[i] = (matrix.A * Commands[c].Values[i]) + (matrix.C * Commands[c].Values[i + 1]) + matrix.E;
                            Commands[c].Values[i + 1] = (matrix.B * Commands[c].Values[i]) + (matrix.D * Commands[c].Values[i + 1]) + matrix.F;
                            newDAttributeValue += " " + Commands[c].Values[i].ToString() + " " + Commands[c].Values[i + 1].ToString();
                        }
                    }
                    else if (Commands[c].Name == Command.CommandName.V ||
                             Commands[c].Name == Command.CommandName.v)
                    {
                        for (int i = 0; i < Commands[c].Values.Length; i++)
                        {
                            Commands[c].Values[i] = (matrix.B * 0) + (matrix.D * Commands[c].Values[i]) + matrix.F;
                            newDAttributeValue += " " + Commands[c].Values[i].ToString();
                        }
                    }
                    else if (Commands[c].Name == Command.CommandName.H ||
                             Commands[c].Name == Command.CommandName.h)
                    {
                        for (int i = 0; i < Commands[c].Values.Length; i++)
                        {
                            Commands[c].Values[i] = (matrix.A * Commands[c].Values[i]) + (matrix.C * 0) + matrix.E;
                            newDAttributeValue += " " + Commands[c].Values[i].ToString();
                        }
                    }
                    //else if (Commands[c].Name == Command.CommandName.Z ||
                    //         Commands[c].Name == Command.CommandName.z)
                    //{
                    //    newDAttributeValue += " " + Commands[c].Name.ToString();
                    //}
                }

                Node.Attributes["d"] = newDAttributeValue;
                Node.RemoveAttribute("transform");
            }
        }

        //public void RemoveZeroChangeCommands(float errorDeviation = 0.05f) // удаляет команды, которые никак не отрисовываются (нулевой длины (точка), одна и таже координата для всей команды и т.п.)
        //{
        //    List<Command> newCommands = new List<Command>();
        //    foreach (var commandsGroup in Commands.GroupBy(c => c.Name))
        //    {
        //        foreach (var command in commandsGroup)
        //        {
        //            bool equal = false;
        //            Point[] points = command.GetPoints();
        //            if (points.Length > 1)
        //            {
        //                equal = true;
        //                Point firstPoint = points[0];
        //                for (int i = 1; i < points.Length; i++)
        //                {
        //                    if (Point.GetDistance(firstPoint, points[i]) > errorDeviation)
        //                    {
        //                        equal = false;
        //                        break;
        //                    }
        //                }
        //            }
        //            if (!equal)
        //            {
        //                newCommands.Add(command);
        //            }
        //        }
        //    }
        //    Commands = newCommands.OrderBy(c => c.Order).ToArray();
        //}

        public System.Drawing.RectangleF GetBounds()
        {
            float left, right, top, bottom;
            float minLeft = float.MaxValue;
            float maxRight = float.MinValue;
            float minTop = float.MaxValue;
            float maxBottom = float.MinValue;
            foreach (var command in Commands.Where(c => c.Values.Count() > 0))
            {
                left = command.GetPoints().Aggregate((c1, c2) => c1.X < c2.X ? c1 : c2).X;
                right = command.GetPoints().Aggregate((c1, c2) => c1.X > c2.X ? c1 : c2).X;
                top = command.GetPoints().Aggregate((c1, c2) => c1.Y < c2.Y ? c1 : c2).Y;
                bottom = command.GetPoints().Aggregate((c1, c2) => c1.Y > c2.Y ? c1 : c2).Y;

                if (left < minLeft)
                {
                    minLeft = left;
                }

                if (right > maxRight)
                {
                    maxRight = right;
                }

                if (top < minTop)
                {
                    minTop = top;
                }

                if (bottom > maxBottom)
                {
                    maxBottom = bottom;
                }
            }

            return new System.Drawing.RectangleF(minLeft, minTop, maxRight - minLeft, maxBottom - minTop);
        }

        public static System.Drawing.RectangleF GetBounds(List<Path> paths)
        {
            float left, right, top, bottom;
            float minLeft = float.MaxValue;
            float maxRight = float.MinValue;
            float minTop = float.MaxValue;
            float maxBottom = float.MinValue;
            foreach (var path in paths)
            {
                foreach (var command in path.Commands.Where(c => c.Values.Count() > 0))
                {
                    left = command.GetPoints().Aggregate((c1, c2) => c1.X < c2.X ? c1 : c2).X;
                    right = command.GetPoints().Aggregate((c1, c2) => c1.X > c2.X ? c1 : c2).X;
                    top = command.GetPoints().Aggregate((c1, c2) => c1.Y < c2.Y ? c1 : c2).Y;
                    bottom = command.GetPoints().Aggregate((c1, c2) => c1.Y > c2.Y ? c1 : c2).Y;

                    if (left < minLeft)
                    {
                        minLeft = left;
                    }

                    if (right > maxRight)
                    {
                        maxRight = right;
                    }

                    if (top < minTop)
                    {
                        minTop = top;
                    }

                    if (bottom > maxBottom)
                    {
                        maxBottom = bottom;
                    }
                }
            }

            return new System.Drawing.RectangleF(minLeft, minTop, maxRight - minLeft, maxBottom - minTop);
        }
    }
}
