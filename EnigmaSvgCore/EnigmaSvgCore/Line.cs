using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnigmaSvgCore
{
    public class Line
    {
        public int Id { get; set; } = -1;
        public List<Path> Paths { get; set; } = null;
        public Point Point1 { get; set; }
        public Point Point2 { get; set; }
        public float Length { get; set; }
        
        public Line(Point point1, Point point2, int id = -1)
        {
            Point1 = new Point(point1.X, point1.Y);
            Point2 = new Point(point2.X, point2.Y);
            Length = Point.GetDistance(Point1, Point2);
            Id = id;
        }

        public Line(Point point1, Point point2, List<Path> paths, int id = -1)
        {
            Point1 = new Point(point1.X, point1.Y);
            Point2 = new Point(point2.X, point2.Y);
            Length = Point.GetDistance(Point1, Point2);
            if (paths != null)
            {
                Paths = new List<Path>(paths);
            }
            Id = id;
        }

        public RectangleF GetBounds()
        {
            float left = Point1.X < Point2.X ? Point1.X : Point2.X;
            float right = Point1.X < Point2.X ? Point2.X : Point1.X;
            float top = Point1.Y < Point2.Y ? Point1.Y : Point2.Y;
            float bottom = Point1.Y < Point2.Y ? Point2.Y : Point1.Y;
            RectangleF bounds = new RectangleF(left, top, right - left, bottom - top);
            return bounds;
        }

        public bool IsVertical(float deviationRatio = 1.0f) // допустимый процент от длины линии
        {
            var length = Math.Abs(Point2.Y - Point1.Y);
            if (Math.Abs(Point2.X - Point1.X) <= (length / 100.0f) * deviationRatio)
            {
                return true;
            }
            return false;
        }

        public bool IsHorisontal(float deviationRatio = 1.0f) // допустимый процент от длины линии
        {
            var length = Math.Abs(Point2.X - Point1.X);
            if (Math.Abs(Point2.Y - Point1.Y) <= (length / 100.0f) * deviationRatio)
            {
                return true;
            }
            return false;
        }

        public static List<Line> ConcatenateHLines(List<Line> hLines, RectangleF targetArea, float continuousLineMinLength = 5.0f, float leanRatio = 2.0f, float allowableBreakSpacing = 2.0f, float maxDistance = 1.0f, float noiseFilter = 3.0f)
        {
            var horizontalLines = hLines.AsParallel().Where(l => l.IsHorisontal(leanRatio)).ToList();
            horizontalLines.AsParallel().ForAll(f => f.FixHLineDirection());

            if (!targetArea.IsEmpty)
            {
                horizontalLines = horizontalLines.AsParallel().Where(l => 
                l.Point1.Y >= targetArea.Top && l.Point1.Y <= targetArea.Bottom && // Y
                ((l.Point1.X >= targetArea.Left && l.Point1.X <= targetArea.Right) || (l.Point2.X >= targetArea.Left && l.Point2.X <= targetArea.Right) || // левая или правая точка внутри границы
                (l.Point1.X < targetArea.Left && l.Point2.X > targetArea.Right)) // обе точки за границами
                ).ToList();
                horizontalLines.AsParallel().ForAll(a =>
                {
                    if (a.Point1.X < targetArea.Left)
                    {
                        a.Point1.X = targetArea.Left;
                    }
                    if (a.Point2.X > targetArea.Right)
                    {
                        a.Point2.X = targetArea.Right;
                    }
                    a.Length = a.Point2.X - a.Point1.X;
                });
            }

            horizontalLines = horizontalLines.Where(l => l.Length >= noiseFilter).ToList(); // Filter noise-lines

            if (horizontalLines.Count < 2)
            {
                return horizontalLines.Where(l => l.Length >= continuousLineMinLength).ToList();
            }
            else
            {
                //#region линии, сгруппированные по Y, упорядоченные по X в каждой группе
                //var linesGroupedByY_OrderedByX = horizontalLines.GroupBy(v => v.Point1.Y, (key, group) => new { Y = key, Lines = group.OrderBy(x => x.Point1.X).ToList() }).ToList();
                //#endregion

                //#region новые "склеенные" линии в каждой группе Y с учетом допустимого "разрыва" между ними, упорядоченные по X в каждой группе
                //Dictionary<float, List<Line>> newLinesGroupedByY_OrderedByX = new Dictionary<float, List<Line>>();
                //object _lock = new object();

                //linesGroupedByY_OrderedByX.AsParallel().ForAll(a =>
                //{
                //    var xDistancesForY_group = a.Lines.Skip(1).Zip(a.Lines, (rightLine, leftLine) => rightLine.Point1.X - leftLine.Point2.X > allowableBreakSpacing ? new { Left = leftLine, Right = rightLine, Spacing = rightLine.Point1.X - leftLine.Point2.X } : null).Where(v => v != null).OrderBy(v => v.Left.Point1.X).ToList(); // "разрывы" между линиями одной группы
                //    if (xDistancesForY_group.Count == 0) // если "разрывов" нет, считаем данный набор линий одной линией
                //    {
                //        lock (_lock)
                //        {
                //            newLinesGroupedByY_OrderedByX.Add(a.Y, new List<Line> { new Line(new Point(a.Lines.First().Point1.X, a.Lines.First().Point1.Y), new Point(a.Lines.Last().Point2.X, a.Lines.Last().Point2.Y)) });
                //        }
                //    }
                //    else // есть как минимум один "разрыв"
                //    {
                //        List<Line> lines = new List<Line>();
                //        var leftLines = a.Lines.Where(v => v.Point2.X <= xDistancesForY_group.First().Left.Point2.X).OrderBy(l => l.Point1.X).ToList(); // все линии левее первого "разрыва" между группами
                //        lines.Add(new Line(new Point(leftLines.First().Point1.X, leftLines.First().Point1.Y), new Point(leftLines.Last().Point2.X, leftLines.Last().Point2.Y)));

                //        for (int g = 0; g < xDistancesForY_group.Count - 1; g++)
                //        {
                //            var middleLines = a.Lines.Where(v => v.Point1.X >= xDistancesForY_group[g].Right.Point1.X && v.Point2.X <= xDistancesForY_group[g + 1].Left.Point2.X).OrderBy(l => l.Point1.X).ToList();
                //            lines.Add(new Line(new Point(middleLines.First().Point1.X, middleLines.First().Point1.Y), new Point(middleLines.Last().Point2.X, middleLines.Last().Point2.Y)));
                //        }

                //        var rightLines = a.Lines.Where(v => v.Point1.X >= xDistancesForY_group.Last().Right.Point1.X).OrderBy(l => l.Point1.X).ToList(); // все линии правее последнего "разрыва" между группами
                //        lines.Add(new Line(new Point(rightLines.First().Point1.X, rightLines.First().Point1.Y), new Point(rightLines.Last().Point2.X, rightLines.Last().Point2.Y)));
                //        lock (_lock)
                //        {
                //            newLinesGroupedByY_OrderedByX.Add(a.Y, lines);
                //        }
                //    }
                //});
                //#endregion

                //#region линии, сгруппированные по дистанции между Y с учетом максимально допустимого расстояния
                //newLinesGroupedByY_OrderedByX = newLinesGroupedByY_OrderedByX.OrderBy(v => v.Key).ToDictionary(key => key.Key, value => value.Value.OrderBy(l => l.Point1.X).ToList());
                //var yGroupsBreaks = newLinesGroupedByY_OrderedByX.Skip(1).Zip(newLinesGroupedByY_OrderedByX, (bottomGroup, topGroup) => bottomGroup.Key - topGroup.Key > maxDistance ? new { Top = topGroup, Bottom = bottomGroup, Distance = bottomGroup.Key - topGroup.Key } : null).Where(v => v != null).OrderBy(v => v.Top.Key).ToList(); // "разрывы" по вертикали между Y-группами

                //Dictionary<int, List<Line>> yJoinedLines = new Dictionary<int, List<Line>>();
                //int yJoinsCounter = 0;
                //if (yGroupsBreaks.Count == 0) // если "разрывов" нет, считаем данный набор линий частью одной группы
                //{
                //    yJoinedLines.Add(yJoinsCounter++, newLinesGroupedByY_OrderedByX.SelectMany(v => v.Value).ToList());
                //}
                //else // есть как минимум один "разрыв"
                //{
                //    var topLines = newLinesGroupedByY_OrderedByX.Where(v => v.Key <= yGroupsBreaks.First().Top.Key).SelectMany(v => v.Value).ToList(); // все линии выше первого "Y-разрыва" между группами
                //    yJoinedLines.Add(yJoinsCounter++, topLines);

                //    for (int g = 0; g < yGroupsBreaks.Count - 1; g++)
                //    {
                //        var middleLines = newLinesGroupedByY_OrderedByX.Where(v => v.Key >= yGroupsBreaks[g].Bottom.Key && v.Key <= yGroupsBreaks[g + 1].Top.Key).SelectMany(v => v.Value).ToList();
                //        yJoinedLines.Add(yJoinsCounter++, middleLines);
                //    }

                //    var bottomLines = newLinesGroupedByY_OrderedByX.Where(v => v.Key >= yGroupsBreaks.Last().Bottom.Key).SelectMany(v => v.Value).ToList(); // все линии ниже последнего "Y-разрыва" между группами
                //    yJoinedLines.Add(yJoinsCounter++, bottomLines);
                //}
                //#endregion

                //#region совокупности линий в пределах одной Y-группы, но разделенные по X с учетом максимально допустимого расстояния
                //// из этих совокупностей линий и будут строиться "единые" горизонтальные линии
                //List<Line> concatLines = new List<Line>();
                //yJoinedLines.AsParallel().ForAll(a =>
                //{
                //    var xGroupLinesInY_joinedGroup = a.Value.OrderBy(v => v.Point1.X).ToList();
                //    var xGroupLinesBreaks = xGroupLinesInY_joinedGroup.Skip(1).Zip(xGroupLinesInY_joinedGroup, (rightGroup, leftGroup) => rightGroup.Point1.X - leftGroup.Point2.X > allowableBreakSpacing ? new { Left = leftGroup, Right = rightGroup, Spacing = rightGroup.Point1.X - leftGroup.Point2.X } : null).Where(v => v != null).OrderBy(v => v.Left.Point1.X).ToList(); // "разрывы" по горизонтали между X-группами внутри Y-группы

                //    Dictionary<int, List<Line>> xyScopeLines = new Dictionary<int, List<Line>>();
                //    int xyScopesCounter = 0;
                //    if (xGroupLinesBreaks.Count == 0) // если "разрывов" нет, считаем данный набор линий частью одной совокупности линий для объединения в "единую" линию
                //    {
                //        xyScopeLines.Add(xyScopesCounter++, xGroupLinesInY_joinedGroup);
                //    }
                //    else // есть как минимум один "разрыв"
                //    {
                //        var leftLines = xGroupLinesInY_joinedGroup.Where(v => v.Point2.X <= xGroupLinesBreaks.First().Left.Point2.X).ToList(); // все линии левее первого "X-разрыва" между группами
                //        xyScopeLines.Add(xyScopesCounter++, leftLines);

                //        for (int g = 0; g < yGroupsBreaks.Count - 1; g++)
                //        {
                //            var middleLines = xGroupLinesInY_joinedGroup.Where(v => v.Point1.X >= xGroupLinesBreaks[g].Right.Point1.X && v.Point2.X <= xGroupLinesBreaks[g + 1].Right.Point2.X).ToList();
                //            xyScopeLines.Add(xyScopesCounter++, middleLines);
                //        }

                //        var rightLines = xGroupLinesInY_joinedGroup.Where(v => v.Point1.X >= xGroupLinesBreaks.Last().Right.Point1.X).ToList(); // все линии правее последнего "X-разрыва" между группами
                //        xyScopeLines.Add(xyScopesCounter++, rightLines);
                //    }

                //    for (int i = 0; i < xyScopeLines.Count; i++) // проходим по всем "скоплениям" линий, из которых необходимо создать одну "единую" линию
                //    {
                //        //float y = xyScopeLines[i].Aggregate((l1, l2) => l1.Length > l2.Length ? l1 : l2).Point1.Y; // за основу берется Y-координата самой длинной линии, чтобы нивелировать шум от мелких линий
                //        //var setLines = xyScopeLines[i].OrderBy(l => l.Point1.X);
                //        //float x1 = setLines.First().Point1.X;
                //        //float x2 = setLines.Last().Point2.X;
                //        //Line resultHLine = new Line(new Point(x1, y), new Point(x2, y));
                //        lock (_lock)
                //        {
                //            concatLines.AddRange(xyScopeLines[i]);
                //        }
                //    }
                //});
                //#endregion

                //var newSolidLinesGroupedByY_OrderedByX
                //var xDistancesForY_group = yGroupedLines[i].Skip(1).Zip(yGroupedLines[i], (rightLine, leftLine) => rightLine.Point1.X - leftLine.Point2.X > allowableBreakSpacing ? new { Left = leftLine, Right = rightLine, Spacing = rightLine.Point1.X - leftLine.Point2.X } : null).Where(v => v != null).OrderBy(v => v.Left.Point1.X).ToList(); // "разрывы" между линиями одной группы
                //var gLines = yLines.Where(i => i.Index <= groupsYDistances.First().Top.Index).SelectMany(l => l.Lines).OrderBy(l => l.Point1.X).ToList(); // все линии выше первого "разрыва" между группами
                //yGroupedLines.Add(gCounter++, gLines);

                //for (int g = 0; g < groupsYDistances.Count - 1; g++)
                //{
                //    gLines = yLines.Where(i => i.Index >= groupsYDistances[g].Bottom.Index && i.Index <= groupsYDistances[g + 1].Top.Index).SelectMany(l => l.Lines).OrderBy(l => l.Point1.X).ToList();
                //    yGroupedLines.Add(gCounter++, gLines);
                //}

                //gLines = yLines.Where(i => i.Index >= groupsYDistances.Last().Bottom.Index).SelectMany(l => l.Lines).OrderBy(l => l.Point1.X).ToList(); // все линии ниже последнего "разрыва" между группами
                //yGroupedLines.Add(gCounter++, gLines);

                //int yIndexCounter = 0;
                //var linesGroupedByY_OrderedByX = horizontalLines.GroupBy(v => v.Point1.Y, (key, group) => new { Y = key, Lines = group.OrderBy(x => x.Point1.X).ToList() }).OrderBy(y => y.Y).Select(r => new { Index = yIndexCounter++, Y = r.Y, Lines = r.Lines }).OrderBy(v => v.Index).ToList();
                //#endregion

                int indexesCounter = 0;
                var yLines = horizontalLines.GroupBy(v => v.Point1.Y, (key, group) => new { Y = key, Lines = group.OrderBy(x => x.Point1.X).ToList() }).OrderBy(y => y.Y).Select(r => new { Index = indexesCounter++, Y = r.Y, Lines = r.Lines }).OrderBy(v => v.Index).ToList(); // сгруппированные по Y
                var groupsYDistances = yLines.Skip(1).Zip(yLines, (bottomGroup, topGroup) => bottomGroup.Y - topGroup.Y > maxDistance ? new { Top = topGroup, Bottom = bottomGroup, Deviation = bottomGroup.Y - topGroup.Y } : null).Where(v => v != null).OrderBy(v => v.Top.Index).ToList(); // "разрывы" между группами

                Dictionary<int, List<Line>> yGroupedLines = new Dictionary<int, List<Line>>();
                int gCounter = 0;
                List<Line> gLines = new List<Line>();

                gLines = yLines.Where(i => i.Index <= groupsYDistances.First().Top.Index).SelectMany(l => l.Lines).OrderBy(l => l.Point1.X).ToList(); // все линии выше первого "разрыва" между группами
                yGroupedLines.Add(gCounter++, gLines);

                for (int g = 0; g < groupsYDistances.Count - 1; g++)
                {
                    gLines = yLines.Where(i => i.Index >= groupsYDistances[g].Bottom.Index && i.Index <= groupsYDistances[g + 1].Top.Index).SelectMany(l => l.Lines).OrderBy(l => l.Point1.X).ToList();
                    yGroupedLines.Add(gCounter++, gLines);
                }

                gLines = yLines.Where(i => i.Index >= groupsYDistances.Last().Bottom.Index).SelectMany(l => l.Lines).OrderBy(l => l.Point1.X).ToList(); // все линии ниже последнего "разрыва" между группами
                yGroupedLines.Add(gCounter++, gLines);


                List<Line> concatLines = new List<Line>();

                // [NEW]
                //for (int i = 0; i < yGroupedLines.Count; i++) // склейка линий (единые линии из отдельных линий)
                //{
                //    var groupsXDistances = yGroupedLines[i].Skip(1).Zip(yGroupedLines[i], (rightLine, leftLine) => rightLine.Point1.X - leftLine.Point2.X > allowableBreakSpacing ? new { Left = leftLine, Right = rightLine, Spacing = rightLine.Point1.X - leftLine.Point2.X } : null).Where(v => v != null).OrderBy(v => v.Left.Point1.X).ToList(); // "разрывы" между линиями одной группы

                //    if (groupsXDistances.Count != 0)
                //    {
                //        var lines1 = yGroupedLines[i].Where(v => v.Point2.X <= groupsXDistances.First().Left.Point2.X).OrderBy(l => l.Point1.X).ToList(); // все линии левее первого "разрыва" между группами
                //        concatLines.Add(new Line(new Point(lines1.First().Point1.X, lines1.First().Point1.Y), new Point(lines1.Last().Point2.X, lines1.Last().Point2.Y)));

                //        for (int g = 0; g < groupsXDistances.Count - 1; g++)
                //        {
                //            gLines = yGroupedLines[i].Where(v => v.Point1.X >= groupsXDistances[g].Right.Point1.X && v.Point2.X <= groupsXDistances[g + 1].Left.Point2.X).OrderBy(l => l.Point1.X).ToList();
                //            concatLines.Add(new Line(new Point(gLines.First().Point1.X, gLines.First().Point1.Y), new Point(gLines.Last().Point2.X, gLines.Last().Point2.Y)));
                //        }

                //        var lines2 = yGroupedLines[i].Where(v => v.Point1.X >= groupsXDistances.Last().Right.Point1.X).OrderBy(l => l.Point1.X).ToList(); // все линии правее последнего "разрыва" между группами
                //        concatLines.Add(new Line(new Point(lines2.First().Point1.X, lines2.First().Point1.Y), new Point(lines2.Last().Point2.X, lines2.Last().Point2.Y)));
                //    }
                //    else
                //    {
                //        var lines = yGroupedLines[i].OrderBy(o => o.Point1.X);
                //        concatLines.Add(new Line(new Point(lines.First().Point1.X, lines.First().Point1.Y), new Point(lines.Last().Point2.X, lines.Last().Point2.Y)));
                //    }
                //}

                // [OLD]
                foreach (var item in yGroupedLines) // склейка линий (единые линии из отдельных линий)
                {
                    var leftLine = item.Value.First();
                    if (item.Value.Count < 2)
                    {
                        concatLines.Add(leftLine);
                    }
                    else
                    {
                        for (int i = 1; i < item.Value.Count; i++)
                        {
                            var rightLine = item.Value[i];
                            if (rightLine.Point1.X - leftLine.Point2.X <= allowableBreakSpacing)
                            {
                                if (leftLine.Point2.X <= rightLine.Point2.X)
                                {
                                    leftLine.Point2.X = rightLine.Point2.X;
                                    //leftLine.Point2.Y = rightLine.Point2.Y; // можно оставить Y без изменений и тогда наклон не будет применен, а линия останется строго горизонтальной (в этом случае Y будет взят от первой линии в группе)
                                    leftLine.Length = Point.GetDistance(leftLine.Point1, leftLine.Point2);
                                    leftLine.Paths.AddRange(rightLine.Paths);
                                }

                                if (i == item.Value.Count - 1)
                                {
                                    concatLines.Add(leftLine);
                                }
                            }
                            else
                            {
                                concatLines.Add(leftLine);
                                if (i == item.Value.Count - 1)
                                {
                                    concatLines.Add(rightLine);
                                }
                                leftLine = rightLine;
                            }
                        }
                    }
                }
                return concatLines.Where(l => l.Length >= continuousLineMinLength).ToList();
            }
        }

        public static List<Line> ConcatenateVLines(List<Line> vLines, RectangleF targetArea, float continuousLineMinLength = 5.0f, float leanRatio = 2.0f, float allowableBreakSpacing = 2.0f, float maxDistance = 1.0f, float noiseFilter = 3.0f)
        {
            var verticalLines = vLines.AsParallel().Where(l => l.IsVertical(leanRatio)).ToList();
            verticalLines.AsParallel().ForAll(f => f.FixVLineDirection());

            if (!targetArea.IsEmpty)
            {
                verticalLines = verticalLines.AsParallel().Where(l =>
                l.Point1.X >= targetArea.Left && l.Point1.X <= targetArea.Right && // X
                ((l.Point1.Y >= targetArea.Top && l.Point1.Y <= targetArea.Bottom) || (l.Point2.Y >= targetArea.Top && l.Point2.Y <= targetArea.Bottom) || // верхняя или нижняя точка внутри границы
                (l.Point1.Y < targetArea.Top && l.Point2.Y > targetArea.Bottom)) // обе точки за границами
                ).ToList();
                verticalLines.AsParallel().ForAll(a =>
                {
                    if (a.Point1.Y < targetArea.Top)
                    {
                        a.Point1.Y = targetArea.Top;
                    }
                    if (a.Point2.Y > targetArea.Bottom)
                    {
                        a.Point2.Y = targetArea.Bottom;
                    }
                    a.Length = a.Point2.Y - a.Point1.Y;
                });
            }

            verticalLines = verticalLines.Where(l => l.Length >= noiseFilter).ToList(); // Filter noise-lines

            if (verticalLines.Count < 2)
            {
                return verticalLines.Where(l => l.Length >= continuousLineMinLength).ToList();
            }
            else
            {
                int indexesCounter = 0;
                var xLines = verticalLines.GroupBy(v => v.Point1.X, (key, group) => new { X = key, Lines = group.OrderBy(x => x.Point1.Y).ToList() }).OrderBy(x => x.X).Select(r => new { Index = indexesCounter++, X = r.X, Lines = r.Lines }).OrderBy(v => v.Index).ToList(); // сгруппированные по Y
                var groupsXDistances = xLines.Skip(1).Zip(xLines, (rightGroup, leftGroup) => rightGroup.X - leftGroup.X > maxDistance ? new { Left = leftGroup, Right = rightGroup, Deviation = rightGroup.X - leftGroup.X } : null).Where(v => v != null).OrderBy(v => v.Left.Index).ToList(); // "разрывы" между группами

                Dictionary<int, List<Line>> xGroupedLines = new Dictionary<int, List<Line>>();
                int gCounter = 0;
                List<Line> gLines = new List<Line>();

                gLines = xLines.Where(i => i.Index <= groupsXDistances.First().Left.Index).SelectMany(l => l.Lines).OrderBy(l => l.Point1.Y).ToList(); // все линии левее первого "разрыва" между группами
                xGroupedLines.Add(gCounter++, gLines);

                for (int g = 0; g < groupsXDistances.Count - 1; g++)
                {
                    gLines = xLines.Where(i => i.Index >= groupsXDistances[g].Right.Index && i.Index <= groupsXDistances[g + 1].Left.Index).SelectMany(l => l.Lines).OrderBy(l => l.Point1.Y).ToList();
                    xGroupedLines.Add(gCounter++, gLines);
                }

                gLines = xLines.Where(i => i.Index >= groupsXDistances.Last().Right.Index).SelectMany(l => l.Lines).OrderBy(l => l.Point1.Y).ToList(); // все линии правее последнего "разрыва" между группами
                xGroupedLines.Add(gCounter++, gLines);

                List<Line> concatLines = new List<Line>();
                foreach (var item in xGroupedLines) // склейка линий (единые линии из отдельных линий)
                {
                    var topLine = item.Value.First();
                    if (item.Value.Count < 2)
                    {
                        concatLines.Add(topLine);
                    }
                    else
                    {
                        for (int i = 1; i < item.Value.Count; i++)
                        {
                            var bottomLine = item.Value[i];
                            if (bottomLine.Point1.Y - topLine.Point2.Y <= allowableBreakSpacing)
                            {
                                if (topLine.Point2.Y <= bottomLine.Point2.Y)
                                {
                                    topLine.Point2.Y = bottomLine.Point2.Y;
                                    //topLine.Point2.X = bottomLine.Point2.X; // можно оставить X без изменений и тогда наклон не будет применен, а линия останется строго вертикальной (в этом случае X будет взят от первой линии в группе)
                                    topLine.Length = Point.GetDistance(topLine.Point1, topLine.Point2);
                                    topLine.Paths.AddRange(bottomLine.Paths);
                                }
                                
                                if (i == item.Value.Count - 1)
                                {
                                    concatLines.Add(topLine);
                                }
                            }
                            else
                            {
                                concatLines.Add(topLine);
                                if (i == item.Value.Count - 1)
                                {
                                    concatLines.Add(bottomLine);
                                }
                                topLine = bottomLine;
                            }
                        }
                    }
                }
                return concatLines.Where(l => l.Length >= continuousLineMinLength).ToList();
            }
        }

        public void FixHLineDirection()
        {
            if (Point1.X > Point2.X)
            {
                var tempX = Point1.X;
                Point1.X = Point2.X;
                Point2.X = tempX;
            }
        }

        public void FixVLineDirection()
        {
            if (Point1.Y > Point2.Y)
            {
                var tempY = Point1.Y;
                Point1.Y = Point2.Y;
                Point2.Y = tempY;
            }
        }

        /// <summary>
        /// Находит для горизонтальной линии все вертикальные линии, которые с ней пересекаются
        /// </summary>
        /// <param name="verticalLines">Вертикальные линии</param>
        /// <param name="allowableBreakSpacing">Допустимый "разрыв" между линиями</param>
        /// <returns>Вертикальные линии, пересекающиеся с данной горизонтальной линией</returns>
        public List<Cross> GetVerticalCrossLines(List<Line> verticalLines, float allowableBreakSpacing = 0.0f) // применимо только для горизонтальной линии (для которой метод IsHorisontal() возвращают True)
        {
            float hLeft = Point1.X - allowableBreakSpacing;
            float hRight = Point2.X + allowableBreakSpacing;
            var crossLines = verticalLines.AsParallel().Where(l => l.Point1.X >= hLeft && l.Point1.X <= hRight && Point1.Y >= l.Point1.Y - allowableBreakSpacing && Point1.Y <= l.Point2.Y + allowableBreakSpacing).OrderBy(c => c.Point1.X).ToList();
            
            List<Cross> lineCrosses = new List<Cross>();
            for (int i = 0; i < crossLines.Count; i++)
            {
                lineCrosses.Add(new Cross(this, crossLines[i]));
            }
            return lineCrosses;
        }

        /// <summary>
        /// Находит для вертикальной линии все горизонтальные линии, которые с ней пересекаются
        /// </summary>
        /// <param name="verticalLines">Горизонтальные линии</param>
        /// <param name="allowableBreakSpacing">Допустимый "разрыв" между линиями</param>
        /// <returns>Горизонтальные линии, пересекающиеся с данной вертикальной линией</returns>
        public List<Cross> GetHorizontalCrossLines(List<Line> horizontallines, float allowableBreakSpacing = 0.0f) // применимо только для вертикальной линии (для которой метод IsVertical() возвращают True)
        {
            float vTop = Point1.Y - allowableBreakSpacing;
            float vBottom = Point2.Y + allowableBreakSpacing;
            var crossLines = horizontallines.AsParallel().Where(l => l.Point1.Y >= vTop && l.Point1.Y <= vBottom && Point1.X >= l.Point1.X - allowableBreakSpacing && Point1.X <= l.Point2.X + allowableBreakSpacing).OrderBy(c => c.Point1.Y).ToList();

            List<Cross> lineCrosses = new List<Cross>();
            for (int i = 0; i < crossLines.Count; i++)
            {
                lineCrosses.Add(new Cross(crossLines[i], this));
            }
            return lineCrosses;
        }
    }
}
