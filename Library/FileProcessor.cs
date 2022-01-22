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
        public async Task<AttributesModel> GetFileAttributes(byte[] pdfBytes, string link)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            MemoryStream stream = new MemoryStream();
            stream.Write(pdfBytes, 0, pdfBytes.Length);
            Svg svg = new Svg(stream);
            Svg.GetImage(stream);


            AttributesModel model = new AttributesModel();
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
                    var table = tables.Aggregate((t1, t2) => t1.Cells.Count() > t2.Cells.Count() ? t1 : t2);
                    var footerRow = svg.GetWords(false, 0.02f, new System.Drawing.RectangleF(table.Bounds.X, table.Bounds.Bottom, table.Bounds.Width, svg.Height));
                    SaveSvg(svg, table, link);

                    model.FooterName = footerRow.Count() > 0 ? Regex.Match(footerRow.First().Value, @"AGCC.*\.[a-z]+").Value : Error.error.ToString();
                    model.PapeSize = footerRow.Count() > 0 ?Regex.Match(footerRow.LastOrDefault().Value, @"[AА][0-9]+([xх][0-9])?").Value : Error.error.ToString();
                    model.Scale = "N/D";
                    var rightCells = table.RightCells.OrderByDescending(r=>r.Bounds.Y).ToArray();
                    model.TotalSheets = Int32.TryParse(rightCells[1].GetContent().Value, out int sheets) ? sheets : -1;
                    model.ContractorName = rightCells[3].GetContent().Value;
                    model.FileName = rightCells[4].GetContent().Value;
                    model.Rev = rightCells[5].GetContent().Value;
                    var activeCell = table.SetActiveCell(rightCells[1]);                   
                    var _cell = table.MoveLeft(1);
                    model.Sheet = Int32.TryParse(_cell.GetContent().Value, out int sheet) ? sheet : -1;
                    activeCell = table.SetActiveCell(_cell);
                    _cell = table.MoveLeft(1);
                    model.StageEn = _cell.GetContent().Value;
                    activeCell = table.SetActiveCell(_cell);
                    _cell = table.MoveLeft(0);
                    model.RusDescription = _cell.GetContent().Value;
                    activeCell = table.SetActiveCell(rightCells[6]);
                    _cell = table.MoveLeft(1);
                    activeCell = table.SetActiveCell(_cell);
                    _cell = table.MoveLeft(1);
                    model.StageRu = _cell.GetContent().Value;
                    activeCell = table.SetActiveCell(_cell);
                    _cell = table.MoveLeft(0);
                    model.EngDescription = _cell.GetContent().Value;
                    model.ClientRev = Error.error.ToString();
                    model.Date = new DateTime(01,01,0001);
                    model.Status = Error.error.ToString();
                    model.Issue = Error.error.ToString();
                    model.PurposeIssue = Error.error.ToString();

                    var leftCells = table.LeftCells.OrderBy(r => r.Bounds.Y).ToArray();
                    var i = 0;
                    var topCell = leftCells[i];
                    while (string.IsNullOrEmpty(topCell.GetContent().Value))
                    {
                        topCell = leftCells[i++];                        
                    }
                    var topCells = table.Cells.Where(r => r.TopLeftCross.HorizontalLine.Id == topCell.TopLeftCross.HorizontalLine.Id).OrderBy(r=>r.Bounds.X).ToArray();
                    model.ClientRev =  topCells[0].GetContent().Value;
                    model.Date = DateTime.TryParse(topCells[1].GetContent().Value, out DateTime date) ? date : new DateTime(01, 01, 0001);
                    model.PurposeIssue = topCells[2].GetContent().Value;
                    model.Issue =  topCells[2].GetContent().Value.Substring(0,3);                     

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

            return await Task.FromResult(model);
        }

        public void SaveSvg(Svg svg, Table table, string link)
        {
            foreach(var cell in table.Cells)
            {
                svg.DrawRectangle(cell.Bounds, 2, "#EA13F4");
            }
            svg.SaveAsSvg(link.Replace("pdf", "svg"));

        }

        public void GetImage(Svg svg, Table table, string link)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            //var svgLink = link.Replace("pdf", "svg");
            //foreach (var cell in table.Cells)
            //{
            //    svg.DrawRectangle(cell.Bounds, 2, "#EA13F4");
            //}
            //svg.SaveAsSvg(svgLink);
            //var svgBytes = svg.File;
            //var bytes = File.ReadAllBytes(link);
            //image.Save(link.Replace("pdf","bmp"));
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
