using EnigmaSvgCore;
using RdLibrary.LocalServices;
using RDLibrary.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static RDLibrary.Models.State;

namespace RDLibrary
{
    public class FileProcessor
    {
        public const string dir = @"D:\VS\AGCC_RD\DemoProject\client-app\public";

        public async Task<AttributesModel> GetFileAttributes(byte[] pdfBytes, string link)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            List<string> errStatus = new List<string>
            {
                "N/D",
                "warning",
                "-1",
                "0001-01-01T00:00:00"
            };
            MemoryStream stream = new MemoryStream();
            stream.Write(pdfBytes, 0, pdfBytes.Length);
            Svg svg = new Svg(stream);
            AttributesModel model = new AttributesModel();
            List<Cell> errorCells = new List<Cell>();
            List<Element> elements = new List<Element>();
            Table table = null;

            try
            {
                //Поиск ячеек штампа
                System.Drawing.RectangleF targetArea1 = new System.Drawing.RectangleF(svg.Width - svg.Width / 2, svg.Height - svg.Height / 2, svg.Width, svg.Height);                
                List<Cell> cells = svg.GetCells(targetArea1).AsParallel()
                    .Where(r => r.Bounds.Height > 1.5 && r.Bounds.X > 1.8)
                    .ToList();                
                //Группировка по строкам 
                var tables = svg.GetTables(cells);
                if(tables.Count() > 0)
                {
                    table = tables.Aggregate((t1, t2) => t1.Cells.Count() > t2.Cells.Count() ? t1 : t2);
                    var footerCells = svg.GetWords(false, 0.02f, new System.Drawing.RectangleF(table.Bounds.X, table.Bounds.Bottom, table.Bounds.Width, svg.Height));
                    var footerRow = footerCells.Count() > 0 ? footerCells : null;
                    var rightCells = table.RightCells.OrderByDescending(r=>r.Bounds.Y).ToArray();
                    elements.Add(new Element
                    { 
                        Name = "FooterName",
                        Cell = null,
                        Content = Regex.Match(footerRow?.First()?.Value ?? "N/D", @"AGCC.*\.[a-z]+")?.Value ?? "N/D"
                    });
                    elements.Add(new Element
                    {
                        Name = "PapeSize",
                        Cell = null,
                        Content = Regex.Match(footerRow?.First()?.Value ?? "N/D", @"AGCC.*\.[a-z]+")?.Value ?? "N/D"
                    });
                    elements.Add(new Element
                    {
                        Name = "TotalSheets",
                        Cell = rightCells?[1],
                        Content = Int32.TryParse(rightCells?[1].GetContent()?.Value ?? "N/D", out int sheets) ? sheets : -1
                    });
                    elements.Add(new Element
                    {
                        Name = "ContractorName",
                        Cell = rightCells?[3],
                        Content = rightCells?[3].GetContent()?.Value ?? "N/D"
                    });
                    elements.Add(new Element
                    {
                        Name = "FileName",
                        Cell = rightCells?[4],
                        Content = rightCells?[4].GetContent()?.Value ?? "N/D"
                    });
                    elements.Add(new Element
                    {
                        Name = "Rev",
                        Cell = rightCells?[5],
                        Content = rightCells?[5].GetContent()?.Value ?? "N/D"
                    });
                    //model.FooterName = footerRow.Count() > 0 ? Regex.Match(footerRow.First().Value, @"AGCC.*\.[a-z]+").Value : Error.error.ToString();
                    //model.PapeSize = footerRow.Count() > 0 ?Regex.Match(footerRow.LastOrDefault().Value, @"[AА][0-9]+([xх][0-9])?").Value : Error.error.ToString();
                    //model.Scale = "N/D";
                    //model.TotalSheets = Int32.TryParse(rightCells[1].GetContent().Value, out int sheets) ? sheets : -1;
                    //model.ContractorName = rightCells[3].GetContent().Value;
                    //model.FileName = rightCells[4].GetContent().Value;
                    //model.Rev = rightCells[5].GetContent().Value;

                    var activeCell = table.SetActiveCell(rightCells[1]);                   
                    var _cell = table.MoveLeft(1);
                    elements.Add(new Element
                    {
                        Name = "Sheet",
                        Cell = _cell,
                        Content = Int32.TryParse(_cell?.GetContent()?.Value ?? "N/D", out int sheet) ? sheet : -1
                    });
                    //model.Sheet = Int32.TryParse(_cell.GetContent().Value, out int sheet) ? sheet : -1;
                    activeCell = table.SetActiveCell(_cell);
                    _cell = table.MoveLeft(1);
                    elements.Add(new Element
                    {
                        Name = "StageRu",
                        Cell = _cell,
                        Content = _cell?.GetContent()?.Value ?? "N/D"
                    });
                    //model.StageEn = _cell.GetContent().Value;
                    activeCell = table.SetActiveCell(_cell);
                    _cell = table.MoveLeft(0);
                    elements.Add(new Element
                    {
                        Name = "RusDescription",
                        Cell = _cell,
                        Content = _cell?.GetContent()?.Value ?? "N/D"
                    });
                    //model.RusDescription = _cell.GetContent().Value;
                    activeCell = table.SetActiveCell(rightCells[6]);
                    _cell = table.MoveLeft(1);
                    activeCell = table.SetActiveCell(_cell);
                    _cell = table.MoveLeft(1);
                    elements.Add(new Element
                    {
                        Name = "StageEn",
                        Cell = _cell,
                        Content = _cell?.GetContent()?.Value ?? "N/D"
                    });
                    //model.StageRu = _cell.GetContent().Value;
                    activeCell = table.SetActiveCell(_cell);
                    _cell = table.MoveLeft(0);
                    elements.Add(new Element
                    {
                        Name = "EngDescription",
                        Cell = _cell,
                        Content = _cell?.GetContent()?.Value ?? "N/D"
                    });
                    //model.EngDescription = _cell.GetContent().Value;
                    //model.ClientRev = Error.error.ToString();
                    //model.Date = new DateTime(01,01,0001);
                    //model.Status = Error.error.ToString();
                    //model.Issue = Error.error.ToString();
                    //model.PurposeIssue = Error.error.ToString();

                    var leftCells = table.LeftCells.OrderBy(r => r.Bounds.Y).ToArray();
                    var i = 0;
                    var topCell = leftCells[i];
                    while (string.IsNullOrEmpty(topCell.GetContent().Value))
                    {
                        topCell = leftCells[i++];                        
                    }
                    var topCells = table.Cells.Where(r => r.TopLeftCross.HorizontalLine.Id == topCell.TopLeftCross.HorizontalLine.Id).OrderBy(r=>r.Bounds.X).ToArray();
                    elements.Add(new Element
                    {
                        Name = "ClientRev",
                        Cell = _cell,
                        Content = topCells?[0]?.GetContent()?.Value ?? "N/D"
                    });
                    string date = string.IsNullOrEmpty(topCells?[1]?.GetContent()?.Value.Trim()) ? "N/D" : topCells?[1]?.GetContent()?.Value.Trim();
                    //model.ClientRev =  topCells[0].GetContent().Value;
                    elements.Add(new Element
                    {
                        Name = "Date",
                        Cell = topCells?[1],
                        Content = DateTime.TryParseExact(date, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out var d) ? d : new DateTime(01, 01, 0001)
                    });
                    //model.Date = DateTime.TryParse(topCells[1].GetContent().Value, out DateTime date) ? date : new DateTime(01, 01, 0001);
                    elements.Add(new Element
                    {
                        Name = "PurposeIssue",
                        Cell = topCells?[2],
                        Content = topCells?[2]?.GetContent()?.Value ?? "N/D"
                    });
                    //model.PurposeIssue = topCells[2].GetContent().Value;
                    elements.Add(new Element
                    {
                        Name = "Issue",
                        Cell = topCells?[2],
                        Content = topCells?[2]?.GetContent()?.Value?.Substring(0, 3) ?? "N/D"
                    });
                    //model.Issue =  topCells[2].GetContent().Value.Substring(0,3);                     
                    model.Issue = elements.Where(r => r.Name == "Issue").Select(r => r.Content).First().ToString();
                    model.PurposeIssue = elements.Where(r => r.Name == "PurposeIssue").Select(r => r.Content).First().ToString();
                    model.Date = (DateTime)elements.Where(r => r.Name == "Date").Select(r => r.Content).First();
                    model.ClientRev = elements.Where(r => r.Name == "ClientRev").Select(r => r.Content).First().ToString();
                    model.EngDescription = elements.Where(r => r.Name == "EngDescription").Select(r => r.Content).First().ToString();
                    model.StageRu = elements.Where(r => r.Name == "StageRu").Select(r => r.Content).First().ToString();
                    model.RusDescription = elements.Where(r => r.Name == "RusDescription").Select(r => r.Content).First().ToString();
                    model.StageEn = elements.Where(r => r.Name == "StageEn").Select(r => r.Content).First().ToString();
                    model.Sheet = (int)elements.Where(r => r.Name == "Sheet").Select(r => r.Content).First();
                    model.Rev = elements.Where(r => r.Name == "Rev").Select(r => r.Content).First().ToString();
                    model.FileName = elements.Where(r => r.Name == "FileName").Select(r => r.Content).First().ToString();
                    model.ContractorName = elements.Where(r => r.Name == "ContractorName").Select(r => r.Content).First().ToString();
                    model.TotalSheets = (int)elements.Where(r => r.Name == "TotalSheets").Select(r => r.Content).First();
                    model.PapeSize = elements.Where(r => r.Name == "PapeSize").Select(r => r.Content).First().ToString();
                    model.FooterName = elements.Where(r => r.Name == "FooterName").Select(r => r.Content).First().ToString();
                }
            }
            catch (Exception ex)
            {
                model.ClientRev = Error.warning.ToString();
                model.ContractorName = Error.warning.ToString();
                model.Date = new DateTime(01, 01, 0001);
                model.EngDescription = Error.warning.ToString();
                model.FileName = Error.warning.ToString();
                model.FooterName = Error.warning.ToString();
                model.Issue = Error.warning.ToString();
                model.PapeSize = Error.warning.ToString();
                model.PurposeIssue = Error.warning.ToString();
                model.Rev = Error.warning.ToString();
                model.RusDescription = Error.warning.ToString();
                model.Scale = Error.warning.ToString();
                model.Sheet = -1;
                model.StageEn = Error.warning.ToString();
                model.StageRu = Error.warning.ToString();
                model.Status = Error.warning.ToString();
                model.TotalSheets = -1;
            }
            try
            {
                errorCells = elements.Where(r => errStatus.Any(er => r.Content.ToString() == er)).Select(r=>r.Cell).ToList();
                var svgLink = SaveColorSvg(svg, table, errorCells, link);
                string imagePath = dir + "\\"+Regex.Replace(System.IO.Path.GetFileNameWithoutExtension(link), "_page_[0-9]+","") + ".bmp";

                Svg.SaveAsImg(svgLink, imagePath);
            }
            catch{

            }
            
            return await Task.FromResult(model);
        }

        public string SaveColorSvg(Svg svg, Table table, List<Cell> cells, string link, string tableColor = "#13F423", string errorColor = "#F52007")
        {
            string name = link.Replace("pdf", "svg");
            svg.DrawRectangle(table.Bounds, 2, tableColor);
            foreach (var cell in cells.Where(r=>r!=null))
            {
                svg.DrawRectangle(cell.Bounds, 2, errorColor);
            }
            svg.SaveAsSvg(link.Replace("pdf", "svg"));
            return name;
        }

       
        //public async Task<TestAttribute> GetFileAttributes(byte[] pdfBytes)
        //{
        //    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        //    MemoryStream stream = new MemoryStream();
        //    stream.Write(pdfBytes, 0, pdfBytes.Length);
        //    Svg svg = new Svg(stream);
        //    TestAttribute model = new TestAttribute();

        //    try
        //    {
        //        //Поиск ячеек штампа
        //        System.Drawing.RectangleF targetArea1 = new System.Drawing.RectangleF(svg.Width - svg.Width / 2, svg.Height - svg.Height / 2, svg.Width, svg.Height);

        //        List<Cell> cells = svg.GetCells(targetArea1).AsParallel()
        //            .Where(r => r.Bounds.Height > 1.5 && r.Bounds.X > 1.8)
        //            .ToList();

        //        //Группировка по строкам 
        //        var tables = svg.GetTables(cells);
        //        foreach (var t in tables[0].Cells)
        //        {
        //            svg.DrawRectangle(t.Bounds);
        //            svg.SaveAsSvg(@"D:\VS\AGCC_RD\test.svg");
        //        }
        //        var cellsGroupByY = cells.GroupBy(y => y.Bounds.Y, new ClosenessComparer(0.2f))
        //        .Select(c => new
        //        {
        //            Key = c.Key,
        //            Value = c.OrderBy(x => x.Bounds.X).ToArray()
        //        })
        //        .OrderBy(y => y.Key)
        //        .Select(r => r.Value)
        //        .ToList();
        //        if (cellsGroupByY.Count > 0)
        //        {
        //            model.FooterName = cellsGroupByY.LastOrDefault().Count() > 0 ?
        //                Regex.Match(cellsGroupByY.LastOrDefault().First().GetContent().Value, @"AGCC.*\.[a-z]+").Value
        //                : Error.error.ToString();

        //            model.PapeSize = cellsGroupByY.LastOrDefault().Count() > 0 ?
        //                Regex.Match(cellsGroupByY.LastOrDefault().Last().GetContent().Value, @"[AА][0-9]+([xх][0-9])?").Value
        //                : Error.error.ToString();

        //            model.Scale = cellsGroupByY.LastOrDefault().Count() >= 2 ?
        //                Regex.Match(cellsGroupByY.LastOrDefault()[1].GetContent().Value, @"[0-9.,]+:[0-9.,]+").Value
        //                : Error.error.ToString();

        //            model.EngDescription = Error.error.ToString();
        //            if (cellsGroupByY.Count() >= 6)
        //            {
        //                if (cellsGroupByY[6].Count() > 3)
        //                {
        //                    model.EngDescription = cellsGroupByY[6][2].GetContent().Value;
        //                }
        //            }

        //            model.StageEn = Error.error.ToString();
        //            model.Sheet = -1;
        //            model.TotalSheets = -1;
        //            if (cellsGroupByY.Count() >= 7)
        //            {
        //                if (cellsGroupByY[7].Count() >= 3)
        //                {
        //                    model.StageEn = cellsGroupByY[7].First().GetContent().Value;
        //                    model.Sheet = int.TryParse(cellsGroupByY[7][1].GetContent().Value, out int sheet) ? sheet : -1;
        //                    model.TotalSheets = int.TryParse(cellsGroupByY[7].Last().GetContent().Value, out int totalSheet) ? totalSheet : -1;
        //                }
        //            }

        //            model.ContractorName = Error.error.ToString();
        //            model.Rev = Error.error.ToString();
        //            if (cellsGroupByY.Count() >= 8 && cellsGroupByY[8].Count() >= 2)
        //            {
        //                model.ContractorName = cellsGroupByY[8].First().GetContent().Words.OrderBy(y => y.Bounds.Y).Last().Value;
        //                model.Rev = cellsGroupByY[8].Last().GetContent().Value;
        //            }

        //            model.FileName = Error.error.ToString();
        //            if (cellsGroupByY.Count() >= 10 && cellsGroupByY[10].Count() > 0)
        //            {
        //                model.FileName = cellsGroupByY[10].Last().GetContent().Value;
        //            }

        //            model.RusDescription = Error.error.ToString();
        //            if (cellsGroupByY.Count() >= 15 && cellsGroupByY[15].Count() >= 5)
        //            {
        //                model.RusDescription = cellsGroupByY[15][cellsGroupByY[15].ToList().IndexOf(cellsGroupByY[15].Last()) - 3].GetContent().Value;
        //            }

        //            model.StageRu = Error.error.ToString();
        //            if (cellsGroupByY.Count() >= 16 && cellsGroupByY[16].Count() > 3)
        //            {
        //                model.StageRu = cellsGroupByY[16][cellsGroupByY[16].ToList().IndexOf(cellsGroupByY[16].Last()) - 2].GetContent().Value;
        //            }

        //            model.ClientRev = Error.error.ToString();
        //            model.Date = new DateTime(01, 01, 0001);
        //            model.Status = Error.error.ToString();
        //            model.Issue = Error.error.ToString();
        //            model.PurposeIssue = Error.error.ToString();
        //            var group = cellsGroupByY.Where(r => r.Count() >= 5 && r.All(w => w.GetContent().Words.Count() > 0)).FirstOrDefault();
        //            if (group != null)
        //            {
        //                model.ClientRev = group[2].GetContent().Value;
        //                model.Date = DateTime.TryParse(group[3].GetContent().Value, out DateTime date) ? date : new DateTime(01, 01, 0001);
        //                model.Status = group[0].GetContent().Value;
        //                model.Issue = group[1].GetContent().Value;
        //                model.PurposeIssue = group[2].GetContent().Value;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        model.ClientRev = Error.warning.ToString();
        //        model.ContractorName = Error.warning.ToString();
        //        model.Date = new DateTime(01, 01, 0001);
        //        model.EngDescription = Error.warning.ToString();
        //        model.FileName = Error.warning.ToString();
        //        model.FooterName = Error.warning.ToString();
        //        model.Issue = Error.warning.ToString();
        //        model.PapeSize = Error.warning.ToString();
        //        model.PurposeIssue = Error.warning.ToString();
        //        model.Rev = Error.warning.ToString();
        //        model.RusDescription = Error.warning.ToString();
        //        model.Scale = Error.warning.ToString();
        //        model.Sheet = -1;
        //        model.StageEn = Error.warning.ToString();
        //        model.StageRu = Error.warning.ToString();
        //        model.Status = Error.warning.ToString();
        //        model.TotalSheets = -1;
        //    }

        //    return await Task.FromResult(model);
        //}
    }
}
