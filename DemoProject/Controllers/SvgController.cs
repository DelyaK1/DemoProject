using EnigmaSvgCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            FileAttributesContext context = new FileAttributesContext();

            var svgname = context.GetFiles().Where(r => r.Id == id).Select(r=>r.DocumentName).First().Split(new string[] { ".pdf" }, StringSplitOptions.None)[0];
            EnigmaSvgCore.Svg.ConvertPdfToSvg(Directory.GetCurrentDirectory() + "\\wwwroot\\"+svgname+".pdf", Directory.GetCurrentDirectory() + "\\wwwroot\\" + svgname + ".svg");

            var document = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\wwwroot", "*.svg").Where(r => r.Contains(svgname)).FirstOrDefault();
            if(string.IsNullOrEmpty(document))
            {
                EnigmaSvgCore.Svg svg = new EnigmaSvgCore.Svg(document);
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
            else
            {
                return null;
            }
        }
    }
}
