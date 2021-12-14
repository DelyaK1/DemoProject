using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EnigmaSvgCore
{
    public class Pdf
    {
        public string Filename { get; set; }
        public Stream Filestream { get; set; }

        public Pdf(string filename)
        {
            Filename = filename;
            Filestream = new MemoryStream(File.ReadAllBytes(filename))
            {
                Position = 0
            };
        }

        public Pdf(Stream filestream)
        {
            filestream.CopyTo(Filestream);
            Filestream.Position = 0;
        }

        public List<string> SplitBySinglePages(string outputFolder, string baseFilename, string pageFilenameAnnex = "_page")
        {
            List<string> pagesFilenames = new List<string>();
            using (iText.Kernel.Pdf.PdfDocument pdfDoc = new iText.Kernel.Pdf.PdfDocument(new iText.Kernel.Pdf.PdfReader(Filestream)))
            {
                int totalPages = pdfDoc.GetNumberOfPages();

                for (int i = 1; i <= totalPages; i++)
                {
                    string path = outputFolder + $"\\{baseFilename}{pageFilenameAnnex}" + i.ToString() + ".pdf";
                    pagesFilenames.Add(path);
                    var pdfWriter = new iText.Kernel.Pdf.PdfWriter(path);

                    iText.Kernel.Pdf.PdfDocument pdfDoc1 = new iText.Kernel.Pdf.PdfDocument(pdfWriter);
                    pdfDoc.CopyPagesTo(i, i, pdfDoc1);
                    pdfDoc1.Close();
                }
            }
            return pagesFilenames;
        }

        public string SaveAsSvg(string outputFilename, bool removeWatermark = true)
        {
            var document = new Aspose.Pdf.Document(Filestream, false);
            document.Save(outputFilename, Aspose.Pdf.SaveFormat.Svg);

            if (removeWatermark) // remove watermark
            {
                var svgTextLines = System.IO.File.ReadAllText(outputFilename);
                var svgClearText = Regex.Replace(svgTextLines, @"[\n\r\t]+?((<text)(.)+[\n\r]+?(Evaluation)(.)+(Aspose)(.)+[\n\r]+?(.)+(<\/text>))[\n\r\t]*", "");
                System.IO.File.WriteAllText(outputFilename, svgClearText);
            }
            
            return outputFilename;
        }

        public Stream ConvertToSvgStream(bool removeWatermark = true)
        {
            var document = new Aspose.Pdf.Document(Filestream, false);
            MemoryStream ms1 = new MemoryStream();
            document.Save(ms1, Aspose.Pdf.SaveFormat.Svg);
            ms1.Position = 0;

            // remove watermark
            if (removeWatermark)
            {
                var sr = new StreamReader(ms1);
                List<string> svgTextLines = new List<string>();
                string line = null;
                while ((line = sr.ReadLine()) != null)
                {
                    svgTextLines.Add(line);
                }
                var svgClearTextLines = Regex.Replace(string.Join(Environment.NewLine, svgTextLines), 
                    @"[\n\r\t]+?((<text)(.)+[\n\r]+?(Evaluation)(.)+(Aspose)(.)+[\n\r]+?(.)+(<\/text>))[\n\r\t]*", "")
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                byte[] svgAsBytes = svgClearTextLines.SelectMany(s => System.Text.Encoding.UTF8.GetBytes(s + Environment.NewLine)).ToArray();
                MemoryStream ms2 = new MemoryStream();
                ms2.Write(svgAsBytes, 0, svgAsBytes.Length);
                return ms2;
            }
            else
            {
                return ms1;
            }
        }
    }
}
