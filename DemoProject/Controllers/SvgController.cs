using DemoProject.Database.Models;
using EnigmaSvgCore;
using Microsoft.AspNetCore.Mvc;
using RDLibrary;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DemoProject.Controllers
{
    [Route("api/[controller]")]
    [Consumes("multipart/form-data")]
    [ApiController]

    public class SvgController : Controller
    {
        [HttpGet("{id}")]
        public ContentResult GetSvg(int id)
        {
            FileProcessor processor = new FileProcessor();
            FileContext fileContext = new FileContext();
            var file = fileContext.GetFile(id);
            //var svgname = file.link.Replace("pdf", "svg");
            //if (!System.IO.File.Exists(svgname))
            //{
            //    Svg.ConvertPdfToSvg(file.link, svgname);
            //}
            //var bytes = System.IO.File.ReadAllBytes(svgname);
            Svg svg = null;
            Xml xml = new Xml(svg.GetXmlLines());
            string xmlText = string.Join(Environment.NewLine, xml.Lines);
            System.Text.Encoding encoding = null;
            try
            {
                encoding = System.Text.Encoding.GetEncoding(svg.XmlEncoding);
            }
            catch
            {
                encoding = System.Text.Encoding.UTF8;
            }

            return new ContentResult()
            {
                Content = xmlText,
                ContentType = "text/plain"
            };
        }
    }
}
