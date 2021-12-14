using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnigmaSvgCore
{
    public class Table
    {
        public int Id { get; set; } = -1;
        public Svg Svg { get; }
        public RectangleF Bounds { get; }
        private Cell ActiveCell { get; set; }
        public List<Cell> Cells { get; set; }
        public Cell[] LeftCells { get; }
        public Cell[] RightCells { get; }
        public Cell[] TopCells { get; }
        public Cell[] BottomCells { get; }
        public Table(Svg svg, List<Cell> cells, int id = -1)
        {
            if (cells.Count == 0)
            {
                throw new ArgumentException("Таблица должна состоять по меньшей мере из одной ячейки.", nameof(cells));
            }
            Svg = svg;
            Id = id;
            Cells = new List<Cell>();
            Cells.AddRange(cells);
            TopCells = cells.GroupBy(g => g.TopLeftCross.Point.Y, (key, group) => new { Y = key, Cells = group.OrderBy(x => x.TopLeftCross.Point.X).ToList() }).OrderBy(y => y.Y).First().Cells.ToArray();
            LeftCells = cells.AsParallel().Where(c => c.TopLeftCross.VerticalLine.Id == TopCells.First().TopLeftCross.VerticalLine.Id).OrderBy(y => y.TopLeftCross.Point.Y).ToArray();
            RightCells = cells.AsParallel().Where(c => c.TopRightCross.VerticalLine.Id == TopCells.Last().TopRightCross.VerticalLine.Id).OrderBy(y => y.TopRightCross.Point.Y).ToArray();
            BottomCells = cells.AsParallel().Where(c => c.BottomLeftCross.HorizontalLine.Id == LeftCells.Last().BottomLeftCross.HorizontalLine.Id).OrderBy(x => x.BottomLeftCross.Point.X).ToArray();

            ActiveCell = TopCells.First();
            Bounds = new RectangleF(TopCells.First().Bounds.Left, TopCells.First().Bounds.Top, TopCells.Last().Bounds.Right - TopCells.First().Bounds.Left, BottomCells.First().Bounds.Bottom - TopCells.First().Bounds.Top);
        }

        public Content GetHeader(float height = 50.0f, float symbolsDistortion = 0.2f, Margins margins = null)
        {
            float top = Bounds.Top - height;
            float bottom = Bounds.Top;
            float left = Bounds.Left;
            float right = Bounds.Right;
            RectangleF hBounds = new RectangleF(left, top, right - left, bottom - top);
            if (margins != null)
            {
                hBounds.X += margins.Left;
                hBounds.Y += margins.Top;
                hBounds.Width -= margins.Left - margins.Right;
                hBounds.Height -= margins.Top - margins.Left;
            }
            return new Content(Svg.GetWords(false, symbolsDistortion, hBounds));
        }

        public Content GetFooter(float depth = 25.0f, float symbolsDistortion = 0.2f, Margins margins = null)
        {
            float top = Bounds.Bottom;
            float bottom = Bounds.Bottom + depth;
            float left = Bounds.Left;
            float right = Bounds.Right;
            RectangleF hBounds = new RectangleF(left, top, right - left, bottom - top);
            if (margins != null)
            {
                hBounds.X += margins.Left;
                hBounds.Y += margins.Top;
                hBounds.Width -= margins.Left - margins.Right;
                hBounds.Height -= margins.Top - margins.Left;
            }
            return new Content(Svg.GetWords(false, symbolsDistortion, hBounds));
        }

        public Content GetAllContent(Margins margins = null, bool hidden = false, float symbolsDistortion = 0.2f)
        {
            RectangleF bounds = Bounds;
            if (margins != null)
            {
                bounds = new RectangleF(bounds.Left + margins.Left, bounds.Top + margins.Top, bounds.Right - bounds.Left - margins.Left - margins.Right, bounds.Bottom - bounds.Top - margins.Top - margins.Bottom);
            }
            
            return new Content(Svg.GetWords(hidden, symbolsDistortion, bounds));
        }

        public Cell GetTopLeftCell()
        {
            return TopCells.First();
        }

        public Cell GetTopRightCell()
        {
            return TopCells.Last();
        }

        public Cell GetBottomLeftCell()
        {
            return BottomCells.First();
        }

        public Cell GetBottomRightCell()
        {
            return BottomCells.Last();
        }

        public Cell GetActiveCell()
        {
            return ActiveCell;
        }

        public Cell SetActiveCell(Cell cell)
        {
            ActiveCell = cell;
            return ActiveCell;
        }

        public Cell[] GetTopNeighborCells(Cell cell)
        {
            return Cells.Where(c => c.BottomLeftCross.HorizontalLine.Id == cell.TopLeftCross.HorizontalLine.Id && 
                              ((c.Bounds.Left >= cell.Bounds.Left && c.Bounds.Left <= cell.Bounds.Right) || (c.Bounds.Right >= cell.Bounds.Left && c.Bounds.Right <= cell.Bounds.Right)))
                        .OrderBy(v => v.Bounds.X).ToArray();
        }

        public Cell[] GetBottomNeighborCells(Cell cell)
        {
            return Cells.Where(c => c.TopLeftCross.HorizontalLine.Id == cell.BottomLeftCross.HorizontalLine.Id &&
                              ((c.Bounds.Left >= cell.Bounds.Left && c.Bounds.Left <= cell.Bounds.Right) || (c.Bounds.Right >= cell.Bounds.Left && c.Bounds.Right <= cell.Bounds.Right)))
                        .OrderBy(v => v.Bounds.X).ToArray();
        }

        public Cell[] GetLeftNeighborCells(Cell cell)
        {
            return Cells.Where(c => c.TopRightCross.VerticalLine.Id == cell.TopLeftCross.VerticalLine.Id &&
                              ((c.Bounds.Top >= cell.Bounds.Top && c.Bounds.Top <= cell.Bounds.Bottom) || (c.Bounds.Bottom >= cell.Bounds.Top && c.Bounds.Bottom <= cell.Bounds.Bottom)))
                        .OrderBy(v => v.Bounds.Y).ToArray();
        }

        public Cell[] GetRightNeighborCells(Cell cell)
        {
            return Cells.Where(c => c.TopLeftCross.VerticalLine.Id == cell.TopRightCross.VerticalLine.Id &&
                              ((c.Bounds.Top >= cell.Bounds.Top && c.Bounds.Top <= cell.Bounds.Bottom) || (c.Bounds.Bottom >= cell.Bounds.Top && c.Bounds.Bottom <= cell.Bounds.Bottom)))
                        .OrderBy(v => v.Bounds.Y).ToArray();
        }

        public List<Cell> GetAllNeighborCells(Cell cell)
        {
            List<Cell> cells = new List<Cell>();
            cells.AddRange(GetTopNeighborCells(cell));
            cells.AddRange(GetBottomNeighborCells(cell));
            cells.AddRange(GetLeftNeighborCells(cell));
            cells.AddRange(GetRightNeighborCells(cell));
            return cells.Distinct().ToList();
        }

        public Cell MoveRight(int rightCellIndex = 0)
        {
            try
            {
                ActiveCell = GetRightNeighborCells(ActiveCell)[rightCellIndex];
                return ActiveCell;
            }
            catch
            {
                return null;
            }
        }

        public Cell MoveLeft(int leftCellIndex = 0)
        {
            try
            {
                ActiveCell = GetLeftNeighborCells(ActiveCell)[leftCellIndex];
                return ActiveCell;
            }
            catch
            {
                return null;
            }
        }

        public Cell MoveUp(int upCellIndex = 0)
        {
            try
            {
                ActiveCell = GetTopNeighborCells(ActiveCell)[upCellIndex];
                return ActiveCell;
            }
            catch
            {
                return null;
            }
        }

        public Cell MoveDown(int downCellIndex = 0)
        {
            try
            {
                ActiveCell = GetTopNeighborCells(ActiveCell)[downCellIndex];
                return ActiveCell;
            }
            catch
            {
                return null;
            }
        }
    }
}
