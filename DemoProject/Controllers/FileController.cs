using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DemoProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class FileController : Controller
    {
        [HttpGet("{name}")]
        public EnigmaSvgCore.Svg File(string name)
        {
            string pdfname = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", name + ".pdf");
            string svgname = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", name + ".svg");
            try
            {
                EnigmaSvgCore.Svg.ConvertPdfToSvg(pdfname, svgname);
                EnigmaSvgCore.Svg svg = new EnigmaSvgCore.Svg(svgname);

                System.IO.File.Delete(pdfname);

                return svg;
            }
            catch
            {
                return null;
            }

        }
    }
}
