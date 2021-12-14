using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnigmaSvgCore
{
    public class DefaultCellsProcessor : ICellsProcessor
    {
        public List<List<Cell>> FindTableCells(List<Cell> allCells, int minCellsInsideTable)
        {
            if (minCellsInsideTable <= 0)
            {
                minCellsInsideTable = 1;
            }
            
            List<Cell> cells = allCells.AsParallel().Where(cell => allCells.Where(c1 => c1.Bounds.Left > cell.Bounds.Left && c1.Bounds.Right < cell.Bounds.Right && c1.Bounds.Top > cell.Bounds.Top && c1.Bounds.Bottom < cell.Bounds.Bottom).Count() == 0).ToList(); // фильтрация рамок на чертежах
            
            object _lock = new object();
            List<List<Cell>> tables = new List<List<Cell>>();

            var topTablesCells = cells.Where(c1 => cells.Where(c2 => c2.BottomLeftCross.HorizontalLine.Id == c1.TopLeftCross.HorizontalLine.Id).Count() == 0)
                                      .GroupBy(c => c.TopLeftCross.HorizontalLine.Id, (key, group) => new
                                      {
                                          Y = key,
                                          TopLeftTablesCells = group.Where(f1 => group.Where(g1 => g1.TopRightCross.VerticalLine.Id == f1.TopLeftCross.VerticalLine.Id).Count() == 0).OrderBy(s1 => s1.Bounds.X).ToList(), // левые ячейки всех таблиц в группе
                                          TopRightTablesCells = group.Where(f2 => group.Where(g2 => g2.TopLeftCross.VerticalLine.Id == f2.TopRightCross.VerticalLine.Id).Count() == 0).OrderBy(s2 => s2.Bounds.X).ToList(), // правые ячейки всех таблиц в группе
                                      })
                                      .ToList();
            topTablesCells.AsParallel().ForAll(a =>
            {
                for (int i = 0; i < a.TopLeftTablesCells.Count; i++) // перебор пар левых и правых граничных ячеек для каждой таблицы
                {
                    try
                    {
                        var top = a.TopLeftTablesCells[i].TopLeftCross.Point.Y;
                        var left = a.TopLeftTablesCells[i].TopLeftCross.Point.X;
                        var right = a.TopRightTablesCells[i].Bounds.Right;
                        var cells1 = cells.AsReadOnly().Where(c1 => c1.BottomLeftCross.VerticalLine.Id == a.TopLeftTablesCells[i].BottomLeftCross.VerticalLine.Id);
                        var cells2 = cells.AsReadOnly().Where(c2 => c2.BottomRightCross.VerticalLine.Id == a.TopRightTablesCells[i].BottomRightCross.VerticalLine.Id);
                        var bottom = (from bottom1 in cells1
                                      from bottom2 in cells2
                                      where bottom1.BottomLeftCross.HorizontalLine.Id == bottom2.BottomRightCross.HorizontalLine.Id
                                      select bottom1
                                     ).OrderByDescending(v => v.BottomLeftCross.HorizontalLine.Point1.Y).First().BottomRightCross.HorizontalLine.Point2.Y;

                        var tablesCells = cells.AsReadOnly().Where(c => c.Bounds.Left >= left && c.Bounds.Top >= top && c.Bounds.Right <= right && c.Bounds.Bottom <= bottom).ToList();

                        if (tablesCells.Count >= minCellsInsideTable)
                        {
                            lock (_lock)
                            {
                                tables.Add(tablesCells);
                            }
                        }
                    }
                    catch
                    {
                        Debug.WriteLine("Кривая таблица 0_о или неведомый баг...");
                    }
                }
            });

            return tables;
        }
    }

    public class CustomCellsProcessor : ICellsProcessor
    {
        public List<List<Cell>> FindTableCells(List<Cell> cells, int minCellsInsideTable)
        {
            #region ТВОЯ РЕАЛИЗАЦИЯ ФОРМИРОВАНИЯ ТАБЛИЦ ИЗ ЯЧЕЕК
            // TO DO:
            // implement your custom amazing solution here :)
            // ...
            throw new NotImplementedException(); // не забудь это удалить ;)
            #endregion
        }
    }
}
