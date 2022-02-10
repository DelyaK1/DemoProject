using iText.Kernel.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace EnigmaSvgCore
{
    public class Svg
    {
        private class CustomPdfSplitter : iText.Kernel.Utils.PdfSplitter
        {
            string OutputFilename { get; set; }
            public CustomPdfSplitter(iText.Kernel.Pdf.PdfDocument pdfDocument, string outputFilename) : base(pdfDocument)
            {
                OutputFilename = outputFilename;
            }

            protected override iText.Kernel.Pdf.PdfWriter GetNextPdfWriter(iText.Kernel.Utils.PageRange documentPageRange)
            {
                return new iText.Kernel.Pdf.PdfWriter(OutputFilename);
            }
        }

        public string Filename { get; }
        public byte[] File { get; }
        public float Width { get; }
        public float Height { get; }
        public string SvgVersion { get; }
        public string XmlVersion { get; }
        public string XmlEncoding { get; }
        public int PageNumber { get; set; } = 0;
        public List<Node> Nodes { get; } = new List<Node>();

        public string SaveAsSvg(string svgFilename, bool aspose = false)
        {
            try
            {
                string[] xmlRows = new string[Nodes.Count - Nodes.Where(n => n.Name == "text" && n.Type == Node.NodeType.Closing).Count() * 2];
                if (aspose)
                {
                    xmlRows = new string[Nodes.Count];
                }

                int xmlRowsCounter = 0;
                bool textNode = false;
                string xmlRow = "";
                foreach (Node xmlNode in Nodes.OrderBy(n => n.Order))
                {
                    if (textNode == false)
                    {
                        xmlRow = "";
                    }

                    if (xmlNode.Type == Node.NodeType.Opening || (xmlNode.Type == Node.NodeType.Closing && xmlNode.Name != "text") || xmlNode.Type == Node.NodeType.Single)
                    {
                        for (int t = 0; t < xmlNode.Depth; t++)
                        {
                            xmlRow += "\t";
                        }
                    }

                    switch (xmlNode.Type)
                    {
                        case Node.NodeType.XmlDeclaration:
                            xmlRow += "<?xml";
                            foreach (var item in xmlNode.Attributes)
                            {
                                xmlRow += " " + item.Key + "=\"" + item.Value + "\"";
                            }
                            xmlRow += "?>";
                            break;
                        case Node.NodeType.DocType:
                            xmlRow += $"<!DOCTYPE {xmlNode.Name}";
                            foreach (var item in xmlNode.Attributes)
                            {
                                xmlRow += " ";
                                if (item.Key != "SYSTEM")
                                {
                                    xmlRow += item.Key + " ";
                                }
                                xmlRow += "\"" + item.Value + "\"";
                            }
                            xmlRow += ">";
                            break;
                        case Node.NodeType.Comment:
                            xmlRow += "<!--" + xmlNode.Value + "-->";
                            break;
                        case Node.NodeType.CDATA:
                            xmlRow += "<![CDATA[" + xmlNode.Value + "]]>";
                            break;
                        case Node.NodeType.Content:
                            string nodeContent = xmlNode.Value.Replace("<", "&lt;");
                            nodeContent = nodeContent.Replace(">", "&gt;");
                            xmlRow += nodeContent;
                            break;
                        default:
                            if (xmlNode.Type == Node.NodeType.Closing)
                            {
                                xmlRow += "</" + xmlNode.Name + ">";
                            }
                            else
                            {
                                xmlRow += "<" + xmlNode.Name;
                                foreach (var attribute in xmlNode.Attributes)
                                {
                                    xmlRow += " " + attribute.Key + "=\"" + attribute.Value + "\"";
                                }
                                if (xmlNode.Type == Node.NodeType.Single)
                                {
                                    xmlRow += "/>";
                                }
                                else if (xmlNode.Type == Node.NodeType.Opening)
                                {
                                    xmlRow += ">";
                                }
                            }
                            break;
                    }
                    if (xmlNode.Name == "text" && xmlNode.Type == Node.NodeType.Opening)
                    {
                        textNode = true;
                    }
                    if (textNode && xmlNode.Name == "text" && xmlNode.Type == Node.NodeType.Closing)
                    {
                        textNode = false;
                    }
                    if (!textNode && xmlNode.Type != Node.NodeType.Content)
                    {
                        xmlRows[xmlRowsCounter++] = xmlRow;
                    }
                    if (!textNode && xmlNode.Type == Node.NodeType.Content)
                    {
                        xmlRows[xmlRowsCounter++] = xmlRow;
                    }
                }
                System.IO.File.WriteAllLines(svgFilename, xmlRows, System.Text.Encoding.Unicode);
                return svgFilename;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        public string SaveAsSvg2(string svgFilename)
        {
            System.IO.File.WriteAllBytes(svgFilename, File);
            return svgFilename;
        }

        public string[] GetXmlLines()
        {
            try
            {
                string[] xmlRows = new string[Nodes.Count - Nodes.Where(n => n.Name == "text" && n.Type == Node.NodeType.Closing).Count() * 2];
                int xmlRowsCounter = 0;
                bool textNode = false;
                string xmlRow = "";
                foreach (Node xmlNode in Nodes.OrderBy(n => n.Order))
                {
                    if (textNode == false)
                    {
                        xmlRow = "";
                    }

                    if (xmlNode.Type == Node.NodeType.Opening || (xmlNode.Type == Node.NodeType.Closing && xmlNode.Name != "text") || xmlNode.Type == Node.NodeType.Single)
                    {
                        for (int t = 0; t < xmlNode.Depth; t++)
                        {
                            xmlRow += "\t";
                        }
                    }

                    switch (xmlNode.Type)
                    {
                        case Node.NodeType.XmlDeclaration:
                            xmlRow += "<?xml";
                            foreach (var item in xmlNode.Attributes)
                            {
                                xmlRow += " " + item.Key + "=\"" + item.Value + "\"";
                            }
                            xmlRow += "?>";
                            break;
                        case Node.NodeType.DocType:
                            xmlRow += $"<!DOCTYPE {xmlNode.Name}";
                            foreach (var item in xmlNode.Attributes)
                            {
                                xmlRow += " ";
                                if (item.Key != "SYSTEM")
                                {
                                    xmlRow += item.Key + " ";
                                }
                                xmlRow += "\"" + item.Value + "\"";
                            }
                            xmlRow += ">";
                            break;
                        case Node.NodeType.Comment:
                            xmlRow += "<!--" + xmlNode.Value + "-->";
                            break;
                        case Node.NodeType.CDATA:
                            xmlRow += "<![CDATA[" + xmlNode.Value + "]]>";
                            break;
                        case Node.NodeType.Content:
                            xmlRow += xmlNode.Value;
                            break;
                        default:
                            if (xmlNode.Type == Node.NodeType.Closing)
                            {
                                xmlRow += "</" + xmlNode.Name + ">";
                            }
                            else
                            {
                                xmlRow += "<" + xmlNode.Name;
                                foreach (var attribute in xmlNode.Attributes)
                                {
                                    xmlRow += " " + attribute.Key + "=\"" + attribute.Value + "\"";
                                }
                                if (xmlNode.Type == Node.NodeType.Single)
                                {
                                    xmlRow += "/>";
                                }
                                else
                                {
                                    xmlRow += ">";
                                }
                            }
                            break;
                    }
                    if (xmlNode.Name == "text" && xmlNode.Type == Node.NodeType.Opening)
                    {
                        textNode = true;
                    }
                    if (textNode && xmlNode.Name == "text" && xmlNode.Type == Node.NodeType.Closing)
                    {
                        textNode = false;
                    }
                    if (!textNode && xmlNode.Type != Node.NodeType.Content)
                    {
                        xmlRows[xmlRowsCounter++] = xmlRow;
                    }
                    if (!textNode && xmlNode.Type == Node.NodeType.Content)
                    {
                        xmlRows[xmlRowsCounter++] = xmlRow;
                    }
                }
                return xmlRows;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static void SaveAsImg(string svgFilename, string imgFilename)
        {
            Spire.Pdf.PdfDocument pdf3 = new Spire.Pdf.PdfDocument();
            pdf3.LoadFromSvg(svgFilename);
            MemoryStream memStream = new MemoryStream();
            pdf3.SaveToStream(memStream);
            var bytes = memStream.ToArray();
            var img = SaveAsImage(bytes, imgFilename);
        }
        public static string SaveAsImage(byte[] file, string imageFilename)
        {
            int Resolution = 0;
           // Buffer.BlockCopy(file, 0, file, 0, file.Length);
            //System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            using (BitMiracle.Docotic.Pdf.PdfDocument pdf = new BitMiracle.Docotic.Pdf.PdfDocument(file))
            {
                var page = pdf.Pages[0];
                Resolution = (int)page.Resolution;
            }
            using (Aspose.Pdf.Document aPdf = new Aspose.Pdf.Document(new MemoryStream(file)))
            {
                
                var size = GetSizeWithRotationInt(file);
                Aspose.Pdf.Devices.BmpDevice bmpDevice = new Aspose.Pdf.Devices.BmpDevice(new Aspose.Pdf.PageSize(size.Width, size.Height), new Aspose.Pdf.Devices.Resolution(Resolution, Resolution));
                MemoryStream ms = new MemoryStream();
                bmpDevice.Process(aPdf.Pages[1], ms);
                System.IO.File.WriteAllBytes(imageFilename, ms.ToArray());
            }
            return imageFilename;
        }

        public static Rectangle GetSizeWithRotationInt(byte[] file)
        {
            string guid = Guid.NewGuid().ToString();
            string TempFolder = System.IO.Path.GetTempPath();
            string pTempFilename = TempFolder + guid + ".pdf";
            System.IO.File.WriteAllBytes(pTempFilename, file);
            using (PdfDocument pdfDoc = new PdfDocument(new PdfReader(pTempFilename)))
            {
                Delete(pTempFilename);
                var size = pdfDoc.GetPage(1).GetPageSizeWithRotation();
                return new Rectangle((int)size.GetX(), (int)size.GetY(), (int)size.GetWidth(), (int)size.GetHeight());
            }
        }

        private static bool Delete(string filename)
        {
            bool success = true;
            try
            {
                System.IO.File.Delete(filename);
            }
            catch
            {
                success = false;
            }
            return success;
        }

        public static string SaveAsPdf(string svgFilename, string pdfFilename)
        {
            try
            {
                //Guid guid = Guid.NewGuid();
                //string svgTempFilename = System.IO.Path.GetTempPath() + guid.ToString() + ".svg";
                //SaveAsSvg(svgTempFilename);
                //using (Spire.Pdf.PdfDocument pdf = new())
                //{
                //    pdf.LoadFromSvg(svgTempFilename);
                //    pdf.SaveToFile(pdfFilename, Spire.Pdf.FileFormat.PDF);
                //}
                //try
                //{
                //    System.IO.File.Delete(svgTempFilename);
                //}
                //catch { }
                return pdfFilename;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        //private Image ConvertToImage()
        //{
        //    Image pdfImage = null;
        //    using (Aspose.Pdf.Document aPdf = new Aspose.Pdf.Document(new MemoryStream(File)))
        //    {
        //        var size = GetSizeWithRotationInt();

        //        Aspose.Pdf.Devices.BmpDevice bmpDevice = new Aspose.Pdf.Devices.BmpDevice(new Aspose.Pdf.PageSize(size.Width, size.Height), new Aspose.Pdf.Devices.Resolution(Resolution, Resolution));
        //        MemoryStream ms = new MemoryStream();
        //        bmpDevice.RenderingOptions = new Aspose.Pdf.RenderingOptions { UseNewImagingEngine = true };
        //        bmpDevice.Process(aPdf.Pages[1], ms);
        //        string guid = Guid.NewGuid().ToString();
        //        string tempFilename = TempFolder + guid + ".bmp";
        //        System.IO.File.WriteAllBytes(tempFilename, ms.ToArray());
        //        pdfImage = new Image(tempFilename);
        //        try
        //        {
        //            System.IO.File.Delete(tempFilename);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //        }
        //        //using (MemoryStream ms = aPdf.Pages[1].ConvertToPNGMemoryStream())
        //        //{
        //        //    string guid = Guid.NewGuid().ToString();
        //        //    string tempFilename = TempFolder + guid + ".png";
        //        //    System.IO.File.WriteAllBytes(tempFilename, ms.ToArray());
        //        //    pdfImage = new Image(tempFilename);
        //        //    try
        //        //    {
        //        //        System.IO.File.Delete(tempFilename);
        //        //    }
        //        //    catch (Exception ex)
        //        //    {
        //        //        Console.WriteLine(ex.Message);
        //        //    }

        //        //}
        //    }
        //    return pdfImage;
        //}
        
        //public static void GetImage(string svg, int pageNumber = 1)
        //{
        //        byte[] file = new byte[File.Length];
        //        Buffer.BlockCopy(File, 0, file, 0, file.Length);
        //        //System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

        //        using (Aspose.Pdf.Document aPdf = new Aspose.Pdf.Document(new MemoryStream(file)))
        //        {
        //            var size = GetSizeWithRotationInt();
        //            Aspose.Pdf.Devices.BmpDevice bmpDevice = new Aspose.Pdf.Devices.BmpDevice(new Aspose.Pdf.PageSize(size.Width, size.Height), new Aspose.Pdf.Devices.Resolution(Resolution, Resolution));
        //            MemoryStream ms = new MemoryStream();
        //            bmpDevice.Process(aPdf.Pages[1], ms);
        //            System.IO.File.WriteAllBytes(imageFilename, ms.ToArray());
        //        }

        //        //using (Aspose.Pdf.Document aPdf = new Aspose.Pdf.Document(new MemoryStream(file)))
        //        //{
        //        //    using (MemoryStream ms = aPdf.Pages[1].ConvertToPNGMemoryStream())
        //        //    {
        //        //        System.IO.File.WriteAllBytes(imageFilename, ms.ToArray());
        //        //    }
        //        //}

        //       // return imageFilename;
            
        //    //try
        //    //{
        //    //    using (Spire.Pdf.PdfDocument pdf = new Spire.Pdf.PdfDocument(pdfBytes))
        //    //    {
        //    //        image = pdf.SaveAsImage(1, Spire.Pdf.Graphics.PdfImageType.Bitmap);
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    Console.WriteLine(ex.Message);
        //    //}
        //    //return image;
        //}

        public Svg(List<Node> svgNodes, int pageNumber = 1)
        {
            try
            {
                string[] xmlRows = new string[svgNodes.Count];
                int xmlRowsCounter = 0;
                bool textNode = false;
                string xmlRow = "";
                foreach (Node xmlNode in svgNodes.OrderBy(n => n.Order))
                {
                    if (textNode == false)
                    {
                        xmlRow = "";
                    }

                    if (xmlNode.Type == Node.NodeType.Opening || (xmlNode.Type == Node.NodeType.Closing && xmlNode.Name != "text") || xmlNode.Type == Node.NodeType.Single)
                    {
                        for (int t = 0; t < xmlNode.Depth; t++)
                        {
                            xmlRow += "\t";
                        }
                    }

                    switch (xmlNode.Type)
                    {
                        case Node.NodeType.XmlDeclaration:
                            xmlRow += "<?xml";
                            foreach (var item in xmlNode.Attributes)
                            {
                                xmlRow += " " + item.Key + "=\"" + item.Value + "\"";
                            }
                            xmlRow += "?>";
                            break;
                        case Node.NodeType.DocType:
                            xmlRow += $"<!DOCTYPE {xmlNode.Name}";
                            foreach (var item in xmlNode.Attributes)
                            {
                                xmlRow += " ";
                                if (item.Key != "SYSTEM")
                                {
                                    xmlRow += item.Key + " ";
                                }
                                xmlRow += "\"" + item.Value + "\"";
                            }
                            xmlRow += ">";
                            break;
                        case Node.NodeType.Comment:
                            xmlRow += "<!--" + xmlNode.Value + "-->";
                            break;
                        case Node.NodeType.CDATA:
                            xmlRow += "<![CDATA[" + xmlNode.Value + "]]>";
                            break;
                        case Node.NodeType.Content:
                            xmlRow += xmlNode.Value;
                            break;
                        default:
                            if (xmlNode.Type == Node.NodeType.Closing)
                            {
                                xmlRow += "</" + xmlNode.Name + ">";
                            }
                            else
                            {
                                xmlRow += "<" + xmlNode.Name;
                                foreach (var attribute in xmlNode.Attributes)
                                {
                                    xmlRow += " " + attribute.Key + "=\"" + attribute.Value + "\"";
                                }
                                if (xmlNode.Type == Node.NodeType.Single)
                                {
                                    xmlRow += "/>";
                                }
                                else
                                {
                                    xmlRow += ">";
                                }
                            }
                            break;
                    }
                    if (xmlNode.Name == "text" && xmlNode.Type == Node.NodeType.Opening)
                    {
                        textNode = true;
                    }
                    if (textNode && xmlNode.Name == "text" && xmlNode.Type == Node.NodeType.Closing)
                    {
                        textNode = false;
                    }
                    if (!textNode)
                    {
                        xmlRows[xmlRowsCounter++] = xmlRow;
                    }
                }
                string[] actualXmlRows = new string[xmlRows.Length - xmlRows.Where(e => e == null).Count()];
                xmlRowsCounter = 0;
                for (int k = 0; k < xmlRows.Length; k++) // Remove empty rows
                {
                    if (xmlRows[k] == null)
                    {
                        continue;
                    }
                    actualXmlRows[xmlRowsCounter++] = xmlRows[k];
                }

                Guid guid = Guid.NewGuid();
                string svgTempFilename = System.IO.Path.GetTempPath() + guid.ToString() + ".svg";

                System.IO.File.WriteAllLines(svgTempFilename, actualXmlRows, System.Text.Encoding.UTF8);
                Svg svgFromNodes = new Svg(svgTempFilename, pageNumber);

                Filename = svgFromNodes.Filename;
                File = System.IO.File.ReadAllBytes(svgTempFilename);
                Width = svgFromNodes.Width;
                Height = svgFromNodes.Height;
                SvgVersion = svgFromNodes.SvgVersion;
                XmlVersion = svgFromNodes.XmlVersion;
                XmlEncoding = svgFromNodes.XmlEncoding;
                Nodes = new List<Node>(svgFromNodes.Nodes);

                try
                {
                    System.IO.File.Delete(svgTempFilename);
                }
                catch { }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public Svg(string svgFilename, int pageNumber = 1)
        {
            try
            {
                File = System.IO.File.ReadAllBytes(svgFilename);
                Filename = svgFilename;
                Nodes = ReadXmlContent(svgFilename);
                PageNumber = pageNumber;
                try
                {
                    string xmlVersionAndEncoding = Nodes.Where(x => x.Type == Node.NodeType.XmlDeclaration).FirstOrDefault().Value;
                    string xmlVersion = Regex.Match(xmlVersionAndEncoding, @"(version="")[\d\.]+("")", RegexOptions.IgnoreCase).Value;
                    XmlVersion = xmlVersion.Substring(xmlVersion.IndexOf('\"') + 1, xmlVersion.Length - (xmlVersion.IndexOf('\"') + 1) - 1);
                    string xmlEncoding = Regex.Match(xmlVersionAndEncoding, @"(encoding="")[A-Za-z\d-]+("")", RegexOptions.IgnoreCase).Value;
                    XmlEncoding = xmlEncoding.Substring(xmlEncoding.IndexOf('\"') + 1, xmlEncoding.Length - (xmlEncoding.IndexOf('\"') + 1) - 1);
                }
                catch
                {
                    XmlVersion = null;
                    XmlEncoding = null;
                }
                try
                {
                    var svgVersionAndEncoding = Nodes.Where(x => x.Name == "svg" && x.Type != Node.NodeType.DocType).FirstOrDefault();
                    Width = float.Parse(svgVersionAndEncoding.Attributes["width"]);
                    Height = float.Parse(svgVersionAndEncoding.Attributes["height"]);
                    SvgVersion = svgVersionAndEncoding.Attributes["version"];
                }
                catch
                {
                    Width = -1;
                    Height = -1;
                    SvgVersion = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public Svg(Stream svgOrPdfStream, int pageNumber = 1, bool svgStream = false)
        {
            try
            {
                Filename = "";
                PageNumber = pageNumber;
                if (svgStream)
                {
                    Nodes = ReadXmlContentFromSvgStream(svgOrPdfStream);
                    File = new byte[svgOrPdfStream.Length];
                    svgOrPdfStream.Position = 0;
                    svgOrPdfStream.Read(File, 0, File.Length);
                }
                else
                {
                    using (Stream tempStream = ConvertPdfStreamToSvgStream(svgOrPdfStream))
                    {
                        File = new byte[tempStream.Length];
                        svgOrPdfStream.Position = 0;
                        tempStream.Read(File, 0, File.Length);
                        Nodes = ReadXmlContentFromSvgStream(tempStream);
                    }
                }

                try
                {
                    string xmlVersionAndEncoding = Nodes.Where(x => x.Type == Node.NodeType.XmlDeclaration).FirstOrDefault().Value;
                    string xmlVersion = Regex.Match(xmlVersionAndEncoding, @"(version="")[\d\.]+("")", RegexOptions.IgnoreCase).Value;
                    XmlVersion = xmlVersion.Substring(xmlVersion.IndexOf('\"') + 1, xmlVersion.Length - (xmlVersion.IndexOf('\"') + 1) - 1);
                }
                catch
                {
                    XmlVersion = null;
                }

                try
                {
                    string xmlVersionAndEncoding = Nodes.Where(x => x.Type == Node.NodeType.XmlDeclaration).FirstOrDefault().Value;
                    string xmlEncoding = Regex.Match(xmlVersionAndEncoding, @"(encoding="")[A-Za-z\d-]+("")", RegexOptions.IgnoreCase).Value;
                    XmlEncoding = xmlEncoding.Substring(xmlEncoding.IndexOf('\"') + 1, xmlEncoding.Length - (xmlEncoding.IndexOf('\"') + 1) - 1);
                }
                catch
                {
                    XmlEncoding = null;
                }

                var svgVersionAndEncoding = Nodes.Where(x => x.Name == "svg" && x.Type != Node.NodeType.DocType).FirstOrDefault();
                try
                {
                    Width = float.Parse(svgVersionAndEncoding.Attributes["width"]);
                    Height = float.Parse(svgVersionAndEncoding.Attributes["height"]);
                }
                catch
                {
                    Width = -1;
                    Height = -1;
                }
                try
                {
                    SvgVersion = svgVersionAndEncoding.Attributes["version"];
                }
                catch
                {
                    SvgVersion = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("============= " + ex.Message);
            }
        }

        public Svg(byte[] svgBytes, int pageNumber = 1)
        {
            try
            {
                Filename = "";
                PageNumber = pageNumber;
                using (MemoryStream svgStream = new MemoryStream())
                {
                    svgStream.Write(svgBytes, 0, svgBytes.Length);
                    svgStream.Position = 0;
                    Nodes = ReadXmlContentFromSvgStream(svgStream);
                    File = new byte[svgStream.Length];
                    svgStream.Read(File, 0, File.Length);
                }
                
                try
                {
                    string xmlVersionAndEncoding = Nodes.Where(x => x.Type == Node.NodeType.XmlDeclaration).FirstOrDefault().Value;
                    string xmlVersion = Regex.Match(xmlVersionAndEncoding, @"(version="")[\d\.]+("")", RegexOptions.IgnoreCase).Value;
                    XmlVersion = xmlVersion.Substring(xmlVersion.IndexOf('\"') + 1, xmlVersion.Length - (xmlVersion.IndexOf('\"') + 1) - 1);
                }
                catch
                {
                    XmlVersion = null;
                }

                try
                {
                    string xmlVersionAndEncoding = Nodes.Where(x => x.Type == Node.NodeType.XmlDeclaration).FirstOrDefault().Value;
                    string xmlEncoding = Regex.Match(xmlVersionAndEncoding, @"(encoding="")[A-Za-z\d-]+("")", RegexOptions.IgnoreCase).Value;
                    XmlEncoding = xmlEncoding.Substring(xmlEncoding.IndexOf('\"') + 1, xmlEncoding.Length - (xmlEncoding.IndexOf('\"') + 1) - 1);
                }
                catch
                {
                    XmlEncoding = null;
                }

                var svgVersionAndEncoding = Nodes.Where(x => x.Name == "svg" && x.Type != Node.NodeType.DocType).FirstOrDefault();
                try
                {
                    Width = float.Parse(svgVersionAndEncoding.Attributes["width"]);
                    Height = float.Parse(svgVersionAndEncoding.Attributes["height"]);
                }
                catch
                {
                    Width = -1;
                    Height = -1;
                }
                try
                {
                    SvgVersion = svgVersionAndEncoding.Attributes["version"];
                }
                catch
                {
                    SvgVersion = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void ConvertPdfToSvg(string pdfFilename, string svgFilename)
        {
            try
            {
                using (Spire.Pdf.PdfDocument pdf = new Spire.Pdf.PdfDocument(pdfFilename))
                {
                    pdf.ConvertOptions.SetPdfToSvgOptions(pdf.Pages[0].Size.Width > pdf.Pages[0].Size.Height ? pdf.Pages[0].Size.Width : pdf.Pages[0].Size.Height, pdf.Pages[0].Size.Width < pdf.Pages[0].Size.Height ? pdf.Pages[0].Size.Width : pdf.Pages[0].Size.Height);
                    pdf.SaveToFile(svgFilename, Spire.Pdf.FileFormat.SVG);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Pdf to Svg convertation error", ex);
            }
        }

        public static Svg ConvertPdfBytesToSvg(byte[] pdfPageBytes, int filePageIndex =1, int pageNumber = 1)
        {
            Svg svgPage = null;
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    memoryStream.Write(pdfPageBytes, 0, pdfPageBytes.Length);
                    using (Spire.Pdf.PdfDocument pdf = new Spire.Pdf.PdfDocument(memoryStream))
                    {
                        pdf.ConvertOptions.SetPdfToSvgOptions(pdf.Pages[filePageIndex].Size.Width, pdf.Pages[filePageIndex].Size.Height);
                        svgPage = new Svg(pdf.SaveToStream(Spire.Pdf.FileFormat.SVG)[0], pageNumber, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return svgPage;
        }

        public static Svg[] ConvertPdfBytesToSvgs(byte[] pdfPageBytes)
        {
            Svg[] svgPage = null;
            try
            {
                using (Spire.Pdf.PdfDocument pdf = new Spire.Pdf.PdfDocument(pdfPageBytes))
                {
                    svgPage = new Svg[pdf.Pages.Count];
                    for (int p = 1; p <= pdf.Pages.Count; p++)
                    {
                        pdf.ConvertOptions.SetPdfToSvgOptions(pdf.Pages[p - 1].Size.Width > pdf.Pages[p - 1].Size.Height ? pdf.Pages[p - 1].Size.Width : pdf.Pages[p - 1].Size.Height, pdf.Pages[p - 1].Size.Width < pdf.Pages[p - 1].Size.Height ? pdf.Pages[p - 1].Size.Width : pdf.Pages[p - 1].Size.Height);
                        svgPage[p - 1] = new Svg(pdf.SaveToStream(Spire.Pdf.FileFormat.SVG)[0], p, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return svgPage;
        }

        public static Stream ConvertPdfStreamToSvgStream(Stream pdfStream, bool applySizeTransform = false)
        {
            Stream svgStream = new MemoryStream();
            try
            {
                using (Spire.Pdf.PdfDocument pdf = new Spire.Pdf.PdfDocument(pdfStream))
                {
                    //pdf.ConvertOptions.SetPdfToSvgOptions(-1f, -1f);
                    if (applySizeTransform)
                    {
                        float width = pdf.Pages[0].ActualSize.Width > pdf.Pages[0].ActualSize.Height ? pdf.Pages[0].ActualSize.Width : pdf.Pages[0].ActualSize.Height;
                        float height = pdf.Pages[0].ActualSize.Width < pdf.Pages[0].ActualSize.Height ? pdf.Pages[0].ActualSize.Width : pdf.Pages[0].ActualSize.Height;
                        pdf.ConvertOptions.SetPdfToSvgOptions(width, height);
                    }
                    svgStream = pdf.SaveToStream(Spire.Pdf.FileFormat.SVG)[0];
                }
            }
            catch (Exception ex)
            {
                svgStream = null;
                Console.WriteLine(">>>>>>>> " + ex.Message);
            }
            return svgStream;
        }

        public static Stream ConvertPdfFileToPdfStream(string pdfFilename)
        {
            Stream pdfStream = new MemoryStream();
            try
            {
                using (Spire.Pdf.PdfDocument pdf = new Spire.Pdf.PdfDocument(pdfFilename))
                {
                    pdf.SaveToStream(pdfStream, Spire.Pdf.FileFormat.PDF);
                }
            }
            catch (Exception ex)
            {
                pdfStream = null;
                Console.WriteLine(ex.Message);
            }
            return pdfStream;
        }

        public static Stream ConvertPdfFileToSvgStream(string pdfFilename, int pageNumber, bool applySizeTransform = false)
        {
            Stream svgStream = new MemoryStream();
            try
            {
                using (Spire.Pdf.PdfDocument pdf = new Spire.Pdf.PdfDocument(pdfFilename))
                {
                    //pdf.ConvertOptions.SetPdfToSvgOptions(-1f, -1f);
                    if (applySizeTransform)
                    {
                        float width = pdf.Pages[pageNumber - 1].ActualSize.Width > pdf.Pages[pageNumber - 1].ActualSize.Height ? pdf.Pages[pageNumber - 1].ActualSize.Width : pdf.Pages[pageNumber - 1].ActualSize.Height;
                        float height = pdf.Pages[pageNumber - 1].ActualSize.Width < pdf.Pages[pageNumber - 1].ActualSize.Height ? pdf.Pages[pageNumber - 1].ActualSize.Width : pdf.Pages[pageNumber - 1].ActualSize.Height;
                        pdf.ConvertOptions.SetPdfToSvgOptions(width, height);
                    }
                    svgStream = pdf.SaveToStream(Spire.Pdf.FileFormat.SVG)[pageNumber - 1];
                }
            }
            catch (Exception ex)
            {
                svgStream = null;
                Console.WriteLine(ex.Message);
            }
            return svgStream;
        }

        public static void ConvertSvgToPdf(string svgFilename, string pdfFilename)
        {
            try
            {
                using (Spire.Pdf.PdfDocument pdf = new Spire.Pdf.PdfDocument())
                {
                    pdf.ConvertOptions.SetPdfToSvgOptions(-1f, -1f);
                    pdf.LoadFromSvg(svgFilename);
                    pdf.SaveToFile(pdfFilename, Spire.Pdf.FileFormat.PDF);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private List<Node> ReadXmlContent(string svgFilename)
        {
            int totalXmlNodes = 0;
            List<Node> allXmlNodes = new List<Node>();
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Parse,
                IgnoreWhitespace = true,
                IgnoreComments = false,
                
            };

            using (XmlReader reader = XmlReader.Create(svgFilename, xmlReaderSettings))
            {
                while (reader.Read())
                {
                    totalXmlNodes++;
                    Node xmlNode = new Node
                    {
                        Name = reader.Name,
                        Order = totalXmlNodes,
                        Depth = reader.Depth,
                        Type = Node.NodeType.Opening
                    };

                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Text:
                            xmlNode.Type = Node.NodeType.Content;
                            break;
                        case XmlNodeType.EndElement:
                            xmlNode.Type = Node.NodeType.Closing;
                            break;
                        case XmlNodeType.XmlDeclaration:
                            xmlNode.Type = xmlNode.Name.ToLower() == "xml" ? Node.NodeType.XmlDeclaration : Node.NodeType.Opening;
                            break;
                        case XmlNodeType.DocumentType:
                            xmlNode.Type = Node.NodeType.DocType;
                            break;
                        case XmlNodeType.Comment:
                            xmlNode.Type = Node.NodeType.Comment;
                            break;
                        case XmlNodeType.CDATA:
                            xmlNode.Type = Node.NodeType.CDATA;
                            break;
                        default:
                            if (reader.IsEmptyElement)
                            {
                                xmlNode.Type = Node.NodeType.Single;
                            }
                            break;
                    }

                    if (reader.HasAttributes)
                    {
                        while (reader.MoveToNextAttribute())
                        {
                            if (!xmlNode.Attributes.ContainsKey(reader.Name))
                            {
                                xmlNode.Attributes.Add(reader.Name, reader.Value);
                            }
                        }
                        reader.MoveToElement();
                    }

                    if (reader.HasValue)
                    {
                        xmlNode.Value = reader.Value.Replace("&", "&amp;");
                    }

                    allXmlNodes.Add(xmlNode);
                }
            }
            return allXmlNodes;
        }

        public static List<Node> GetXmlContent(string svgFilename)
        {
            int totalXmlNodes = 0;
            List<Node> allXmlNodes = new List<Node>();
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Parse,
                IgnoreWhitespace = true,
                IgnoreComments = false
            };

            using (XmlReader reader = XmlReader.Create(svgFilename, xmlReaderSettings))
            {
                while (reader.Read())
                {
                    totalXmlNodes++;
                    Node xmlNode = new Node
                    {
                        Name = reader.Name,
                        Order = totalXmlNodes,
                        Depth = reader.Depth,
                        Type = Node.NodeType.Opening
                    };

                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Text:
                            xmlNode.Type = Node.NodeType.Content;
                            break;
                        case XmlNodeType.EndElement:
                            xmlNode.Type = Node.NodeType.Closing;
                            break;
                        case XmlNodeType.XmlDeclaration:
                            xmlNode.Type = xmlNode.Name.ToLower() == "xml" ? Node.NodeType.XmlDeclaration : Node.NodeType.Opening;
                            break;
                        case XmlNodeType.DocumentType:
                            xmlNode.Type = Node.NodeType.DocType;
                            break;
                        case XmlNodeType.Comment:
                            xmlNode.Type = Node.NodeType.Comment;
                            break;
                        case XmlNodeType.CDATA:
                            xmlNode.Type = Node.NodeType.CDATA;
                            break;
                        default:
                            if (reader.IsEmptyElement)
                            {
                                xmlNode.Type = Node.NodeType.Single;
                            }
                            break;
                    }

                    if (reader.HasAttributes)
                    {
                        while (reader.MoveToNextAttribute())
                        {
                            if (!xmlNode.Attributes.ContainsKey(reader.Name))
                            {
                                xmlNode.Attributes.Add(reader.Name, reader.Value);
                            }
                        }
                        reader.MoveToElement();
                    }

                    if (reader.HasValue)
                    {
                        xmlNode.Value = reader.Value.Replace("&", "&amp;");
                    }

                    allXmlNodes.Add(xmlNode);
                }
            }
            return allXmlNodes;
        }

        public static List<Node> ReadXmlContentFromSvgStream(Stream svgStream)
        {
            svgStream.Position = 0;
            int totalXmlNodes = 0;
            List<Node> allXmlNodes = new List<Node>();
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Parse,
                IgnoreWhitespace = true,
                IgnoreComments = false
            };

            using (XmlReader reader = XmlReader.Create(svgStream, xmlReaderSettings))
            {
                while (reader.Read())
                {
                    totalXmlNodes++;
                    Node xmlNode = new Node
                    {
                        Name = reader.Name,
                        Order = totalXmlNodes,
                        Depth = reader.Depth,
                        Type = Node.NodeType.Opening
                    };

                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Text:
                            xmlNode.Type = Node.NodeType.Content;
                            break;
                        case XmlNodeType.EndElement:
                            xmlNode.Type = Node.NodeType.Closing;
                            break;
                        case XmlNodeType.XmlDeclaration:
                            xmlNode.Type = xmlNode.Name.ToLower() == "xml" ? Node.NodeType.XmlDeclaration : Node.NodeType.Opening;
                            break;
                        case XmlNodeType.DocumentType:
                            xmlNode.Type = Node.NodeType.DocType;
                            break;
                        case XmlNodeType.Comment:
                            xmlNode.Type = Node.NodeType.Comment;
                            break;
                        case XmlNodeType.CDATA:
                            xmlNode.Type = Node.NodeType.CDATA;
                            break;
                        default:
                            if (reader.IsEmptyElement)
                            {
                                xmlNode.Type = Node.NodeType.Single;
                            }
                            break;
                    }

                    if (reader.HasAttributes)
                    {
                        while (reader.MoveToNextAttribute())
                        {
                            if (!xmlNode.Attributes.ContainsKey(reader.Name))
                            {
                                xmlNode.Attributes.Add(reader.Name, reader.Value);
                            }
                        }
                        reader.MoveToElement();
                    }

                    if (reader.HasValue)
                    {
                        xmlNode.Value = reader.Value.Replace("&", "&amp;");
                    }

                    allXmlNodes.Add(xmlNode);
                }
            }
            return allXmlNodes;
        }

        public Text[] GetTexts(bool hidden = false, bool includeEmptyTexts = false, System.Drawing.RectangleF? bounds = null)
        {
            var textOpeningNodes = Nodes.AsParallel().Where(n => n.Name == "text" && n.Type == Node.NodeType.Opening && n.Hidden() == hidden).OrderBy(n => n.Order).ToArray();

            List<Text> texts = new List<Text>();
            object _lock = new object();

            //foreach (var a in textOpeningNodes)
            //{
            //    try
            //    {
            //        Text text = new Text(a, Nodes[a.Order], Nodes[a.Order + 1]);
            //        texts.Add(text);
            //    }
            //    catch
            //    {
            //        Text text = new Text(a, Nodes[a.Order], Nodes[a.Order + 1]);
            //    }
            //}
            
            textOpeningNodes.AsParallel().ForAll(a =>
            {
                Text text = new Text(a, Nodes[a.Order], Nodes[a.Order + 1]);
                
                lock (_lock)
                {
                    texts.Add(text);
                }
            });

            Text[] resultTexts = new Text[0];
            if (bounds != null)
            {
                resultTexts = texts.AsParallel().Where(t => t.Bounds.Left > bounds.Value.Left && t.Bounds.Left < bounds.Value.Right && t.Bounds.Bottom > bounds.Value.Top && t.Bounds.Bottom < bounds.Value.Bottom).ToArray();
            }
            else
            {
                resultTexts = texts.ToArray();
            }

            if (!includeEmptyTexts)
            {
                resultTexts = resultTexts.Where(t => !string.IsNullOrEmpty(t.Value)).ToArray();
            }

            return resultTexts;
        }

        public Word[] GetWords(bool hidden = false, float leanDeviation = 0.02f, System.Drawing.RectangleF? bounds = null)
        {
            var texts = GetTexts(hidden, false, bounds).ToList();
            
            var rotationGroups = texts.GroupBy(r => r.Rotation).ToList();
            List<Word> words = new List<Word>();

            object _lock = new object();
            foreach (var rotationGroup in rotationGroups)
            {
                switch (rotationGroup.Key)
                {
                    case 0:
                        List<Text> texts0 = rotationGroup.Select(x => x).Where(v => !string.IsNullOrEmpty(v.Value)).OrderBy(v => v.Symbols.First().Point.X).ToList();
                        bool anyWordExist0 = true;
                        while (anyWordExist0)
                        {
                            var prevLetter = texts0.First();
                            texts0.Remove(prevLetter);
                            List<Text> word = new List<Text>
                            {
                                prevLetter
                            };
                            bool nextLetterExist = true;
                            while (nextLetterExist)
                            {
                                var nextLetters = texts0.AsParallel().Where(v => v.Symbols.First().Point.X > prevLetter.Symbols.Last().Point.X && Math.Abs(v.Symbols.First().Point.X - prevLetter.Symbols.Last().Point.X) < v.FontSize - (v.FontSize / 3) && Math.Abs(prevLetter.Symbols.Last().Point.Y - v.Symbols.First().Point.Y) < leanDeviation);
                                Text nextLetter = null;
                                if (nextLetters.Count() > 0)
                                {
                                    nextLetter = nextLetters.AsParallel().Aggregate((a, b) => a.Symbols.First().Point.X < b.Symbols.First().Point.X ? a : b);
                                }
                                if (nextLetter == null)
                                {
                                    nextLetterExist = false;
                                }
                                else
                                {
                                    word.Add(nextLetter);
                                    prevLetter = nextLetter;
                                    texts0.Remove(nextLetter);
                                }
                            }
                            //string tagValue = "";
                            //for (int k = 0; k < word.Count; k++)
                            //{
                            //    tagValue += word[k].Value;
                            //}
                            if (texts0.Count == 0)
                            {
                                anyWordExist0 = false;
                            }
                            lock (_lock)
                            {
                                words.Add(new Word(word.ToArray()));
                            }
                        }
                        break;
                    case 90:
                        List<Text> texts90 = rotationGroup.Select(y => y).Where(v => !string.IsNullOrEmpty(v.Value)).OrderBy(v => v.Symbols.First().Point.Y).ToList();
                        bool anyWordExist90 = true;
                        while (anyWordExist90)
                        {
                            var prevLetter = texts90.First();
                            texts90.Remove(prevLetter);
                            List<Text> word = new List<Text>
                            {
                                prevLetter
                            };
                            bool nextLetterExist = true;
                            while (nextLetterExist)
                            {
                                var nextLetters = texts90.AsParallel().Where(v => v.Symbols.First().Point.Y > prevLetter.Symbols.Last().Point.Y && Math.Abs(v.Symbols.First().Point.Y - prevLetter.Symbols.Last().Point.Y) < v.FontSize - (v.FontSize / 3) && Math.Abs(prevLetter.Symbols.Last().Point.X - v.Symbols.First().Point.X) < leanDeviation);
                                Text nextLetter = null;
                                if (nextLetters.Count() > 0)
                                {
                                    nextLetter = nextLetters.AsParallel().Aggregate((a, b) => a.Symbols.First().Point.Y > b.Symbols.First().Point.Y ? a : b);
                                }
                                if (nextLetter == null)
                                {
                                    nextLetterExist = false;
                                }
                                else
                                {
                                    word.Add(nextLetter);
                                    prevLetter = nextLetter;
                                    texts90.Remove(nextLetter);
                                }
                            }
                            //string tagValue = "";
                            //for (int k = 0; k < word.Count; k++)
                            //{
                            //    tagValue += word[k].Value;
                            //}
                            if (texts90.Count == 0)
                            {
                                anyWordExist90 = false;
                            }
                            lock (_lock)
                            {
                                words.Add(new Word(word.ToArray()));
                            }
                        }
                        break;
                    case 180:
                        List<Text> texts180 = rotationGroup.Select(y => y).Where(v => !string.IsNullOrEmpty(v.Value)).OrderByDescending(v => v.Symbols.First().Point.X).ToList();
                        bool anyWordExist180 = true;
                        while (anyWordExist180)
                        {
                            var prevLetter = texts180.First();
                            texts180.Remove(prevLetter);
                            List<Text> word = new List<Text>
                            {
                                prevLetter
                            };
                            bool nextLetterExist = true;
                            while (nextLetterExist)
                            {
                                var nextLetters = texts180.AsParallel().Where(v => v.Symbols.First().Point.X < prevLetter.Symbols.Last().Point.X && Math.Abs(v.Symbols.First().Point.X - prevLetter.Symbols.Last().Point.X) < v.FontSize - (v.FontSize / 3) && Math.Abs(prevLetter.Symbols.Last().Point.Y - v.Symbols.First().Point.Y) < leanDeviation);
                                Text nextLetter = null;
                                if (nextLetters.Count() > 0)
                                {
                                    nextLetter = nextLetters.AsParallel().Aggregate((a, b) => a.Symbols.First().Point.X > b.Symbols.First().Point.X ? a : b);
                                }
                                if (nextLetter == null)
                                {
                                    nextLetterExist = false;
                                }
                                else
                                {
                                    word.Add(nextLetter);
                                    prevLetter = nextLetter;
                                    texts180.Remove(nextLetter);
                                }
                            }
                            //string tagValue = "";
                            //for (int k = 0; k < word.Count; k++)
                            //{
                            //    tagValue += word[k].Value;
                            //}
                            if (texts180.Count == 0)
                            {
                                anyWordExist180 = false;
                            }
                            lock (_lock)
                            {
                                words.Add(new Word(word.ToArray()));
                            }
                        }
                        break;
                    case 270:
                        List<Text> texts270 = rotationGroup.Select(y => y).Where(v => !string.IsNullOrEmpty(v.Value)).OrderByDescending(v => v.Symbols.First().Point.Y).ToList();
                        bool anyWordExist270 = true;
                        while (anyWordExist270)
                        {
                            var prevLetter = texts270.First();
                            texts270.Remove(prevLetter);
                            List<Text> word = new List<Text>
                            {
                                prevLetter
                            };
                            bool nextLetterExist = true;
                            while (nextLetterExist)
                            {
                                var nextLetters = texts270.AsParallel().Where(v => v.Symbols.First().Point.Y < prevLetter.Symbols.Last().Point.Y && Math.Abs(v.Symbols.First().Point.Y - prevLetter.Symbols.Last().Point.Y) < v.FontSize - (v.FontSize / 3) && Math.Abs(prevLetter.Symbols.Last().Point.X - v.Symbols.First().Point.X) < leanDeviation);
                                Text nextLetter = null;
                                if (nextLetters.Count() > 0)
                                {
                                    nextLetter = nextLetters.AsParallel().Aggregate((a, b) => a.Symbols.First().Point.Y > b.Symbols.First().Point.Y ? a : b);
                                }
                                if (nextLetter == null)
                                {
                                    nextLetterExist = false;
                                }
                                else
                                {
                                    word.Add(nextLetter);
                                    prevLetter = nextLetter;
                                    texts270.Remove(nextLetter);
                                }
                            }
                            //string tagValue = "";
                            //for (int k = 0; k < word.Count; k++)
                            //{
                            //    tagValue += word[k].Value;
                            //}
                            if (texts270.Count == 0)
                            {
                                anyWordExist270 = false;
                            }
                            lock (_lock)
                            {
                                words.Add(new Word(word.ToArray()));
                            }
                        }
                        break;
                }
            }

            return words.ToArray();
        }

        public List<Path> GetPaths()
        {
            var firstGSectionOrder = Nodes.Where(n => n.Name == "g").OrderBy(o => o.Order).First();
            return Nodes.AsParallel().Where(o => o.Order > firstGSectionOrder.Order).AsParallel().Where(n => n.Name == "path").Select(x => new Path(x)).ToList();
        }

        public List<Line> GetLines(float arcRatio = 5.0f, bool notHiddenOnly = true, bool notDashedOnly = true)
        {
            var paths = GetPaths();

            //var path = paths.Where(p => p.Node.Attributes["d"] == "M1904.88 1108.56L2494.32 1108.56z").FirstOrDefault();

            //int order = -1;
            //if (path != null)
            //{
            //    order = path.Node.Order;
            //}

            paths.AsParallel().ForAll(p => p.ApplyTransformMatrix());
            paths.AsParallel().ForAll(p => p.Commands = Command.IdentifyAllCommandNames(p.Commands));

            List<Line> lines = new List<Line>();

            object _lock = new object();
            Parallel.ForEach(paths, modPath =>
            {
                //if (modPath.Node.Order == order)
                //{
                //    ;
                //}

                if (notHiddenOnly)
                {
                    if (modPath.Node.IsAttributeExist("stroke"))
                    {
                        if (modPath.Node.Attributes["stroke"] == "none" || modPath.Node.Attributes["stroke"] == "#FFFFFF")
                        {
                            if (modPath.Node.Attributes["fill"] == "#FFFFFF")
                            {
                                return;
                            }
                        }
                    }
                }

                if (notDashedOnly)
                {
                    if (modPath.Node.IsAttributeExist("stroke-dasharray"))
                    {
                        return;
                    }
                }

                for (int i = 0; i < modPath.Commands.Length; i++)
                {
                    if (modPath.Commands[i].Name == Command.CommandName.L)
                    {
                        Point point1 = modPath.Commands[i - 1].GetPoints().Last();
                        Point point2 = modPath.Commands[i].GetPoints().First();
                        Line line = new Line(point1, point2)
                        {
                            Paths = new List<Path>(new List<Path> { modPath })
                        };
                        if (line.Length > 0)
                        {
                            lock (_lock)
                            {
                                lines.Add(line);
                            }
                        }
                    }
                    else if (modPath.Commands[i].Name == Command.CommandName.C) // если у команды 'C' все одинаковые X (вертикальная линия) или все одинаковые Y (горизонтальная линия)
                    {
                        var point_M = modPath.Commands[i - 1].GetPoints().Last();
                        var points_C = modPath.Commands[i].GetPoints();
                        float firstX = point_M.X;
                        float firstY = point_M.Y;
                        bool isItLine = true;

                        if (Math.Abs(point_M.X - points_C.Last().X) > (Math.Abs(point_M.Y - points_C.Last().Y) / 100.0f) * arcRatio) // X (vertical)
                        {
                            isItLine = false;
                        }
                        else
                        {
                            var curveLineX = Math.Abs(point_M.X + points_C.Last().X) / 2;
                            var arcPointX = Math.Abs(curveLineX - points_C[0].X) > Math.Abs(curveLineX - points_C[1].X) ? points_C[0].X : points_C[1].X;
                            if (Math.Abs(curveLineX - arcPointX) > (Math.Abs(point_M.Y - points_C.Last().Y) / 100.0f) * arcRatio)
                            {
                                isItLine = false;
                            }
                        }
                        if (!isItLine)
                        {
                            isItLine = true;
                            if (Math.Abs(point_M.Y - points_C.Last().Y) > (Math.Abs(point_M.X - points_C.Last().X) / 100.0f) * arcRatio) // (horizontal)
                            {
                                isItLine = false;
                            }
                            else
                            {
                                var curveLineY = Math.Abs(point_M.Y + points_C.Last().Y) / 2;
                                var arcPointY = Math.Abs(curveLineY - points_C[0].Y) > Math.Abs(curveLineY - points_C[1].Y) ? points_C[0].Y : points_C[1].Y;
                                if (Math.Abs(curveLineY - arcPointY) > (Math.Abs(point_M.X - points_C.Last().X) / 100.0f) * arcRatio)
                                {
                                    isItLine = false;
                                }
                            }
                            if (isItLine)
                            {
                                Point point1 = point_M;
                                Point point2 = modPath.Commands[i].GetPoints().Last();
                                Line line = new Line(point1, point2)
                                {
                                    Paths = new List<Path>(new List<Path> { modPath })
                                };
                                if (line.Length > 0)
                                {
                                    lock (_lock)
                                    {
                                        lines.Add(line);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Point point1 = point_M;
                            Point point2 = modPath.Commands[i].GetPoints().Last();
                            Line line = new Line(point1, point2)
                            {
                                Paths = new List<Path>(new List<Path> { modPath })
                            };
                            if (line.Length > 0)
                            {
                                lock (_lock)
                                {
                                    lines.Add(line);
                                }
                            }
                        }
                    }
                    else if (modPath.Commands[i].Name == Command.CommandName.Z || modPath.Commands[i].Name == Command.CommandName.z)
                    {
                        Point point1 = modPath.Commands[i - 1].GetPoints().Last();
                        Point point2 = modPath.Commands.First().GetPoints().First();
                        Line line = new Line(point1, point2)
                        {
                            Paths = new List<Path>(new List<Path> { modPath })
                        };
                        if (line.Length > 0)
                        {
                            lock (_lock)
                            {
                                lines.Add(line);
                            }
                        }
                    }
                }

                if (modPath.Commands.Where(c => c.Name == Command.CommandName.z || c.Name == Command.CommandName.Z).Count() > 0)
                {
                    Point point1 = modPath.Commands.Where(c => c.Name != Command.CommandName.z && c.Name != Command.CommandName.Z).Last().GetPoints().Last();
                    Point point2 = modPath.Commands.First().GetPoints().Last();
                    Line line = new Line(point1, point2)
                    {
                        Paths = new List<Path>(new List<Path> { modPath })
                    };
                    if (line.Length > 0)
                    {
                        lock (_lock)
                        {
                            lines.Add(line);
                        }
                    }
                }
            });

            int idCounter = 0;
            lines.ForEach(a => a.Id = idCounter++);

            return lines;
        }
        /// <summary>
        /// Находит все ячейки в заданной области
        /// </summary>
        /// <param name="targetArea">Область для поиска</param>
        /// <param name="arcRatio">Коэффициент допустимого изгиба линии</param>
        /// <param name="continuousLineMinLengthH">Минимальная длина конечных горизонтальных линий, на основе которых будет производиться поиск ячеек.</param>
        /// <param name="continuousLineMinLengthV">Минимальная длина конечных вертикалльных линий, на основе которых будет производиться поиск ячеек.</param>
        /// <param name="leanRatio">Коэффициент допустимого наклона, чтобы считать линию всё еще вертикальной/горизонтальной.</param>
        /// <param name="spacing">Максимально допустимый "разрыв" между линиями (можно использовать для случаев, когда линии в таблице не прорисованы до конца).</param>
        /// <param name="distance">Максимально допустимое расстояние между соседними линиями, когда мы всё еще считаем, что это одна линия и их можно рассматривать как отрезки одной линии.</param>
        /// <param name="notHiddenOnly">Если True, то будет производиться поиск только видимых линий. Если False, то невидимые линии и линии белого цвета также будут использоваться для поиска.</param>
        /// <param name="notDashedOnly">Если True, то пунктирные и штрихпунктирные линии будут игнорироваться. Если False, то данные линии будут учитываться при поиске также как и обычные.</param>
        /// <returns></returns>
        public List<Cell> GetCells(System.Drawing.RectangleF targetArea, float arcRatio = 5.0f, float continuousLineMinLengthH = 20.0f, float continuousLineMinLengthV = 20.0f, float leanRatio = 2.0f, float spacing = 3.0f, float distance = 1.0f, float noiseFilter = 6.0f, bool notHiddenOnly = true, bool notDashedOnly = true)
        {
            var lines = GetLines(arcRatio, notHiddenOnly, notDashedOnly);
            List<Line> horizontalLines = Line.ConcatenateHLines(lines, targetArea, continuousLineMinLengthH, leanRatio, spacing, distance, noiseFilter);
            List<Line> verticalLines = Line.ConcatenateVLines(lines, targetArea, continuousLineMinLengthV, leanRatio, spacing, distance, noiseFilter);

            //var lines = GetLines(arcRatio, notHiddenOnly, notDashedOnly);
            //var h = TaskH(lines);
            //var v = TaskV(lines);

            //Task.WhenAll(h, v);
            //List<Line> horizontalLines = h.Result;
            //List<Line> verticalLines = v.Result;

            List<Cross> allCrosses = new List<Cross>();
            if (verticalLines.Count < horizontalLines.Count) // для ускорения процесса поиска пересечений
            {
                foreach (var vLine in verticalLines)
                {
                    allCrosses.AddRange(vLine.GetHorizontalCrossLines(horizontalLines, spacing));
                }
            }
            else
            {
                foreach (var hLine in horizontalLines)
                {
                    allCrosses.AddRange(hLine.GetVerticalCrossLines(verticalLines, spacing));
                }
            }

            List<Cell> cells = new List<Cell>();
            object _lock = new object();
            
            Parallel.ForEach(allCrosses, topCross =>
            {
                var cell = (from rightCross in allCrosses.Where(c => c.HorizontalLine.Id == topCross.HorizontalLine.Id && c.Point.X > topCross.Point.X)
                            from bottomCross in allCrosses.Where(c => c.VerticalLine.Id == rightCross.VerticalLine.Id && c.Point.Y > rightCross.Point.Y)
                            from leftCross in allCrosses.Where(c => c.HorizontalLine.Id == bottomCross.HorizontalLine.Id && c.VerticalLine.Id == topCross.VerticalLine.Id)
                            select new Cell(this, topCross, rightCross, leftCross, bottomCross))
                            .ToList();
                if (cell.Count > 0)
                {
                    var singleCell = cell.OrderBy(c1 => c1.TopRightCross.Point.X).ThenBy(c2 => c2.BottomLeftCross.Point.Y).First();
                    lock (_lock)
                    {
                        cells.Add(singleCell);
                    }
                }
            });

            int idCounter = 0;
            cells.ForEach(c => c.Id = idCounter++);

            return cells;
        }

        private async Task<List<Line>> TaskH(List<Line> lines)
        {
            List<Line> res = new List<Line>();
            await Task.Run(() =>
            {
                res = Line.ConcatenateHLines(lines, new System.Drawing.RectangleF());
            });
            return res;
        }

        private async Task<List<Line>> TaskV(List<Line> lines)
        {
            List<Line> res = new List<Line>();
            await Task.Run(() =>
            {
                res = Line.ConcatenateVLines(lines, new System.Drawing.RectangleF());
            });

            return res;
        }

        public List<Table> GetTables(List<Cell> allCells, int minCellsInsideTable = 4, ICellsProcessor cellsProcessor = null)
        {
            if (cellsProcessor == null)
            {
                cellsProcessor = new DefaultCellsProcessor();
            }
            var cellsByTables = cellsProcessor.FindTableCells(allCells, minCellsInsideTable);
            List<Table> tables = new List<Table>();
            int counterId = 0;
            foreach (var cells in cellsByTables)
            {
                //var timer1 = new Stopwatch();
                //timer1.Start();
                tables.Add(new Table(this, cells, counterId++));
                //timer1.Stop();
                //Console.WriteLine("Total cells: " + item.Count.ToString() + " => " + timer1.Elapsed.TotalSeconds.ToString());
            }

            return tables;
        }

        public void InsertNodes(List<Node> newNodes)
        {
            int firstNewOrder = -1;
            int depth = -1;
            for (int n = Nodes.Count; n > 0; n--)
            {
                if (Nodes[n - 1].Name == "g")
                {
                    firstNewOrder = Nodes[n - 1].Order;
                    depth = Nodes[n - 1].Depth + 1;

                    for (int k = n - 1; k < Nodes.Count; k++) // увеличиваем Order у всех Node на количество добавляемых Nodes, идущих после g
                    {
                        Nodes[k].Order += newNodes.Count;
                    }

                    break;
                }
            }

            if (firstNewOrder != -1)
            {
                foreach (var node in newNodes)
                {
                    node.Order = firstNewOrder++;
                    node.Depth = depth;
                    Nodes.Add(node);
                }
            }
        }

        public void DrawCircle(Point center, float radius, float borderWidth = 2.0f, string hexColor = "#ff0000", string fillProperty = "none")
        {
            Node newNode = new Node()
            {
                Name = "circle",
                Type = Node.NodeType.Single,
                Attributes = new Dictionary<string, string>()
                {
                    { "cx", center.X.ToString() },
                    { "cy", center.Y.ToString() },
                    { "r", radius.ToString() },
                    { "stroke", hexColor },
                    { "stroke-width", borderWidth.ToString() },
                    { "fill", fillProperty }
                }
            };
            InsertNodes(new List<Node> { newNode });
        }

        public void DrawRectangle(System.Drawing.RectangleF bounds, float borderWidth = 2.0f, string hexColor = "#ff0000", string fillProperty = "none", string nClass = null, string nId = null)
        {
            Node newNode = new Node()
            {
                Name = "rect",
                Type = Node.NodeType.Single,
                Attributes = new Dictionary<string, string>()
                {
                    { "x", bounds.X.ToString(CultureInfo.InvariantCulture) },
                    { "y", bounds.Y.ToString(CultureInfo.InvariantCulture) },
                    { "width", bounds.Width.ToString(CultureInfo.InvariantCulture) },
                    { "height", bounds.Height.ToString(CultureInfo.InvariantCulture) },
                    { "style", $"fill:{fillProperty};stroke:{hexColor};stroke-width:{borderWidth};" }
                }
            };

            if (!string.IsNullOrWhiteSpace(nClass))
            {
                newNode.AddAttribute("class", nClass);
            }

            if (!string.IsNullOrWhiteSpace(nId))
            {
                newNode.AddAttribute("id", nId);
            }

            InsertNodes(new List<Node> { newNode });
        }

        public void DrawLine(Point point1, Point point2, float width = 1.0f, string hexColor = "#ff0000")
        {
            Node newNode = new Node()
            {
                Name = "line",
                Type = Node.NodeType.Single,
                Attributes = new Dictionary<string, string>()
                {
                    { "x1", point1.X.ToString() },
                    { "y1", point1.Y.ToString() },
                    { "x2", point2.X.ToString() },
                    { "y2", point2.Y.ToString() },
                    { "style", $"stroke:{hexColor};stroke-width:{width};" }
                }
            };
            InsertNodes(new List<Node> { newNode });
        }

        public static byte[] ExtractPageFromPdfAsSeparatePdfFile(Stream pdfFileStream, int pageNumber)
        {
            try
            {
                if (pageNumber < 1)
                {
                    pageNumber = 1;
                }
                pdfFileStream.Position = 0;
                using (iText.Kernel.Pdf.PdfDocument pdfDoc = new iText.Kernel.Pdf.PdfDocument(new iText.Kernel.Pdf.PdfReader(pdfFileStream)))
                {
                    int totalPages = pdfDoc.GetNumberOfPages();
                    if (pageNumber > totalPages)
                    {
                        pageNumber = totalPages;
                    }

                    byte[] result;
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        var pdfWriter = new iText.Kernel.Pdf.PdfWriter(memoryStream);

                        iText.Kernel.Pdf.PdfDocument pdfDoc1 = new iText.Kernel.Pdf.PdfDocument(pdfWriter);
                        pdfDoc.CopyPagesTo(pageNumber, pageNumber, pdfDoc1);
                        pdfDoc1.Close();
                        result = memoryStream.ToArray();
                    }

                    return result;
                }
            }
            catch
            {
                return null;
            }
        }

        public static byte[] ExtractPageFromPdfAsSeparatePdfFile(string pdfFilename, int pageNumber)
        {
            try
            {
                if (pageNumber < 1)
                {
                    pageNumber = 1;
                }

                using (iText.Kernel.Pdf.PdfDocument pdfDoc = new iText.Kernel.Pdf.PdfDocument(new iText.Kernel.Pdf.PdfReader(pdfFilename)))
                {
                    int totalPages = pdfDoc.GetNumberOfPages();
                    if (pageNumber > totalPages)
                    {
                        pageNumber = totalPages;
                    }

                    byte[] result;
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        var pdfWriter = new iText.Kernel.Pdf.PdfWriter(memoryStream);

                        iText.Kernel.Pdf.PdfDocument pdfDoc1 = new iText.Kernel.Pdf.PdfDocument(pdfWriter);
                        pdfDoc.CopyPagesTo(pageNumber, pageNumber, pdfDoc1);
                        pdfDoc1.Close();
                        result = memoryStream.ToArray();
                    }

                    return result;
                }
            }
            catch
            {
                return null;
            }
        }

        public static string ExtractPagesRangeFromPdf(Stream pdfFileStream, int fromPageNumber, int toPageNumber, string outputPdfFilename)
        {
            pdfFileStream.Position = 0;
            try
            {
                using (iText.Kernel.Pdf.PdfDocument pdfDoc = new iText.Kernel.Pdf.PdfDocument(new iText.Kernel.Pdf.PdfReader(pdfFileStream)))
                {
                    iText.Kernel.Pdf.PdfDocument extractedPages = new CustomPdfSplitter(pdfDoc, outputPdfFilename).ExtractPageRange(new iText.Kernel.Utils.PageRange(fromPageNumber + "-" + toPageNumber));
                    try
                    {
                        extractedPages.Close();
                    }
                    catch { }
                    try
                    {
                        pdfDoc.Close();
                    }
                    catch { }

                    return outputPdfFilename;
                }
            }
            catch
            {
                return null;
            }
        }

        public static string ExtractPagesRangeFromPdf(string pdfFilename, int fromPageNumber, int toPageNumber, string outputPdfFilename)
        {
            try
            {
                using (iText.Kernel.Pdf.PdfDocument pdfDoc = new iText.Kernel.Pdf.PdfDocument(new iText.Kernel.Pdf.PdfReader(pdfFilename)))
                {
                    iText.Kernel.Pdf.PdfDocument extractedPages = new CustomPdfSplitter(pdfDoc, outputPdfFilename).ExtractPageRange(new iText.Kernel.Utils.PageRange(fromPageNumber + "-" + toPageNumber));
                    try
                    {
                        extractedPages.Close();
                    }
                    catch { }
                    try
                    {
                        pdfDoc.Close();
                    }
                    catch { }

                    return outputPdfFilename;
                }
            }
            catch
            {
                return null;
            }
        }

        public static List<string> SplitPdfBySinglePage(string pdfFilename, string outputPdfFolder, string filenamePrefix = "page_")
        {
            try
            {
                List<string> paths = new List<string>();
                using (iText.Kernel.Pdf.PdfDocument pdfDoc = new iText.Kernel.Pdf.PdfDocument(new iText.Kernel.Pdf.PdfReader(pdfFilename)))
                {
                    int totalPages = pdfDoc.GetNumberOfPages();
                    
                    for (int i = 1; i <= totalPages; i++)
                    {
                        string path = outputPdfFolder + $"\\{filenamePrefix}" + i.ToString() + ".pdf";
                        paths.Add(path);
                        var pdfWriter = new iText.Kernel.Pdf.PdfWriter(path);

                        iText.Kernel.Pdf.PdfDocument pdfDoc1 = new iText.Kernel.Pdf.PdfDocument(pdfWriter);
                        pdfDoc.CopyPagesTo(i, i, pdfDoc1);
                        pdfDoc1.Close();
                    }
                    return paths;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Split Error", ex);
            }
        }

        public static List<string> SplitPdfBySinglePage(Stream pdfStream, string outputPdfFolder, string filenamePrefix = "page_")
        {
            try
            {
                List<string> paths = new List<string>();
                pdfStream.Position = 0;
                using (iText.Kernel.Pdf.PdfDocument pdfDoc = new iText.Kernel.Pdf.PdfDocument(new iText.Kernel.Pdf.PdfReader(pdfStream)))
                {
                    int totalPages = pdfDoc.GetNumberOfPages();

                    for (int i = 1; i <= totalPages; i++)
                    {
                        string path = outputPdfFolder + $"\\{filenamePrefix}" + i.ToString() + ".pdf";
                        paths.Add(path);
                        var pdfWriter = new iText.Kernel.Pdf.PdfWriter(path);

                        using (iText.Kernel.Pdf.PdfDocument pdfDoc1 = new iText.Kernel.Pdf.PdfDocument(pdfWriter))
                        {
                            try
                            {
                                pdfDoc.CopyPagesTo(i, i, pdfDoc1);
                            }
                            catch (StackOverflowException ex)
                            {
                                throw new Exception(ex.Message);
                            }
                        }
                    }
                    return paths;
                }
            }
            catch
            {
                return null;
            }
        }

        public static int GetPdfPagesCount(Stream pdfFileStream)
        {
            int pagesCount = 0;
            pdfFileStream.Position = 0;
            using (iText.Kernel.Pdf.PdfDocument pdfDoc = new iText.Kernel.Pdf.PdfDocument(new iText.Kernel.Pdf.PdfReader(pdfFileStream)))
            {
                pagesCount = pdfDoc.GetNumberOfPages();
            }

            return pagesCount;
        }
    }
}